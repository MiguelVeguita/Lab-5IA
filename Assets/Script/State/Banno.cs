using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banno : Humano
{
    private MovementController _movementController;
    private bool reachedDestination = false;

    private void Awake()
    {
        typestate = TypeState.Banno;
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
        _DataAgent.EmptyWC();

        _DataAgent.WC.timeFrameRate = _DataAgent.WC.timeRate;
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

        // Reduce la necesidad de ir al baño
        _DataAgent.ReduceWC();

        // Si la necesidad está por debajo del 10%, vuelve a Jugar
        if (_DataAgent.WC.value <= 0.1f)
        {
            _StateMachine.ChangeState(TypeState.Jugar);
        }
    }

    public override void Exit()
    {

    }
}
