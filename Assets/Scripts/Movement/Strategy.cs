using UnityEngine;

namespace COMP476A1.Movement
{
    /// <summary>
    /// Movement Strategy
    /// </summary>
    public abstract class Strategy
    {
        #region Properties
        /// <summary>
        /// TagController this strategy operates on
        /// </summary>
        protected TagController Controller { get; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new Strategy attached to the given TagController
        /// </summary>
        /// <param name="controller">TagController to attach this Strategy to</param>
        protected Strategy(TagController controller) => this.Controller = controller;
        #endregion

        #region Abstract methods
        /// <summary>
        /// Calculates the Strategy's new velocity and angle each frame
        /// </summary>
        /// <returns>A tuple containing the new velocity and rotation of the character</returns>
        public abstract (Vector2 velocity, float rotation) OnFixedUpdate();
        #endregion

        #region Static methods
        /// <summary>
        /// Creates the appropriate strategy from the given Enum member
        /// </summary>
        /// <param name="strategy">Strategy to pick</param>
        /// <param name="controller">TagController associated to the new Strategy</param>
        /// <returns>The created strategy</returns>
        public static Strategy CreateStrategy(TagState strategy, TagController controller)
        {
            switch (strategy)
            {
                case TagState.TAG:
                    return new Tag(controller);
                case TagState.TARGET:
                    return new Target(controller);
                case TagState.FROZEN:
                    return new Frozen(controller);
                case TagState.THAW:
                    return new Arrive(controller);

                //Wander
                default:
                    return new Wander(controller);
            }
        }
        #endregion
    }
}