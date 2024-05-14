using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine
{
    protected IState currentStage;

    public void ChangeState(IState newstate)
    {
        currentStage?.Exit();

        currentStage = newstate;

        currentStage?.Enter();
    }
    public void Execute()
    {
        currentStage?.Execute();
    }
}
