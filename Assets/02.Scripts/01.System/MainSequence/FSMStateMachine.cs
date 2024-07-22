using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// �̸��� �ٲ���� ��
public class FSMStateMachine
{
    // ���� �ʿ�
    public MainGameManager Owner {  get; private set; } 

    #region Compoments
    // ������Ʈ ��ü ���� ������Ʈ �ۼ�

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
        // ������Ʈ ���� �ۼ�. 
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
