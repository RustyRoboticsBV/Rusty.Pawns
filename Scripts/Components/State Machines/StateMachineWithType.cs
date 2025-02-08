using System;

namespace Modules.L2.Pawns
{
    public partial class StateMachineWithType<T> : StateMachine
    {
        /* Public properties. */
        public virtual T State { get; protected set; }

        /* Public methods. */
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override void Enter(object newState)
        {
            if (newState is T typedState)
                Enter(typedState);
            else
            {
                throw new ArgumentException($"The state machine '{Name}' does not accept states of type "
                    + $"'{newState.GetType().Name}'.");
            }
        }

        public override bool IsIn(object state)
        {
            if (state is T typedState)
                return IsIn(typedState);
            else
            {
                throw new ArgumentException($"The state machine '{Name}' does not accept states of type "
                    + $"'{state.GetType().Name}'.");
            }
        }

        /// <summary>
        /// Enter a new state.
        /// </summary>
        public void Enter(T newState)
        {
            T prevState = State;
            PreEnter(prevState, newState);
            State = newState;
            PostEnter(prevState, newState);
        }

        /// <summary>
        /// Check if the state machine is in a specific state.
        /// </summary>
        public virtual bool IsIn(T state)
        {
            return State.Equals(state);
        }

        /* Operators. */
        public static implicit operator T(StateMachineWithType<T> stateMachine) => stateMachine.State;

        /* Protected methods. */
        /// <summary>
        /// Gets called right before entering a new state.
        /// </summary>
        protected virtual void PreEnter(T currentState, T nextState) { }

        /// <summary>
        /// Gets called right after entering a new state.
        /// </summary>
        protected virtual void PostEnter(T prevState, T currentState) { }
    }
}