using COMP476A1.Movement;
using UnityEngine;

namespace COMP476A1
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class TagController : MonoBehaviour
    {
        #region Fields
        [SerializeField]
        private float maxSpeed = 1f, maxRotation = 60f;
        [SerializeField]
        private Strategies initialStrategy = Strategies.WANDER;
        [SerializeField]
        private bool isTag, isTarget;
        [SerializeField]
        private Material tagMaterial, frozenMaterial;
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
        /// If this character is the Tag
        /// </summary>
        public bool IsTag
        {
            get => this.isTag;
            set
            {
                this.isTag = value;
                this.strategy = new Tag(this);
                if (this.renderer && this.tagMaterial)
                {
                    this.renderer.material = this.tagMaterial;
                }
            }
        }

        /// <summary>
        /// If this character is the current Target
        /// </summary>
        public bool IsTarget
        {
            get => this.isTarget;
            set => this.isTarget = value;
        }

        /// <summary>
        /// If this character has been frozen by the tag
        /// </summary>
        public bool IsFrozen { get; private set; }
        #endregion

        #region Functions
        private void Awake()
        {
            //Gets the Rigidbody and sets the initial strategy
            this.Rigidbody = GetComponent<Rigidbody2D>();
            this.renderer = GetComponentInChildren<Renderer>();
            if (this.renderer && this.tagMaterial && this.IsTag)
            {
                this.renderer.material = this.tagMaterial;
            }
            if (this.strategy == null)
            {
                this.strategy = Strategy.CreateStrategy(this.initialStrategy, this);
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
        #endregion
    }
}