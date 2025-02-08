using Godot;
using System;

namespace Modules.L2.Pawns
{
    /// <summary>
    /// A state machine that uses integer states. You can also use enum values.
    /// </summary>
	[GlobalClass]
    public partial class IntStateMachine : StateMachineWithType<int>
    {
        /* Private variables. */
        [Export] private int state;

        /* Public properties. */
        public override sealed int State
        {
            get => state;
            protected set => state = value;
        }

        /* Public methods. */
        /// <summary>
        /// Enter a new state.
        /// </summary>
        public void Enter(Enum state)
        {
            Enter(Convert.ToInt32(state));
        }

        public override sealed bool IsIn(int state)
        {
            return State == state;
        }

        /// <summary>
        /// Check if the state machine is in a specific state.
        /// </summary>
        public bool IsIn(Enum state)
        {
            return IsIn(Convert.ToInt32(state));
        }
    }
}