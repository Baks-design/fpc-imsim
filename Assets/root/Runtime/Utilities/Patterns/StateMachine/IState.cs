namespace Assets.root.Runtime.Utilities.Patterns.StateMachine
{
    public interface IState
    {
        public void OnEnter() { }
        public void FixedUpdate() { }
        public void Update() { }
        public void OnExit() { }
    }
}