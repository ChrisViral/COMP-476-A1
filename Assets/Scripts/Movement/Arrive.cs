using UnityEngine;

namespace COMP476A1.Movement
{
    /// <summary>
    /// Arrive movement Strategy
    /// </summary>
    public class Arrive : Strategy
    {
        #region Constructor
        /// <summary>
        /// Creates a new Strategy attached to the given TagController
        /// </summary>
        /// <param name="controller">TagController to attach this Strategy to</param>
        public Arrive(TagController controller) : base(controller) { }
        #endregion

        #region Methods
        /// <summary>
        /// Calculates the Strategy's new position and angle each frame
        /// TODO: Implement this properly
        /// </summary>
        /// <returns>A tuple containing the new position and orientation angle of the character</returns>
        public override (Vector2, float) OnFixedUpdate() => (this.Controller.Position, this.Controller.Rotation);
        #endregion
    }
}
