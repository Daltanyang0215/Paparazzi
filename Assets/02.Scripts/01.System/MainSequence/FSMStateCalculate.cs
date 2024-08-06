using System;
using UnityEngine;

public class FSMStateCalculate : FSMStateBase
{
    public FSMStateCalculate(DaySystem machine, UICanvasBase uiCanvas) : base(machine, uiCanvas)
    {
    }

    public override void OnStateExit()
    {
        SaveSystem.SaveData();
        base.OnStateExit();
    }
}
