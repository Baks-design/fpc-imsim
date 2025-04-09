using UnityEngine;

namespace Assets.root.Runtime.Utilities.Patterns.StateMachine.Predicates
{
    public class Not : IPredicate
    {
        [SerializeField] IPredicate rule;

        public bool Evaluate() => !rule.Evaluate();
    }
}