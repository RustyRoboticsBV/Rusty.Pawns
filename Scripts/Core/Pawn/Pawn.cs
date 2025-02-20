using Godot;

namespace Rusty.Pawns
{
    /// <summary>
    /// The top-layer node for character controllers.
    /// </summary>
    [GlobalClass]
    [Icon("./Pawn.svg")]
    public partial class Pawn : Node3D
    {
        /* Public properties. */
        /// <summary>
        /// When set, the pawn will not move itself, but another Node3D. Generally, you want this to be an ancestor node of the
        /// pawn. Leave this unset if you want the pawn to move itself.
        /// </summary>
        [Export] public Node3D MovementTargetNode { get; set; }
        /// <summary>
        /// The maximum angle at which surfaces below the pawn are considered to be slopes (instead of steep ground).
        /// Set this value to 0 if you don't want to have any slopes.
        /// </summary>
        [Export] public float MaxSlopeAngle { get; set; } = 30f;
        /// <summary>
        /// The maximum angle at which surfaces below the pawn are considered to be ground. Surfaces with steeper angles are
        /// considered to be walls.
        /// </summary>
        [Export] public float MaxGroundAngle { get; set; } = 60f;
        /// <summary>
        /// The maximum angle at which surfaces above the pawn are considered to be sloped ceilings (instead of steep ceilings).
        /// Set this value to 0 if you don't want to have any sloped ceilings.
        /// </summary>
        [Export] public float MaxCeilingSlopeAngle { get; set; } = 30f;
        /// <summary>
        /// The maximum angle at which surfaces above the pawn are considered to be some form of ceiling. Surfaces with steeper
        /// angles are considered to be walls.
        /// </summary>
        [Export] public float MaxCeilingAngle { get; set; } = 60f;
        /// <summary>
        /// The distance of the checks that determine the properties of the nearest surface.
        /// </summary>
        [Export] public float NearestSurfaceCheckDistance { get; set; } = 1000f;
        /// <summary>
        /// The distance at which the nearest surfaces are considered to be adjacent to the pawn.
        /// </summary>
        [Export] public float AdjacencyCheckDistance { get; set; } = 0.01f;
        /// <summary>
        /// Divides the physics loop into multiple steps. Increases precision, but lowers performance.
        /// </summary>
        [Export] public int SubSteps { get; set; } = 1;

        // Children.
        public PawnChildList<Condition> Conditions { get; private set; }
        public PawnChildList<Raycaster> Raycasters { get; private set; }
        public PawnChildList<Trigger> Triggers { get; private set; }
        public PawnChildList<StateMachine> StateMachines { get; private set; }
        public PawnChildList<ActionProperties> Properties { get; private set; }
        public PawnChildList<Action> Actions { get; private set; }
        public PawnChildList<Modifier> Modifiers { get; private set; }

        public Raycaster ActiveRaycaster { get; private set; }

        // Face direction.
        public bool IsFacingUp { get; set; } = true;
        public bool IsFacingDown
        {
            get => !IsFacingUp;
            set => IsFacingUp = !value;
        }
        public bool IsFacingRight { get; set; } = true;
        public bool IsFacingLeft
        {
            get => !IsFacingRight;
            set => IsFacingRight = !value;
        }

        // Surroundings.
        public NearestSurface ToLeft { get; private set; } = NearestSurface.Nothing;
        public NearestSurface ToRight { get; private set; } = NearestSurface.Nothing;
        public NearestSurface Below { get; private set; } = NearestSurface.Nothing;
        public NearestSurface Above { get; private set; } = NearestSurface.Nothing;
        public NearestSurface Front => IsFacingRight ? ToRight : ToLeft;
        public NearestSurface Behind => IsFacingRight ? ToLeft : ToRight;

