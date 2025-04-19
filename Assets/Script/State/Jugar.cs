using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jugar : Humano
{
    private MovementController _movementController;
    private bool reachedDestination = false;
    private PathMover _pathMover;
    private void Awake()
    {
        typestate = TypeState.Jugar;
        LocadComponent();
        _movementController = GetComponent<MovementController>();
        _pathMover = GetComponent<PathMover>();

    }
    public override void LocadComponent()
    {
        base.LocadComponent();
        
    }
    // Método que se invoca al finalizar el movimiento
    private void OnArrival()
    {
        
        reachedDestination = true;

        if (_pathMover != null)
        {
            _pathMover.StartPathMovement();
        }
        // Se puede colocar aquí cualquier inicialización adicional que se quiera ejecutar al llegar
        Debug.Log("He llegado al destino. Empieza la lógica de Jugar.");
    }
    
    public override void Enter()
    {
        reachedDestination = false; // Asegúrate de resetear la bandera al entrar al estado
        // Primero mueve al personaje al destino asignado 
        if (_movementController != null && destination != null)
        {
            _movementController.MoveTo(destination, OnArrival);
        }
        else
        {
            // Si no hay destino asignado o MovementController, ejecuta la lógica sin desplazarse
            OnArrival();
        }
    }
    public override void Execute()
    {
        if (!reachedDestination)
            return;

        // Prioridad 1: Chequear energía para comer
        if (_DataAgent.Energy.value < 0.25f)
        {
            _StateMachine.ChangeState(TypeState.Comer);
            return; // Salir inmediatamente después de cambiar estado
        }

        // Prioridad 2: Chequear sueño para dormir
        if (_DataAgent.Sleep.value >= 0.75f)
        {
            _StateMachine.ChangeState(TypeState.Dormir);
            return;
        }

        // Prioridad 3: Chequear necesidad de baño
        if (_DataAgent.WC.value >= 0.6f)
        {
            _StateMachine.ChangeState(TypeState.Banno);
            return;
        }

        // Aumentar necesidad de baño mientras juega (agrega esto)
        _DataAgent.IncreaseWC();
        // Si no hay necesidades urgentes, sigue jugando
        _DataAgent.DiscountEnergy();

      

        base.Execute();
    }

    public override void Exit()
    {
       
        _DataAgent.StopLoadingEnergy(); // Detiene la recarga de energía
        if (_pathMover != null)
        {
            _pathMover.StopPathMovement();
        }
    }
}
