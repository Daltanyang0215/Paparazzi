using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 이름도 바꿔줘야 됨
public class FSMStateMachine
{
    // 오너 필요
    public MainGameManager Owner {  get; private set; } 

    #region Compoments
    // 스테이트 전체 공유 컴포넌트 작성

    #endregion

    public MainGameState current = MainGameState.Start;
    public Dictionary<MainGameState, IState> states = new Dictionary<MainGameState, IState>();

    public FSMStateMachine(MainGameManager owner)
    {
        this.Owner = owner;

        StateInit();
    }

    private void StateInit()
    {
        // 스테이트 종류 작성. 
        //states.Add(MainGameState.Start, new CreatureIdle(Owner.GetCreatureData, this));

    }

    public void ChangeState(MainGameState newstate)
    {
        if (current == newstate) return;
        if (states[newstate].canExecute == false) return;

        states[current].OnStateExit();
        states[newstate].OnStateEnter();

        current = newstate;
    }

    public void StateExecution()
    {
        states[current].OnStateExecution();
    }
}