        public AdjacentSurface ToLeftAdjacent => ToLeft.IsAdjacent ? ToLeft.Surface : AdjacentSurface.Nothing;
        public AdjacentSurface ToRightAdjacent => ToRight.IsAdjacent ? ToRight.Surface : AdjacentSurface.Nothing;
        public AdjacentSurface BelowAdjacent => Below.IsAdjacent ? Below.Surface : AdjacentSurface.Nothing;
        public AdjacentSurface AboveAdjacent => Above.IsAdjacent ? Above.Surface : AdjacentSurface.Nothing;
        public AdjacentSurface FrontAdjacent => IsFacingRight ? ToRightAdjacent : ToLeftAdjacent;
        public AdjacentSurface BehindAdjacent => IsFacingRight ? ToLeftAdjacent : ToRightAdjacent;

        /* Public methods. */
        /// <summary>
        /// Instantly try to move some distance along the x and y axes, being stopped by physics bodies that happen to be in the
        /// way. Updates surroundings before moving, and climbs and descends slopes.
        /// </summary>
        public void TryMove(float x, float y)
        {
            UpdateSurroundings();
            DoMove(new Vector2(x, y), true, true);
        }

        /* Godot overrides. */
        public override void _EnterTree()
        {
            // Make sure that the number of sub-steps has an allowed value.
            if (SubSteps < 1)
                SubSteps = 1;
            else if (SubSteps > 8)
                SubSteps = 8;
        }

        public override void _Ready()
        {
            // Get all discoverable pawn children.
            Conditions = new(this, true);
            Raycasters = new(this, true);
            Triggers = new(this, true);
            StateMachines = new(this, true);
            Properties = new(this, true);
            Actions = new(this, true);
            Modifiers = new(this, true);

            Conditions.CreateFromNodeTree(this);
            Raycasters.CreateFromNodeTree(this);
            Triggers.CreateFromNodeTree(this);
            StateMachines.CreateFromNodeTree(this);
            Actions.CreateFromNodeTree(this);
            Properties.CreateFromNodeTree(this);
            Modifiers.CreateFromNodeTree(this);

            // Call initialize methods.
            for (int i = 0; i < GetChildCount(); i++)
            {
                Node node = GetChild(i);
                if (node is PawnComponent child)
                {
                    child.Init(this);
                }
            }
        }

        public override void _PhysicsProcess(double deltaTime)
        {
            double subDeltaTime = deltaTime / SubSteps;

            // Update active raycaster.
            ActiveRaycaster = Raycasters.GetFirstActive();

            // Update surroundings (in case objects in the environment moved).
            UpdateSurroundings();

            for (int i = 0; i < SubSteps; i++)
            {
                // Update action properties.
                foreach (Trigger trigger in Triggers)
                {
                    if (trigger.CheckActive(this))
                        trigger.PreUpdateProperties(subDeltaTime, this);
                }
                foreach (StateMachine stateMachine in StateMachines)
                {
                    if (stateMachine.CheckActive(this))
                        stateMachine.PreUpdateProperties(subDeltaTime, this);
                }
                foreach (Action action in Actions)
                {
                    if (action.CheckActive(this))
                    {
                        ActionProperties current = action.GetProperties();
                        action.UpdateProperties(subDeltaTime, this);
                        ActionProperties next = action.GetProperties();
                        if (current != next)
                        {
                            current?.OnDeactivate(subDeltaTime, this);
                            next?.OnActivate(subDeltaTime, this);
                        }
                    }
                    else
                        action.ForceStop();
                }
                foreach (Modifier modifier in Modifiers)
                {
                    if (modifier.CheckActive(this))
                        modifier.PostUpdateProperties(subDeltaTime, this);
                }

                // Update speed.
                foreach (StateMachine stateMachine in StateMachines)
                {
                    if (stateMachine.CheckActive(this))
                        stateMachine.PreUpdateSpeed(subDeltaTime, this);
                }
                foreach (Action action in Actions)
                {
                    if (action.CheckActive(this))
                        action.UpdateSpeed(subDeltaTime, this);
                    else
                        action.ForceStop();
                }
                foreach (Modifier modifier in Modifiers)
                {
                    if (modifier.CheckActive(this))
                        modifier.PostUpdateSpeed(subDeltaTime, this);
                }

                // Update movement.
                foreach (StateMachine stateMachine in StateMachines)
                {
                    if (stateMachine.CheckActive(this))
                        stateMachine.PreUpdateMovement(subDeltaTime, this);
                }
                foreach (Action action in Actions)
                {
                    if (action.CheckActive(this))
                        action.UpdateMovement(subDeltaTime, this);
                    else
                        action.ForceStop();
                }
                foreach (Modifier modifier in Modifiers)
                {
                    if (modifier.CheckActive(this))
                        modifier.PostUpdateMovement(subDeltaTime, this);
                }

                // Update face direction.
                foreach (StateMachine stateMachine in StateMachines)
                {
                    if (stateMachine.CheckActive(this))
                        stateMachine.PreUpdateFaceDirection(subDeltaTime, this);
                }
                foreach (Action action in Actions)
                {
                    if (action.CheckActive(this))
                        action.UpdateFaceDirection(subDeltaTime, this);
                    else
                        action.ForceStop();
                }
                foreach (Modifier modifier in Modifiers)
                {
                    if (modifier.CheckActive(this))
                        modifier.PostUpdateFaceDirection(subDeltaTime, this);
                }

                // Apply each action.
                foreach (Action action in Actions)
                {
                    if (action.CheckActive(this))
                    {
                        DoMove(action.GetMovement(), true, action.DescendsSlopes);
                        ApplyFacing(action.GetFaceDirection());
                    }
                }
            }
        }

