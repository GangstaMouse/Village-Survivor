public interface IInputReceiver
{
    public abstract InputHandlerInstance InputHandler { get; protected set; }

    internal void SetInputHandler(in InputHandlerInstance inputHandler)
    {
        InputHandler = inputHandler;
        OnInputHandlerChanged(inputHandler);
    }
    
    internal protected abstract void OnInputHandlerChanged(in InputHandlerInstance inputHandler);
}
