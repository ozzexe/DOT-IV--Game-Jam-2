namespace BeykozEdu.FSM.Interfaces
{
    public interface IState<T> where T : IBaseStateData
    {
        void Init(IStateMachineHandler<T> stateMachineHandler, T data);

        void OnEnter();

        void OnUpdate();

        void OnExit();
    }
}