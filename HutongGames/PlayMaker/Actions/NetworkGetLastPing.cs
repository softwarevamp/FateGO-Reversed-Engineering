namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Get the last ping time to the given player in milliseconds. \nIf the player can't be found -1 will be returned. Pings are automatically sent out every couple of seconds."), ActionCategory(ActionCategory.Network)]
    public class NetworkGetLastPing : FsmStateAction
    {
        private NetworkPlayer _player;
        [HutongGames.PlayMaker.Tooltip("The player reference is cached, that is if the connections list changes, the player reference remains.")]
        public bool cachePlayerReference = true;
        public bool everyFrame;
        [ActionSection("Result"), RequiredField, HutongGames.PlayMaker.Tooltip("Get the last ping time to the given player in milliseconds."), UIHint(UIHint.Variable)]
        public FsmInt lastPing;
        [HutongGames.PlayMaker.Tooltip("Event to send if the player is found (pings back).")]
        public FsmEvent PlayerFoundEvent;
        [UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("The Index of the player in the network connections list."), RequiredField, ActionSection("Setup")]
        public FsmInt playerIndex;
        [HutongGames.PlayMaker.Tooltip("Event to send if the player can't be found. Average Ping is set to -1.")]
        public FsmEvent PlayerNotFoundEvent;

        private void GetLastPing()
        {
            if (!this.cachePlayerReference)
            {
                this._player = Network.connections[this.playerIndex.Value];
            }
            int lastPing = Network.GetLastPing(this._player);
            this.lastPing.Value = lastPing;
            if ((lastPing == -1) && (this.PlayerNotFoundEvent != null))
            {
                base.Fsm.Event(this.PlayerNotFoundEvent);
            }
            if ((lastPing != -1) && (this.PlayerFoundEvent != null))
            {
                base.Fsm.Event(this.PlayerFoundEvent);
            }
        }

        public override void OnEnter()
        {
            if (this.cachePlayerReference)
            {
                this._player = Network.connections[this.playerIndex.Value];
            }
            this.GetLastPing();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.GetLastPing();
        }

        public override void Reset()
        {
            this.playerIndex = null;
            this.lastPing = null;
            this.PlayerNotFoundEvent = null;
            this.PlayerFoundEvent = null;
            this.cachePlayerReference = true;
            this.everyFrame = false;
        }
    }
}

