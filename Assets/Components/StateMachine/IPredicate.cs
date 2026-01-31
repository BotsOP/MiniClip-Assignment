namespace Components.StateMachine
{
    public interface IPredicate
    {
        bool Evaluate();
        void OnEnter();
    }
}
