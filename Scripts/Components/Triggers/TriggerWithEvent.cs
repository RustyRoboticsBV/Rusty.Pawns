using Godot;
using System;

namespace Modules.L2.Pawns
{
    public partial class TriggerWithEvent<EventArgsT> : Node
    {
        /// <summary>
        /// The event that gets invoked by the trigger.
        /// </summary>
        public event Action<EventArgsT> Triggered;
    }
}