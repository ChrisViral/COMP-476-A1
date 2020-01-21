using UnityEngine;

namespace A1
{
    /// <summary>
    /// Calculates the Theoretical part of the Assignment
    /// </summary>
    public class Theoretical : MonoBehaviour
    {
        #region Fields
        [SerializeField]
        private int loops = 5;
        [SerializeField]
        private Vector2 originalPos = new Vector2(5f, 6f), originalVelocity = new Vector2(3f, 1f);
        [SerializeField]
        private Vector2 target = new Vector2(8f, 2f);
        [SerializeField]
        private float dt = 0.4f, maxSpeed = 5f, maxAcc = 17f;
        [SerializeField]
        private float satRadius = 1f, kinematicT2T = 0.55f;
        [SerializeField]
        private float arrivalRadius = 0.2f, slowdownRadius = 1.5f, steeringT2T = 0.5f;
        #endregion

        #region Functions
        private void Start()
        {
            //Seek Kinematic
            Debug.Log("Seek (kinematic):");
            Vector2 pos = this.originalPos;
            Vector2 v;
            for (int i = 1; i <= this.loops; i++)
            {
                //Calculate velocity
                v = (this.target - pos).normalized * this.maxSpeed;
                //Set position
                pos += v * this.dt;
                //Dump info
                Debug.Log($"{i}: p:{pos.ToString("0.###")}, v:{v.ToString("0.###")}");
            }

            //Seek Steering
            Debug.Log("Seek (steering)");
            pos = this.originalPos;
            v = this.originalVelocity;
            for (int i = 1; i <= this.loops; i++)
            {
                //Calculate acceleration
                Vector2 a = (this.target - pos).normalized * this.maxAcc;
                //Calculate velocity
                v += a * this.dt;
                if (v.magnitude > this.maxSpeed)
                {
                    v = v.normalized * this.maxSpeed;
                }
                //Set position
                pos += v * this.dt;
                //Dump info
                Debug.Log($"{i}: p:{pos.ToString("0.###")}, v:{v.ToString("0.###")}, a:{a.ToString("0.###")}");
            }

            //Arrive Kinematic
            Debug.Log("Arrive (kinematic)");
            pos = this.originalPos;
            for (int i = 1; i <= this.loops; i++)
            {
                //Get direction and distance to target
                Vector2 dir = this.target - pos;
                float dist = dir.magnitude;
                //Calculate velocity
                v = dir.normalized * (dist > this.satRadius ? this.maxSpeed : Mathf.Min(this.maxSpeed, dist / this.kinematicT2T));
                //Set position
                pos += v * this.dt;
                //Dump info
                Debug.Log($"{i}: p:{pos.ToString("0.###")}, v:{v.ToString("0.###")}, dist:{dist:0.###}");
            }

            //Arrive Steering
            Debug.Log("Arrive (steering)");
            pos = this.originalPos;
            v = this.originalVelocity;
            for (int i = 1; i <= this.loops; i++)
            {
                //Get direction and distance to target
                Vector2 dir = this.target - pos;
                float dist = dir.magnitude;
                Vector2 a;
                if (dist <= this.arrivalRadius)
                {
                    //Arrived, do not move anymore
                    v = a = Vector2.zero;
                }
                else
                {
                    if (dist > this.slowdownRadius)
                    {
                        //Outside of slowdown radius, act like seek and calculate acceleration
                        a = dir.normalized * this.maxAcc;
                    }
                    else
                    {
                        //Within slowdown radius, interpolate goal velocity between the target and slowdown radius
                        Vector2 goal = (dir / this.slowdownRadius) * this.maxSpeed;
                        //Then set acceleration to reach this velocity
                        a = (goal - v) / this.steeringT2T;
                        if (a.magnitude > this.maxAcc)
                        {
                            a = a.normalized * this.maxAcc;
                        }
                    }

                    //Calculate velocity
                    v += a * this.dt;
                    if (v.magnitude >= this.maxSpeed)
                    {
                        v = v.normalized * this.maxSpeed;
                    }

                    //Set position
                    pos += v * this.dt;
                }

                //Dump info
                Debug.Log($"{i}: p:{pos.ToString("0.###")}, v:{v.ToString("0.###")}, a:{a.ToString("0.###")}, dist:{dist:0.###}");
            }
        }
        #endregion
    }
}
