using BeykozEdu.FSM.Interfaces;

namespace BeykozEdu.FSM
{
    public abstract class BaseState<T> : IState<T> where T : IBaseStateData
    {
        protected IStateMachineHandler<T> StateMachineHandler;
        protected T StateData;

        public void Init(IStateMachineHandler<T> stateMachineHandler, T data)
        {
            StateMachineHandler = stateMachineHandler;
            StateData = data;
        }

        public abstract void OnEnter();

        public abstract void OnUpdate();

        public abstract void OnExit();
    }
}