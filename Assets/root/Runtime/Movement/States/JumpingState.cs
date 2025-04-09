using Assets.root.Runtime.Utilities.Patterns.StateMachine;

namespace Assets.root.Runtime.Movement.States
{
    public class JumpingState : IState
    {
        readonly PlayerController controller;

        public JumpingState(PlayerController controller) => this.controller = controller;
    }
}