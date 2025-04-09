using Assets.root.Runtime.Utilities.Patterns.StateMachine.Predicates;

namespace Assets.root.Runtime.Utilities.Patterns.StateMachine
{
    public interface ITransition
    {
        IState To { get; }
        IPredicate Condition { get; }
    }
}