namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Network), HutongGames.PlayMaker.Tooltip("Get host data from the master server.")]
    public class MasterServerGetHostData : FsmStateAction
    {
        [UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("A miscellaneous comment (can hold data)")]
        public FsmString comment;
        [HutongGames.PlayMaker.Tooltip("Currently connected players"), UIHint(UIHint.Variable)]
        public FsmInt connectedPlayers;
        [UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("The name of the game (e.g., 'John Does's Game')")]
        public FsmString gameName;
        [HutongGames.PlayMaker.Tooltip("The type of the game (e.g., 'MyUniqueGameType')"), UIHint(UIHint.Variable)]
        public FsmString gameType;
        [UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("The GUID of the host, needed when connecting with NAT punchthrough.")]
        public FsmString guid;
        [HutongGames.PlayMaker.Tooltip("The index into the MasterServer Host List"), RequiredField]
        public FsmInt hostIndex;
        [HutongGames.PlayMaker.Tooltip("Server IP address."), UIHint(UIHint.Variable)]
        public FsmString ipAddress;
        [HutongGames.PlayMaker.Tooltip("Does the server require a password?"), UIHint(UIHint.Variable)]
        public FsmBool passwordProtected;
        [HutongGames.PlayMaker.Tooltip("Maximum players limit"), UIHint(UIHint.Variable)]
        public FsmInt playerLimit;
        [UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("Server port")]
        public FsmInt port;
        [HutongGames.PlayMaker.Tooltip("Does this server require NAT punchthrough?"), UIHint(UIHint.Variable)]
        public FsmBool useNat;

        private void GetHostData()
        {
            int length = MasterServer.PollHostList().Length;
            int index = this.hostIndex.Value;
            if ((index < 0) || (index >= length))
            {
                this.LogError("MasterServer Host index out of range!");
            }
            else
            {
                HostData data = MasterServer.PollHostList()[index];
                if (data == null)
                {
                    this.LogError("MasterServer HostData could not found at index " + index);
                }
                else
                {
                    this.useNat.Value = data.useNat;
                    this.gameType.Value = data.gameType;
                    this.gameName.Value = data.gameName;
                    this.connectedPlayers.Value = data.connectedPlayers;
                    this.playerLimit.Value = data.playerLimit;
                    this.ipAddress.Value = data.ip[0];
                    this.port.Value = data.port;
                    this.passwordProtected.Value = data.passwordProtected;
                    this.comment.Value = data.comment;
                    this.guid.Value = data.guid;
                }
            }
        }

        public override void OnEnter()
        {
            this.GetHostData();
            base.Finish();
        }

        public override void Reset()
        {
            this.hostIndex = null;
            this.useNat = null;
            this.gameType = null;
            this.gameName = null;
            this.connectedPlayers = null;
            this.playerLimit = null;
            this.ipAddress = null;
            this.port = null;
            this.passwordProtected = null;
            this.comment = null;
            this.guid = null;
        }
    }
}

