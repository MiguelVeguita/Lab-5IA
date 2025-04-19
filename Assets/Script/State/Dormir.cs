using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dormir : Humano
{
    private MovementController _movementController;
    private bool reachedDestination = false;

    private void Awake()
    {
        typestate = TypeState.Dormir;
        LocadComponent();
        _movementController = GetComponent<MovementController>();

    }
    public override void LocadComponent()
    {
        base.LocadComponent();

    }
    private void OnArrival()
    {
        reachedDestination = true;
        _DataAgent.LoadEnergy();
        // Se puede colocar aquí cualquier inicialización adicional que se quiera ejecutar al llegar
        Debug.Log("He llegado al destino. Empieza la lógica de Jugar.");
    }
   
    public override void Enter()
    {
        reachedDestination = false;
        if (_movementController != null && destination != null)
        {
            _movementController.MoveTo(destination, OnArrival);
        }
        else
        {
            OnArrival();
        }

    }
    public override void Execute()
    {
        if (!reachedDestination)
            return;

        base.Execute();

        _DataAgent.DecreaseSleep(); // Reduce el sueño

        // Si el sueño baja al 25%, vuelve a jugar
        if (_DataAgent.Sleep.value <= 0.25f)
        {
            _StateMachine.ChangeState(TypeState.Jugar);
        }
    }
    public override void Exit()
    {

    }
}
