using UnityEngine;

namespace COMP476A1.Movement
{
    /// <summary>
    /// Frozen movement Strategy; does not move or rotate
    /// </summary>
    public class Frozen : Strategy
    {
        #region Constructors
        /// <summary>
        /// Creates a new Frozen movement Strategy attached to the given TagController
        /// </summary>
        /// <param name="controller">TagController to attach this Strategy to</param>
        public Frozen(TagController controller) : base(controller) { }
        #endregion

        #region Methods
        /// <summary>
        /// Frozen movement does not move or rotate
        /// </summary>
        /// <returns>A tuple containing Vector2.zero and 0f</returns>
        public override (Vector2 velocity, float rotation) OnFixedUpdate() => (Vector2.zero, 0f);
        #endregion
    }
}
