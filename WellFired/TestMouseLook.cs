namespace WellFired
{
    using System;
    using UnityEngine;

    public class TestMouseLook : MonoBehaviour
    {
        public float lookSmooth = 0.1f;
        public float lookSpeed = 40f;
        private float lookX;
        public float lookXMax = 20f;
        private float lookY;
        public float lookYMax = 20f;

        private void Update()
        {
            this.lookX += (Input.GetAxis("Mouse X") * this.lookSpeed) * Time.deltaTime;
            this.lookY += (Input.GetAxis("Mouse Y") * this.lookSpeed) * Time.deltaTime;
            this.lookX = Mathf.Clamp(this.lookX, -this.lookXMax, this.lookXMax);
            this.lookY = Mathf.Clamp(this.lookY, -this.lookYMax, this.lookYMax);
            Quaternion b = Quaternion.Euler(-this.lookY, this.lookX, 0f);
            base.transform.localRotation = Quaternion.Lerp(base.transform.localRotation, b, this.lookSmooth);
        }
    }
}

