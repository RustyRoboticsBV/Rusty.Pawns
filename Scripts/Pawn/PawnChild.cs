using Godot;
using System.Collections.Generic;

namespace Modules.L2.Pawns
{
    /// <summary>
    /// Base class for pawn component nodes.
    /// </summary>
    [GlobalClass]
    [Icon("res://Nerves/Modules/Level 2/Pawns/Core/Pawn/PawnChild.svg")]
    public abstract partial class PawnComponent : Node3D
    {
        /* Public properties. */
        [Export] public bool Enabled { get; set; } = true;
        [Export] public virtual bool Discoverable { get; set; } = true;

        public Pawn Pawn { get; private set; }
        public List<PawnComponent> Children { get; private set; }
        public List<Raycaster> Raycasters { get; private set; }
        public List<Condition> Conditions { get; private set; }
        public List<Trigger> Triggers { get; private set; }
        public List<StateMachine> StateMachines { get; private set; }
        public List<ActionProperties> Properties { get; private set; }
        public List<Action> Actions { get; private set; }
        public List<Modifier> Modifiers { get; private set; }

        /* Godot overrides. */
        public sealed override void _EnterTree() { }

        public sealed override void _Ready() { }

        /* Public methods. */
        /// <summary>
        /// Check if this pawn child is active. This checks if the enabled toggle is set to true, and if all of its conditions
        /// are met (if there are any).
        /// </summary>
        public bool CheckActive(Pawn pawn)
        {
            if (!Enabled)
                return false;

            for (int i = 0; i < Conditions.Count; i++)
            {
                if (!Conditions[i].Evaluate(pawn))
                    return false;
            }

            return true;
        }

        public void Init(Pawn pawn)
        {
            Pawn = pawn;

            Children = new();
            Conditions = new();
            Raycasters = new();
            Triggers = new();
            StateMachines = new();
            Properties = new();
            Actions = new();
            Modifiers = new();
            GetChildren(this, pawn);

            OnInit(pawn);
        }

        /* Protected methods. */
        /// <summary>
        /// Called when this pawn component is initialized.
        /// </summary>
        protected virtual void OnInit(Pawn pawn) { }

        /* Private methods. */
        private void GetChildren(Node parent, Pawn pawn)
        {
            for (int i = 0; i < parent.GetChildCount(); i++)
            {
                Node node = parent.GetChild(i);
                if (node is PawnComponent child)
                {
                    Children.Add(child);
                    if (child is Condition condition)
                        Conditions.Add(condition);
                    if (child is Raycaster raycaster)
                        Raycasters.Add(raycaster);
                    if (child is Trigger trigger)
                        Triggers.Add(trigger);
                    if (child is StateMachine stateMachine)
                        StateMachines.Add(stateMachine);
                    if (child is ActionProperties properties)
                        Properties.Add(properties);
                    if (child is Action action)
                        Actions.Add(action);
                    if (child is Modifier modifier)
                        Modifiers.Add(modifier);
                    child.Init(pawn);
                }
                else
                    GetChildren(node, pawn);
            }
        }
    }
}
