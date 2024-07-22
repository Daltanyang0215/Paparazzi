
public interface IState
{
    public bool canExecute { get; }

    public void OnStateExecution();
    public void OnStateEnter();
    public void OnStateExit();
}
