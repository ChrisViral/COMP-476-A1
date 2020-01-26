using COMP476A1.Movement;
using UnityEngine;

namespace COMP476A1
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class TagController : MonoBehaviour
    {
        #region Fields
        [SerializeField]
        private float maxSpeed = 1f, maxRotation = 45f;
        [SerializeField]
        private float timeToTarget = 0.5f;
        [SerializeField]
        private float slowModifier = 0.3f, minSidestepDistance = 1f;
        [SerializeField]
        private float angleModifier = 30f;
        [SerializeField]
        private Strategies initialStrategy = Strategies.WANDER;
        [SerializeField]
        private bool isTag, isTarget, isFrozen;
        [SerializeField]
        private Material tagMaterial, frozenMaterial;
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
        public float MaxSpeed => this.maxSpeed * (this.isTag ? 1.25f : 1f);

        /// <summary>
        /// Maximum rotational speed of this character, in degrees/s
        /// </summary>
        public float MaxRotation => this.maxRotation;

        /// <summary>
        /// Arrive TimeToTarget value
        /// </summary>
        public float TimeToTarget => this.timeToTarget;

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
        /// If this character is the Tag
        /// </summary>
        public bool IsTag
        {
            get => this.isTag;
            set
            {
                this.isTag = value;
                if (value)
                {
                    this.strategy = new Tag(this);
                    if (this.renderer && this.tagMaterial)
                    {
                        this.renderer.material = this.tagMaterial;
                    }
                }
            }
        }

        /// <summary>
        /// If this character is the current Target
        /// </summary>
        public bool IsTarget
        {
            get => this.isTarget;
            set
            {
                this.isTarget = value;
                this.strategy = value ? new Target(this) : new Wander(this) as Strategy;
            }
        }

        /// <summary>
        /// If this character has been frozen by the tag
        /// </summary>
        public bool IsFrozen { get; private set; }
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
                this.strategy = Strategy.CreateStrategy(this.initialStrategy, this);
                this.setup = true;
            }
        }
        #endregion

        #region Functions
        private void Awake() => SetupComponents();

        private void FixedUpdate()
        {
            //Let the strategy decide how the character moves
            (Vector2 velocity, float rotation) = this.strategy.OnFixedUpdate();
            this.Velocity = Vector2.ClampMagnitude(velocity, this.MaxSpeed);
            this.Position = GridUtils.WrapPosition(this.Position + this.Velocity * Time.fixedDeltaTime);
            this.Rotation += Mathf.Clamp(rotation, -this.MaxRotation, this.MaxRotation) * Time.fixedDeltaTime;
        }
        #endregion
    }
}