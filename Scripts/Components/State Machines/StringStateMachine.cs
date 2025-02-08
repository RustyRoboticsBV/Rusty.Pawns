using Godot;

namespace Modules.L2.Pawns
{
    /// <summary>
    /// A state machine that uses strings as states.
    /// </summary>
	[GlobalClass]
    public partial class StringStateMachine : StateMachineWithType<string>
    {
        /* Private variables. */
        [Export] private string state = "";

        /* Public properties. */
        public override sealed string State
        {
            get => state;
            protected set => state = value;
        }

        /* Public methods. */
        public override sealed bool IsIn(string state)
        {
            return State == state;
        }
    }
}