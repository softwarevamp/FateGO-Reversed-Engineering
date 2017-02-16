namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Close the connection to another system.\n\nConnection index defines which system to close the connection to (from the Network connections array).\nCan define connection to close via Guid if index is unknown. \nIf we are a client the only possible connection to close is the server connection, if we are a server the target player will be kicked off. \n\nSend Disconnection Notification enables or disables notifications being sent to the other end. If disabled the connection is dropped, if not a disconnect notification is reliably sent to the remote party and there after the connection is dropped."), ActionCategory(ActionCategory.Network)]
    public class NetworkCloseConnection : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("Connection GUID to close. Used If Index is not set."), UIHint(UIHint.Variable)]
        public FsmString connectionGUID;
        [HutongGames.PlayMaker.Tooltip("Connection index to close"), UIHint(UIHint.Variable)]
        public FsmInt connectionIndex;
        [HutongGames.PlayMaker.Tooltip("If True, send Disconnection Notification")]
        public bool sendDisconnectionNotification;

        private bool getIndexFromGUID(string guid, out int guidIndex)
        {
            for (int i = 0; i < Network.connections.Length; i++)
            {
                if (guid.Equals(Network.connections[i].guid))
                {
                    guidIndex = i;
                    return true;
                }
            }
            guidIndex = 0;
            return false;
        }

        public override void OnEnter()
        {
            int index = 0;
            if (!this.connectionIndex.IsNone)
            {
                index = this.connectionIndex.Value;
            }
            else
            {
                int num2;
                if (!this.connectionGUID.IsNone && this.getIndexFromGUID(this.connectionGUID.Value, out num2))
                {
                    index = num2;
                }
            }
            if ((index < 0) || (index > Network.connections.Length))
            {
                this.LogError("Connection index out of range: " + index);
            }
            else
            {
                Network.CloseConnection(Network.connections[index], this.sendDisconnectionNotification);
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.connectionIndex = 0;
            this.connectionGUID = null;
            this.sendDisconnectionNotification = true;
        }
    }
}

