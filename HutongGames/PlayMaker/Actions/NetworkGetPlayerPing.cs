namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Get the last average ping time to the given player in milliseconds. \nIf the player can't be found -1 will be returned. Pings are automatically sent out every couple of seconds."), ActionCategory(ActionCategory.Network)]
    public class NetworkGetPlayerPing : FsmStateAction
    {
        private NetworkPlayer _player;
        [ActionSection("Result"), RequiredField, HutongGames.PlayMaker.Tooltip("Get the last average ping time to the given player in milliseconds."), UIHint(UIHint.Variable)]
        public FsmInt averagePing;
        [HutongGames.PlayMaker.Tooltip("The player reference is cached, that is if the connections list changes, the player reference remains.")]
        public bool cachePlayerReference = true;
        public bool everyFrame;
        [HutongGames.PlayMaker.Tooltip("Event to send if the player is found (pings back).")]
        public FsmEvent PlayerFoundEvent;
        [RequiredField, ActionSection("Setup"), HutongGames.PlayMaker.Tooltip("The Index of the player in the network connections list."), UIHint(UIHint.Variable)]
        public FsmInt playerIndex;
        [HutongGames.PlayMaker.Tooltip("Event to send if the player can't be found. Average Ping is set to -1.")]
        public FsmEvent PlayerNotFoundEvent;

        private void GetAveragePing()
        {
            if (!this.cachePlayerReference)
            {
                this._player = Network.connections[this.playerIndex.Value];
            }
            int averagePing = Network.GetAveragePing(this._player);
            if (!this.averagePing.IsNone)
            {
                this.averagePing.Value = averagePing;
            }
            if ((averagePing == -1) && (this.PlayerNotFoundEvent != null))
            {
                base.Fsm.Event(this.PlayerNotFoundEvent);
            }
            if ((averagePing != -1) && (this.PlayerFoundEvent != null))
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
            this.GetAveragePing();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.GetAveragePing();
        }

        public override void Reset()
        {
            this.playerIndex = null;
            this.averagePing = null;
            this.PlayerNotFoundEvent = null;
            this.PlayerFoundEvent = null;
            this.cachePlayerReference = true;
            this.everyFrame = false;
        }
    }
}