        /* Private methods. */
        private void DoMove(Vector2 movement, bool climbSlopes, bool descendSlopes)
        {
            if (movement.X != 0f)
            {
                float distance = movement.X;
                AdjacentSurface front = distance < 0f ? ToLeftAdjacent : ToRightAdjacent;

                // Slope climb.
                if (climbSlopes && front.IsSlopedGround)
                    ClimbSlope(distance, front);

                // Slope descend.
                else if (descendSlopes && (BelowAdjacent.IsSlopedGround || BelowAdjacent.IsSteepGround))
                    DescendSlope(distance, BelowAdjacent);

                // Sloped ceiling descend.
                else if (descendSlopes && front.IsSlopedCeiling)
                    DescendSlopedCeiling(distance, BelowAdjacent);

                // Normal movement.
                else
                    TryMoveX(distance);
            }

            if (movement.Y != 0f)
            {
                float distance = movement.Y;

                // Sloped wall descend.
                if (BelowAdjacent.IsDownwardsSlopedWall && distance < 0f)
                    DescendSlope(distance, BelowAdjacent);

                // Slopes wall upwards.
                else if (AboveAdjacent.IsUpwardsSlopedWall && distance > 0f)
                    ClimbSlopedCeiling(distance, AboveAdjacent);

                // Normal movement.
                else
                    TryMoveY(distance);
            }
        }

        private void ClimbSlope(float distance, AdjacentSurface surface)
        {
            Vector2 redirected = surface.ParallelUp * Mathf.Abs(distance);
            TryMoveY(redirected.Y);
            TryMoveX(redirected.X);

            if (ActiveRaycaster != null)
            {
                ShapecastResult result = ActiveRaycaster.CheckDown(redirected.Y);
                if (result.HasHit)
                    TryMoveY(-result.HitDistance);
            }
        }

        private void DescendSlope(float distance, AdjacentSurface surface)
        {
            Vector2 redirected = surface.ParallelDown * Mathf.Abs(distance);
            TryMoveX(redirected.X);
            TryMoveY(redirected.Y);
        }

        private void ClimbSlopedCeiling(float distance, AdjacentSurface surface)
        {
            Vector2 redirected = surface.ParallelUp * distance;
            TryMoveX(redirected.X);
            TryMoveY(redirected.Y);
        }

        private void DescendSlopedCeiling(float distance, AdjacentSurface surface)
        {
            Vector2 redirected = surface.ParallelDown * distance;
            TryMoveY(redirected.Y);
            TryMoveX(redirected.X);
        }

        private void TryMoveX(float distance)
        {
            if (distance == 0f)
                return;

            if (ActiveRaycaster != null)
            {
                ShapecastResult check = ActiveRaycaster.CheckHorizontal(distance);
                DoMove(check.HitVector);
            }
            else
                DoMove(Vector3.Right * distance);
        }

