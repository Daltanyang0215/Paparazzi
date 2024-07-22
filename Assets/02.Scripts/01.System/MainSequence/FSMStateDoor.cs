using System;
using UnityEngine;

public class FSMStateDoor : FSMStateBase
{
    public FSMStateDoor(DaySystem machine, UICanvasBase uiCanvas) : base(machine, uiCanvas) { }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        _machine.AddDayCount();
        MainGameManager.Instance.RandomSelectMapData();
        MainUIManager.Instance.MemoPanel.HidePanel();
    }
    public override void OnStateExit()
    {
    }
}
