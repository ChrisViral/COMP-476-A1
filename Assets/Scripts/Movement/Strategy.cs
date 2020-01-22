using UnityEngine;

namespace A1.Movement
{
    public abstract class Strategy
    {
        public abstract (Vector2 velocity, float angle) OnFixedUpdate(CharacterController controller);
    }
}