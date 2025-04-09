using System.Collections.Generic;
using UnityEngine;

namespace Assets.root.Runtime.Utilities.Patterns.StateMachine.Predicates
{
    public class And : IPredicate
    {
        [SerializeField] List<IPredicate> rules = new();

        public bool Evaluate()
        {
            foreach (var rule in rules)
                if (!rule.Evaluate())
                    return false;
            return true;
        }
    }
}