using Godot;

namespace Modules.L2.Pawns
{
    /// <summary>
    /// A state machine that can be in either a "true" state or a "false" state.
    /// </summary>
    [GlobalClass]
    public partial class BoolStateMachine : StateMachineWithType<bool>
    {
        /* Private variables. */
        [Export] private bool state;

        /* Public properties. */
        public override sealed bool State
        {
            get => state;
            protected set => state = value;
        }

        /* Public methods. */
        public override sealed bool IsIn(bool state)
        {
            return State == state;
        }
    }
}