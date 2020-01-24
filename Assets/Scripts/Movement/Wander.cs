using UnityEngine;

namespace COMP476A1.Movement
{
    /// <summary>
    /// Random Wander movement Strategy
    /// </summary>
    public class Wander : Strategy
    {
        #region Constants
        /// <summary>
        /// Random seed through the Perlin Noise field
        /// </summary>
        private readonly float perlinSeed = Random.Range(0f, 100f);
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new Wander Strategy attached to the given TagController
        /// </summary>
        /// <param name="controller">TagController this Wander Strategy is attached to</param>
        public Wander(TagController controller) : base(controller) { }
        #endregion

        #region Methods
        /// <summary>
        /// Calculates the Strategy's new velocity and rotation angle for this frame
        /// </summary>
        /// <returns></returns>
        public override (Vector2, float) OnFixedUpdate()
        {
            //Change the facing angle with the PerlinNoise
            float angle = this.Controller.Rotation + ((Mathf.PerlinNoise(this.perlinSeed, Time.fixedTime) - 0.5f) * 2f * this.Controller.MaxRotation * Time.fixedDeltaTime);
            //Set velocity direction from the rotation angle
            float velAngle = -angle * Mathf.Deg2Rad;
            Vector2 velocity = new Vector2(Mathf.Sin(velAngle), Mathf.Cos(velAngle)) * (this.Controller.MaxSpeed * Time.fixedDeltaTime);
            //Return new position and angle
            return (this.Controller.Position + velocity, angle);
        }
        #endregion
    }
}