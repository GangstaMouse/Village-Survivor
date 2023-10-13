public abstract class GameMode
{
    public abstract void Init();
    public abstract void Run();
    public abstract void End();
}

public sealed class DefaultGameMode : GameMode
{
    public override void Init()
    {
        throw new System.NotImplementedException();
    }

    public override void Run()
    {
        throw new System.NotImplementedException();
    }

    public override void End()
    {
        throw new System.NotImplementedException();
    }
}