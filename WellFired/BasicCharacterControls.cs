namespace WellFired
{
    using System;
    using UnityEngine;

    public class BasicCharacterControls : MonoBehaviour
    {
        public USSequencer cutscene;
        public float strength = 10f;

        private void Update()
        {
            if ((this.cutscene == null) || !this.cutscene.IsPlaying)
            {
                float x = this.strength * Time.deltaTime;
                if (Input.GetKey(KeyCode.W))
                {
                    base.GetComponent<Rigidbody>().AddRelativeForce(-x, 0f, 0f);
                }
                if (Input.GetKey(KeyCode.S))
                {
                    base.GetComponent<Rigidbody>().AddRelativeForce(x, 0f, 0f);
                }
                if (Input.GetKey(KeyCode.A))
                {
                    base.GetComponent<Rigidbody>().AddRelativeForce(0f, 0f, -x);
                }
                if (Input.GetKey(KeyCode.D))
                {
                    base.GetComponent<Rigidbody>().AddRelativeForce(0f, 0f, x);
                }
            }
        }
    }
}

