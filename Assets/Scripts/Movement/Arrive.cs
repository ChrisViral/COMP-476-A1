using UnityEngine;

namespace COMP476A1.Movement
{
    public class Arrive : Strategy
    {
        #region Constructors
        /// <summary>
        /// Creates a new Arrive movement Strategy attached to the given TagController
        /// </summary>
        /// <param name="controller">TagController to attach this Strategy to</param>
        public Arrive(TagController controller) : base(controller) { }
        #endregion

        #region Methods
        /// <summary>
        /// Calculates the Strategy's new velocity and angle each frame
        /// </summary>
        /// <returns>A tuple containing the new velocity and rotation of the character</returns>
        public override (Vector2 velocity, float rotation) OnFixedUpdate() => throw new System.NotImplementedException();
        #endregion
    }
}
