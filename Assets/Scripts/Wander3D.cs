using UnityEngine;

namespace A1
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Wander3D : MonoBehaviour
    {
        
        private Rigidbody2D rigidbody;
        [SerializeField]
        private float speed;
        [SerializeField]
        private Transform target;
        

        private void Awake()
        {
            Debug.Log(this.transform.forward.ToString("0.###"));
            this.rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            Debug.Log(this.transform.forward.ToString("0.###"));
            this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.FromToRotation(Vector3.forward, this.target.position - this.transform.position), this.speed * Time.fixedDeltaTime);
        }
    }
}