using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 이름도 바꿔줘야 됨
public class DaySystem
{
    #region Singleton
    private static DaySystem _instance;
    public static DaySystem Instance
    {
        get
        {
            if (_instance == null)
                _instance = new DaySystem(MainGameManager.Instance);
            return _instance;
        }
    }
    #endregion

    #region Property
    [field: SerializeField] public bool IsCameraMove { get; set; }
    public void AddDayCount() => SaveSystem.Save.CurSave.DayCount++;
    #endregion

    #region Compoments
    // 스테이트 전체 공유 컴포넌트 작성
    public MainGameManager Owner { get; private set; }

    #endregion

    public DayState Current { get; private set; } = DayState.Start;
    public Dictionary<DayState, IState> states = new Dictionary<DayState, IState>();

    public DaySystem(MainGameManager owner)
    {
        this.Owner = owner;

        StateInit();
    }

    private void StateInit()
    {
        // 스테이트 종류 작성. 
        //states.Add(MainGameState.Start, new CreatureIdle(Owner.GetCreatureData, this));

        states.Add(DayState.Start, new FSMStateStart(this, null));
        states.Add(DayState.Intro, new FSMStateIntro(this, null));
        states.Add(DayState.Door, new FSMStateDoor(this, MainUIManager.Instance.DoorPanel));
        states.Add(DayState.News, new FSMStateNews(this, MainUIManager.Instance.NewsPanel));
        states.Add(DayState.Capture, new FSMStateCapture(this, MainUIManager.Instance.CapturePanel));
        states.Add(DayState.Requester, new FSMStateRequester(this, MainUIManager.Instance.PhotoSetPanel));
        states.Add(DayState.Calculate, new FSMStateCalculate(this, MainUIManager.Instance.CalculatePanel));
        states.Add(DayState.Ending, new FSMStateEnding(this, null));
    }

    public void ChangeState(bool IsFade)
    {
        // 엔딩 페이지가 없으니 일단 정산이훌로 넘기기로. 엔딩에서도 엔딩 체크만 하고 넘길 예정
        ChangeState(Current >= DayState.Calculate ? DayState.Door : Current + 1, IsFade);
    }

    public void ChangeState(DayState newstate, bool IsFade = false)
    {
        if (Current == newstate) return;
        if (states[newstate].canExecute == false) return;
        
        if (IsFade)
        {
            MainUIManager.Instance.FadeEffect(() => ChangeState(newstate));
            return;
        }
        states[Current].OnStateExit();
        states[newstate].OnStateEnter();

        Current = newstate;
    }

    public void StateExecution()
    {
        states[Current].OnStateExecution();
    }
}

public enum DayState
{
    Start,
    Intro,
    Door,
    News,
    Capture,
    Requester,
    Calculate,
    Ending,
}