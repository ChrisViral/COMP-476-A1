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
        /// <returns>A tuple containing the new velocity and orientation angle of the character</returns>
        public override (Vector2, float) OnFixedUpdate()
        {
            //Get the change in angle from the
            float rotation = (Mathf.PerlinNoise(this.perlinSeed, Time.fixedTime) - 0.5f) * 2f * this.Controller.MaxRotation;
            //Set velocity direction from the rotation angle
            float velAngle = -(this.Controller.Rotation + (rotation * Time.fixedDeltaTime)) * Mathf.Deg2Rad;
            Vector2 velocity = new Vector2(Mathf.Sin(velAngle), Mathf.Cos(velAngle)) * this.Controller.MaxSpeed;
            //Return new position and angle
            return (velocity, rotation);
        }
        #endregion
    }
}