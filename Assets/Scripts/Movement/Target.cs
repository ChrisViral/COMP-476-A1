using UnityEngine;

namespace COMP476A1.Movement
{
    public class Target : Strategy
    {
        #region Constructors
        /// <summary>
        /// Creates a new Target movement Strategy attached to the given TagController
        /// </summary>
        /// <param name="controller">TagController to attach this Strategy to</param>
        public Target(TagController controller) : base(controller) { }
        #endregion

        #region Methods
        /// <summary>
        /// Calculates the Target movement Strategy's new velocity and angle each frame
        /// </summary>
        /// <returns>A tuple containing the new velocity and rotation of the character</returns>
        public override (Vector2 velocity, float rotation) OnFixedUpdate()
        {
            //Get tag information
            TagController tag = GameLogic.Instance.Tag;
            Vector2 toTag = GridUtils.GetShortestDirection(this.Controller.Position, tag.Position);
            float targetDistance = toTag.magnitude;

            //Calculate velocity away from tag
            Vector2 velocity = toTag.normalized * -this.Controller.MaxSpeed;
            //Calculate angle away from tag and necessary rotation
            float deltaAngle = Mathf.DeltaAngle(this.Controller.Rotation, Vector2.SignedAngle(Vector2.up, -toTag));
            float rotation = Mathf.Abs(deltaAngle) <= 0.05f * this.Controller.MaxRotation ? deltaAngle : this.Controller.MaxRotation * Mathf.Sign(deltaAngle);

            //Case when not moving
            if (this.Controller.IsStationary)
            {
                //If within sidestep range, do not rotate, simply step that way
                if (targetDistance <= this.Controller.MinSidestepDistance)
                {
                    rotation = 0f;
                }
                //Else if the target is not in front of us, keep turning without moving
                else if (Mathf.Abs(deltaAngle) > this.Controller.DepartAngle)
                {
                    velocity = Vector2.zero;
                }
            }
            //When moving, make sure we're facing away
            else if (Mathf.Abs(deltaAngle) > this.Controller.DepartAngle)
            {
                velocity = Vector2.zero;
            }

            //Return velocity and rotation
            return (velocity, rotation);
        }
        #endregion
    }
}
