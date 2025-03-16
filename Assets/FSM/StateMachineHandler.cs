using System.Collections.Generic;
using BeykozEdu.FSM.Interfaces;

namespace BeykozEdu.FSM
{
    public class StateMachineHandler<T> : IStateMachineHandler<T> where T : IBaseStateData
    {
        private readonly Stack<IState<T>> _states = new();
        private bool HasAnyState => _states.Count > 0;

        public void AddState(IState<T> state, T stateData)
        {
            if (HasAnyState)
                _states.Peek().OnExit();

            state.Init(this, stateData);
            state.OnEnter();
            _states.Push(state);
        }

        public void RemoveState()
        {
            if (!HasAnyState)
                return;

            _states.Pop().OnExit();
            if (HasAnyState)
                _states.Peek().OnEnter();
        }

        public void UpdateStates()
        {
            if (!HasAnyState)
                return;

            _states.Peek().OnUpdate();
        }
    }
}