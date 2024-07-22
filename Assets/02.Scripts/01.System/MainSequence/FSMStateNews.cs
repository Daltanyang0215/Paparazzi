using System;
using UnityEngine;

public class FSMStateNews : FSMStateBase
{
    public FSMStateNews(DaySystem machine, UICanvasBase uiCanvas) : base(machine, uiCanvas)
    {
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        MainGameManager.Instance.DailyStart();
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
        MainUIManager.Instance.DoorPanel.HidePanel();
    }
}