        private void TryMoveY(float distance)
        {
            if (distance == 0f)
                return;

            if (ActiveRaycaster != null)
            {
                ShapecastResult check = ActiveRaycaster.CheckVertical(distance);
                DoMove(check.HitVector);
            }
            else
                DoMove(Vector3.Up * distance);
        }

        private void DoMove(Vector3 movement)
        {
            if (MovementTargetNode == null)
                Translate(movement);
            else
                MovementTargetNode.Translate(movement);
        }


        private void ApplyFacing(FaceDirection direction)
        {
            if (direction.X == FaceDirectionX.Left)
                IsFacingLeft = true;
            else if (direction.X == FaceDirectionX.Right)
                IsFacingRight = true;

            if (direction.Y == FaceDirectionY.Down)
                IsFacingDown = true;
            else if (direction.Y == FaceDirectionY.Up)
                IsFacingUp = true;
        }


        private void UpdateSurroundings()
        {
            // If there is no currently-active raycaster, all surroundings become empty air.
            if (ActiveRaycaster == null)
            {
                ToLeft = NearestSurface.Nothing;
                ToRight = NearestSurface.Nothing;
                Above = NearestSurface.Nothing;
                Below = NearestSurface.Nothing;
                return;
            }

            // Left adjacent.
            ShapecastResult checkLeft = ActiveRaycaster.CheckLeft(NearestSurfaceCheckDistance);
            ToLeft = GetNearestSurface(checkLeft);

            // Right adjacent.
            ShapecastResult checkRight = ActiveRaycaster.CheckRight(NearestSurfaceCheckDistance);
            ToRight = GetNearestSurface(checkRight);

            // Down adjacent.
            ShapecastResult checkDown = ActiveRaycaster.CheckDown(NearestSurfaceCheckDistance);
            Below = GetNearestSurface(checkDown);

            // Up adjacent.
            ShapecastResult checkUp = ActiveRaycaster.CheckUp(NearestSurfaceCheckDistance);
            Above = GetNearestSurface(checkUp);

            // Hacky fix for sloped surfaces sometimes not being detected.
            if (BelowAdjacent.IsSlanted)
            {
                if (BelowAdjacent.FacesLeft && ToRightAdjacent.IsAir)
                    ToRight = Below;
                else if (BelowAdjacent.FacesRight && ToRightAdjacent.IsAir)
                    ToLeft = Below;
            }

            else if (BelowAdjacent.IsAir)
            {
                if (ToLeftAdjacent.FacesUp)
                    Below = ToLeft;
                else if (ToRightAdjacent.FacesUp)
                    Below = ToRight;
            }

            if (AboveAdjacent.IsSlanted)
            {
                if (AboveAdjacent.FacesLeft && ToRightAdjacent.IsAir)
                    ToRight = Above;
                else if (AboveAdjacent.FacesRight && ToLeftAdjacent.IsAir)
                    ToLeft = Above;
            }

            else if (AboveAdjacent.IsAir)
            {
                if (ToLeftAdjacent.FacesDown)
                    Above = ToLeft;
                else if (ToRightAdjacent.FacesDown)
                    Above = ToRight;
            }

            // Debug prints.
            if (Input.IsKeyPressed(Key.P))
            {
                GD.Print("< " + ToLeft);
                GD.Print("> " + ToRight);
                GD.Print("v " + Below);
                GD.Print("^ " + Above);
            }
        }

        private NearestSurface GetNearestSurface(ShapecastResult castResult)
        {
            // Do initial surface conversion.
            Vector2 up = new Vector2(Vector3.Up.X, Vector3.Up.Y);
            Vector2 normal = new Vector2(castResult.HitNormal.X, castResult.HitNormal.Y);

            AdjacentSurface surface = new AdjacentSurface(castResult.HasHit, normal, up,
                MaxSlopeAngle, MaxGroundAngle, MaxCeilingSlopeAngle, MaxCeilingAngle);

            // Convert to nearest surface.
            return new NearestSurface(surface, castResult.HitDistance, AdjacencyCheckDistance);
        }
    }
}
