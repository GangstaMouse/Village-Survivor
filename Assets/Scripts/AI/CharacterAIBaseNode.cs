using Frameworks.BehaviourTree;

public abstract class CharacterAIBaseNode : Node
{
    protected readonly Character m_Controller;
    protected readonly AIInputHandlerInst m_InputHandler;

    public CharacterAIBaseNode(in Character controller, in AIInputHandlerInst inputHandler)
    {
        m_Controller = controller;
        m_InputHandler = inputHandler;
    }
}
