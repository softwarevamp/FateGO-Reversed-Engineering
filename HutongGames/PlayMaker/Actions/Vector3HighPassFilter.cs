﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Use a high pass filter to isolate sudden changes in a Vector3 Variable. Useful when working with Get Device Acceleration to remove the constant effect of gravity."), ActionCategory(ActionCategory.Vector3)]
    public class Vector3HighPassFilter : FsmStateAction
    {
        private Vector3 filteredVector;
        [HutongGames.PlayMaker.Tooltip("Determines how much influence new changes have.")]
        public FsmFloat filteringFactor;
        [HutongGames.PlayMaker.Tooltip("Vector3 Variable to filter. Should generally come from some constantly updated input, e.g., acceleration."), UIHint(UIHint.Variable), RequiredField]
        public FsmVector3 vector3Variable;

        public override void OnEnter()
        {
            this.filteredVector = new Vector3(this.vector3Variable.Value.x, this.vector3Variable.Value.y, this.vector3Variable.Value.z);
        }

        public override void OnUpdate()
        {
            this.filteredVector.x = this.vector3Variable.Value.x - ((this.vector3Variable.Value.x * this.filteringFactor.Value) + (this.filteredVector.x * (1f - this.filteringFactor.Value)));
            this.filteredVector.y = this.vector3Variable.Value.y - ((this.vector3Variable.Value.y * this.filteringFactor.Value) + (this.filteredVector.y * (1f - this.filteringFactor.Value)));
            this.filteredVector.z = this.vector3Variable.Value.z - ((this.vector3Variable.Value.z * this.filteringFactor.Value) + (this.filteredVector.z * (1f - this.filteringFactor.Value)));
            this.vector3Variable.Value = new Vector3(this.filteredVector.x, this.filteredVector.y, this.filteredVector.z);
        }

        public override void Reset()
        {
            this.vector3Variable = null;
            this.filteringFactor = 0.1f;
        }
    }
}

