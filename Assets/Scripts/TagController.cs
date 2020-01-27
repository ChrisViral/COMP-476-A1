using System.Linq;
using COMP476A1.Movement;
using UnityEngine;

namespace COMP476A1
{
    /// <summary>
    /// State of this character
    /// </summary>
    public enum TagState
    {
        WANDER,
        TAG,
        TARGET,
        FROZEN,
        THAW
    }

    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class TagController : MonoBehaviour
    {
        #region Constants
        /// <summary>
        /// Layer name for the tag
        /// </summary>
        private const string tagLayer = "Tag";
        /// <summary>
        /// Layer name for the target
        /// </summary>
        private const string targetLayer = "Target";
        /// <summary>
        /// Layer name for frozen
        /// </summary>
        private const string frozenLayer = "Frozen";
        #endregion

        #region Fields
        [SerializeField]
        private float maxSpeed = 1f, maxRotation = 45f;
        [SerializeField]
        private float timeToTarget = 0.5f, satisfactionRadius = 1f;
        [SerializeField]
        private float slowModifier = 0.3f, minSidestepDistance = 1f;
        [SerializeField]
        private float angleModifier = 30f;
        [SerializeField]
        private TagState state = TagState.WANDER;
        [SerializeField]
        private Material tagMaterial, targetMaterial, frozenMaterial, defaultMaterial;
        private bool setup;
        private Strategy strategy;
        private new Renderer renderer;
        #endregion

        #region Properties
        /// <summary>
        /// Rigidbody associated to this character
        /// </summary>
        public Rigidbody2D Rigidbody { get; private set; }

        /// <summary>
        /// Current velocity of the character
        /// </summary>
        public Vector2 Velocity { get; private set; }

        /// <summary>
        /// Position of this Character on the 2D plane
        /// </summary>
        public Vector2 Position
        {
            get => this.Rigidbody.position;
            set => this.Rigidbody.position = value;
        }

        /// <summary>
        /// Rotation Angle of this character
        /// </summary>
        public float Rotation
        {
            get => this.Rigidbody.rotation;
            set => this.Rigidbody.rotation = value;
        }

        /// <summary>
        /// Maximum speed of this character
        /// </summary>
        public float MaxSpeed => this.maxSpeed * (this.IsTag ? 1.25f : 1f);

        /// <summary>
        /// Maximum rotational speed of this character, in degrees/s
        /// </summary>
        public float MaxRotation => this.maxRotation;

        /// <summary>
        /// Arrive TimeToTarget value
        /// </summary>
        public float TimeToTarget => this.timeToTarget;

        /// <summary>
        /// Arrive satisfaction radius
        /// </summary>
        public float SatisfactionRadius => this.satisfactionRadius;

        /// <summary>
        /// If this character is considered stationary or not
        /// </summary>
        public bool IsStationary => this.Velocity.magnitude < this.MaxSpeed * this.slowModifier;

        /// <summary>
        /// Minimum distance at which a character can attempt to sidestep to it's target
        /// </summary>
        public float MinSidestepDistance => this.minSidestepDistance;

        /// <summary>
        /// The speed dependent perception cone angle, clamped between 10 and 90
        /// </summary>
        public float ConeAngle => Mathf.Clamp(this.angleModifier / this.Velocity.magnitude, 10f, 90f);

        /// <summary>
        /// Angle at which the character can start moving towards the target at max speed
        /// </summary>
        public float DepartAngle => Mathf.Clamp(this.angleModifier / this.MaxSpeed, 10f, 90f);

        /// <summary>
        /// Current tag state of the character
        /// </summary>
        public TagState State
        {
            get => this.state;
            set
            {
                //Set value
                this.state = value;
                //Set appropriate strategy
                switch (value)
                {
                    case TagState.TAG:
                        this.strategy = new Tag(this);
                        this.renderer.material = this.tagMaterial;
                        this.gameObject.layer = LayerMask.NameToLayer(tagLayer);
                        break;

                    case TagState.TARGET:
                        this.strategy = new Target(this);
                        this.renderer.material = this.targetMaterial;
                        this.gameObject.layer = LayerMask.NameToLayer(targetLayer);
                        if (this.Assigned)
                        {
                            this.Assigned.Assigned = null;
                            this.Assigned = null;
                        }
                        break;

                    case TagState.FROZEN:
                        this.strategy = new Frozen(this);
                        this.renderer.material = this.frozenMaterial;
                        this.gameObject.layer = LayerMask.NameToLayer(frozenLayer);
                        break;

                    case TagState.THAW:
                        this.strategy = new Arrive(this);
                        this.renderer.material = this.defaultMaterial;
                        this.gameObject.layer = 0;
                        break;

                    //Wander
                    default:
                        this.strategy = new Wander(this);
                        this.renderer.material = this.defaultMaterial;
                        this.gameObject.layer = 0;
                        break;
                }
            }
        }

        /// <summary>
        /// If this character is the Tag
        /// </summary>
        public bool IsTag => this.State == TagState.TAG;

        /// <summary>
        /// If this character is the current Target
        /// </summary>
        public bool IsTarget => this.State == TagState.TARGET;

        /// <summary>
        /// If this character has been frozen by the tag
        /// </summary>
        public bool IsFrozen => this.State == TagState.FROZEN;

        /// <summary>
        /// TagController this controller is assigned to
        /// </summary>
        public TagController Assigned { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Sets up all components to properly use this TagController
        /// Automatically called by Awake(), but can be called early if needed
        /// </summary>
        public void SetupComponents()
        {
            if (!this.setup)
            {
                this.Rigidbody = GetComponent<Rigidbody2D>();
                this.renderer = GetComponentInChildren<Renderer>();
                this.strategy = Strategy.CreateStrategy(this.State, this);
                this.setup = true;
            }
        }
        #endregion

        #region Functions
        private void Awake() => SetupComponents();

        private void Update()
        {
            //Check if anyone is assigned to thaw this character
            if (this.State == TagState.FROZEN && !this.Assigned)
            {
                //If not find the closest available character
                TagController request = null;
                float distance = GridUtils.GRID_SIZE * 2;
                foreach (TagController player in GameLogic.Instance.Targets.Where(t => t.State == TagState.WANDER))
                {
                    float d = GridUtils.GetShortestDirection(this.Position, player.Position).magnitude;
                    if (d < distance)
                    {
                        request = player;
                        distance = d;
                    }
                }

                //Assign if one exists
                if (request != null)
                {
                    this.Assigned = request;
                    this.Assigned.State = TagState.THAW;
                    this.Assigned.Assigned = this;
                }
            }
        }

        private void FixedUpdate()
        {
            //Let the strategy decide how the character moves
            (Vector2 velocity, float rotation) = this.strategy.OnFixedUpdate();
            this.Velocity = Vector2.ClampMagnitude(velocity, this.MaxSpeed);
            this.Position = GridUtils.WrapPosition(this.Position + this.Velocity * Time.fixedDeltaTime);
            this.Rotation += Mathf.Clamp(rotation, -this.MaxRotation, this.MaxRotation) * Time.fixedDeltaTime;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            //Get the TagController of the collider
            TagController controller = other.GetComponent<TagController>();
            if (controller)
            {
                switch (this.State)
                {
                    case TagState.TAG:
                    {
                        //If tag and collide with target, freeze it
                        if (controller.IsTarget)
                        {
                            controller.State = TagState.FROZEN;
                        }
                        return;
                    }

                    case TagState.FROZEN:
                    {
                        //If frozen and none-tag collides, thaw
                        if (!controller.IsTag)
                        {
                            this.State = TagState.WANDER;
                            if (this.Assigned)
                            {
                                this.Assigned.Assigned = null;
                                this.Assigned.State = TagState.WANDER;
                                this.Assigned = null;
                            }
                        }
                        return;
                    }

                    //All other states take no action
                    default:
                        return;
                }
            }
        }
        #endregion
    }
}