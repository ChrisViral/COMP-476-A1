using COMP476A1.Movement;
using UnityEngine;

namespace COMP476A1
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class TagController : MonoBehaviour
    {
        #region Properties
        /// <summary>
        /// Rigidbody associated to this character
        /// </summary>
        public Rigidbody2D Rigidbody { get; private set; }

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
        public float MaxSpeed => this.maxSpeed;

        /// <summary>
        /// Maximum rotational speed of this character, in degrees/s
        /// </summary>
        public float MaxRotation => this.maxRotationalSpeed;
        #endregion

        #region Fields
        [SerializeField]
        private float maxSpeed = 1f, maxRotationalSpeed = 45f;
        [SerializeField]
        private Strategies initialStrategy = Strategies.WANDER;
        private Strategy strategy;
        #endregion

        #region Functions
        private void Awake()
        {
            //Gets the Rigidbody and sets the initial strategy
            this.Rigidbody = GetComponent<Rigidbody2D>();
            this.strategy = Strategy.CreateStrategy(this.initialStrategy, this);
        }

        private void FixedUpdate()
        {
            //Let the strategy decide how the character moves
            (Vector2 position, float angle) = this.strategy.OnFixedUpdate();
            this.Position = GridUtils.WrapPosition(position);
            this.Rotation = angle;
        }
        #endregion
    }
}