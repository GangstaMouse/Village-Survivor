using Frameworks.BehaviourTree;

public abstract class CharacterAIBaseNode : Node
{
    protected readonly Character m_Controller;
    protected readonly AIInputHandler m_InputHandler;

    public CharacterAIBaseNode(in Character controller, in AIInputHandler inputHandler)
    {
        m_Controller = controller;
        m_InputHandler = inputHandler;
    }
}
