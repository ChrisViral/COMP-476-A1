using System.Linq;
using UnityEngine;

namespace COMP476A1.Movement
{
    /// <summary>
    /// Tag movement Strategy
    /// </summary>
    public class Tag : Strategy
    {
        #region Constants
        /// <summary>
        /// Target change buffer
        /// </summary>
        public const float TARGET_CHANGE_BUFFER = 0.8f;
        #endregion

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
            //Get path to current target
            TagController target = GameLogic.Instance.Target;
            Vector2 toTarget = GridUtils.ProjectPosition(this.Controller.Position, target.Position) - this.Controller.Position;
            float targetDistance = toTarget.magnitude;

            //Check for a potential target switch
            foreach (TagController t in GameLogic.Instance.Targets.Where(t => !t.IsFrozen && t != target))
            {
                //Check distance to potential switch
                Vector2 to = GridUtils.ProjectPosition(this.Controller.Position, t.Position) - this.Controller.Position;
                float dist = to.magnitude;
                //Only switch target if significantly closer to tag
                if (dist < targetDistance * TARGET_CHANGE_BUFFER)
                {
                    target = t;
                    toTarget = to;
                    targetDistance = dist;
                    GameLogic.Instance.SetTarget(target);
                    break;
                }
            }
            //Calculate pursue target with velocities
            float reachTime = targetDistance / this.Controller.MaxSpeed;
            toTarget += toTarget + target.Velocity * reachTime;
            Vector2 velocity = toTarget.normalized * this.Controller.MaxSpeed;
            //Calculate angle to target and necessary rotation
            float deltaAngle = Mathf.DeltaAngle(this.Controller.Rotation, Vector2.SignedAngle(Vector2.up, toTarget));
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
            //If moving and target outside of perception cone, stop
            else if (Mathf.Abs(deltaAngle) > this.Controller.ConeAngle)
            {
                velocity = Vector2.zero;
            }

            //Return velocity and rotation
            return (velocity, rotation);
        }
        #endregion
    }
}
