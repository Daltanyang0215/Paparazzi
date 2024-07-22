public abstract class FSMStateBase : IState
{
    protected DaySystem _machine;
    protected UICanvasBase _uiCanvas;
    public FSMStateBase(DaySystem machine, UICanvasBase uiCanvas)
    {
        _machine = machine;
        _uiCanvas = uiCanvas;
    }

    public virtual bool canExecute => true;

    public virtual void OnStateEnter()
    {
        _uiCanvas?.ShowPanel();
    }

    public virtual void OnStateExecution()
    {
        
    }

    public virtual void OnStateExit()
    {
        _uiCanvas?.HidePanel();
    }
}
