using System;
using UnityEngine;

public abstract class FSMStateBase : IState
{
    //protected FSMData _data;
    //protected FMSMachine _machine;

    //public FSMStateBase(FSMData data, FMSMachine machine)
    //{
    //    _data = data;
    //    _machine = machine;
    //}

    public virtual bool canExecute => true;

    public virtual void OnStateEnter()
    {
        
    }

    public virtual void OnStateExecution()
    {
        
    }

    public virtual void OnStateExit()
    {
        
    }
}
