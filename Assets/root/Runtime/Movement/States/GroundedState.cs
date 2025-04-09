using Assets.root.Runtime.Utilities.Patterns.StateMachine;

namespace Assets.root.Runtime.Movement.States
{
    public class GroundedState : IState
    {
        readonly PlayerController controller;

        public GroundedState(PlayerController controller) => this.controller = controller;
    }
}