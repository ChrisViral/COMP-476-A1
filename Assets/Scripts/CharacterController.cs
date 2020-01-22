using A1.Movement;
using UnityEngine;

namespace A1
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterController : MonoBehaviour
    {
        #region Properties
        /// <summary>
        /// Rigidbody associated to this character
        /// </summary>
        public Rigidbody2D Rigidbody { get; private set; }

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
        private float maxSpeed, maxRotationalSpeed;
        private Strategy strategy;
        #endregion

        #region Functions
        private void Awake()
        {
            this.Rigidbody = GetComponent<Rigidbody2D>();
            this.strategy = new Wander();
        }

        private void FixedUpdate()
        {
            (Vector2 velocity, float angle) = this.strategy.OnFixedUpdate(this);
            this.Rigidbody.position += velocity * Time.fixedDeltaTime;
            this.Rigidbody.rotation = angle;

            if (Mathf.Abs(this.Rigidbody.position.x) > 5f)
            {
                this.Rigidbody.position = new Vector2(this.Rigidbody.position.x - 10 * Mathf.Floor());
            }
        }
        #endregion
    }
}