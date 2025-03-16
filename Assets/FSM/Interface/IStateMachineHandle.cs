namespace BeykozEdu.FSM.Interfaces
{
    public interface IStateMachineHandler<T> where T : IBaseStateData
    {
        void AddState(IState<T> state, T stateData);

        void RemoveState();

        void UpdateStates();
    }
}