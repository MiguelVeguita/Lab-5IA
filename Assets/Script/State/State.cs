using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public enum TypeState { Jugar,Comer,Banno,Dormir}
public class State : MonoBehaviour
{
    public TypeState typestate;
    public StateMachine _StateMachine;
    public TMP_Text TextState;
    public Transform destination;
    public virtual void LocadComponent()
    {
        _StateMachine = GetComponent<StateMachine>();
    }
    public virtual void Enter()
    {
        if (TextState != null)
            TextState.text = typestate.ToString();
    }
    public virtual void Execute()
    {
        if(TextState!=null)
        TextState.text = typestate.ToString();
    }
    public virtual void Exit()
    {
     
    }
}
