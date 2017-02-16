﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Draws a line from a Start point to an End point. Specify the points as Game Objects or Vector3 world positions. If both are specified, position is used as a local offset from the Object's position."), ActionCategory(ActionCategory.Debug)]
    public class DrawDebugLine : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("The color of the line.")]
        public FsmColor color;
        [HutongGames.PlayMaker.Tooltip("Draw line from a GameObject.")]
        public FsmGameObject fromObject;
        [HutongGames.PlayMaker.Tooltip("Draw line from a world position, or local offset from GameObject if provided.")]
        public FsmVector3 fromPosition;
        [HutongGames.PlayMaker.Tooltip("Draw line to a GameObject.")]
        public FsmGameObject toObject;
        [HutongGames.PlayMaker.Tooltip("Draw line to a world position, or local offset from GameObject if provided.")]
        public FsmVector3 toPosition;

        public override void OnUpdate()
        {
            Vector3 position = ActionHelpers.GetPosition(this.fromObject, this.fromPosition);
            Vector3 vector2 = ActionHelpers.GetPosition(this.toObject, this.toPosition);
            Debug.DrawLine(position, vector2, this.color.Value);
        }

        public override void Reset()
        {
            FsmGameObject obj2 = new FsmGameObject {
                UseVariable = true
            };
            this.fromObject = obj2;
            FsmVector3 vector = new FsmVector3 {
                UseVariable = true
            };
            this.fromPosition = vector;
            obj2 = new FsmGameObject {
                UseVariable = true
            };
            this.toObject = obj2;
            vector = new FsmVector3 {
                UseVariable = true
            };
            this.toPosition = vector;
            this.color = Color.white;
        }
    }
}

