using System;
using Assets.root.Runtime.Utilities.Patterns.StateMachine.Predicates;

namespace Assets.root.Runtime.Utilities.Patterns.StateMachine
{
    public abstract class Transition
    {
        public IState To { get; protected set; }
        public abstract bool Evaluate();
    }

    public class Transition<T> : Transition
    {
        public readonly T condition;

        public Transition(IState to, T condition)
        {
            To = to;
            this.condition = condition;
        }

        public override bool Evaluate() => condition switch
        {
            Func<bool> func => func.Invoke(),
            ActionPredicate actionPredicate => actionPredicate.Evaluate(),
            IPredicate predicate => predicate.Evaluate(),
            _ => false
        };
    }
}