using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// �̸��� �ٲ���� ��
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
    // ������Ʈ ��ü ���� ������Ʈ �ۼ�
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
        // ������Ʈ ���� �ۼ�. 
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
        // ���� �������� ������ �ϴ� �������Ƿ� �ѱ���. ���������� ���� üũ�� �ϰ� �ѱ� ����
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