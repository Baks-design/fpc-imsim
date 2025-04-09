using Assets.root.Runtime.Utilities.Patterns.StateMachine;

namespace Assets.root.Runtime.Movement.States
{
    public class FallingState : IState
    {
        readonly PlayerController controller;

        public FallingState(PlayerController controller) => this.controller = controller;
    }
}