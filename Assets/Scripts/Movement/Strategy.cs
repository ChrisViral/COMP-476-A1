using UnityEngine;

namespace COMP476A1.Movement
{
    /// <summary>
    /// Strategies enum
    /// </summary>
    public enum Strategies
    {
        WANDER,
        ARRIVE
    }

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
        public static Strategy CreateStrategy(Strategies strategy, TagController controller)
        {
            switch (strategy)
            {
                case Strategies.WANDER:
                    return new Wander(controller);
                case Strategies.ARRIVE:
                    return new Tag(controller);
                default:
                    return null;
            }
        }
        #endregion
    }
}