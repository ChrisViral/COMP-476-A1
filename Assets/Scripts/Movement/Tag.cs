using UnityEngine;

namespace COMP476A1.Movement
{
    /// <summary>
    /// Tag movement Strategy
    /// </summary>
    public class Tag : Strategy
    {
        #region Constructor
        /// <summary>
        /// Creates a new Tag movement Strategy attached to the given TagController
        /// </summary>
        /// <param name="controller">TagController to attach this Strategy to</param>
        public Tag(TagController controller) : base(controller) { }
        #endregion

        #region Methods
        /// <summary>
        /// Calculates the Strategy's new position and angle each frame
        /// TODO: Implement this properly
        /// </summary>
        /// <returns>A tuple containing the new position and orientation angle of the character</returns>
        public override (Vector2, float) OnFixedUpdate()
        {
            //Vector2 targetPath = GridUtils.ProjectPosition(this.Controller.Position, GameLogic.Instance.Target.Position);
            return (this.Controller.Velocity, this.Controller.Rotation);
        }
        #endregion
    }
}
