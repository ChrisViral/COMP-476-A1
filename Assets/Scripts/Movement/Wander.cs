using UnityEngine;

namespace A1.Movement
{
    public class Wander : Strategy
    {
        #region Constants
        private readonly float perlinSeed = Random.Range(0f, 100f);
        #endregion
        
        #region Override methods
        public override (Vector2, float) OnFixedUpdate(CharacterController controller)
        {
            float angle = controller.Rigidbody.rotation + ((Mathf.PerlinNoise(this.perlinSeed, Time.fixedTime) - 0.5f) * 2f * controller.MaxRotation * Time.fixedDeltaTime);
            float velAngle = -angle * Mathf.Deg2Rad;
            Vector2 velocity = new Vector2(Mathf.Sin(velAngle), Mathf.Cos(velAngle)) * controller.MaxSpeed;
            return (velocity, angle);
        }
        #endregion
    }
}