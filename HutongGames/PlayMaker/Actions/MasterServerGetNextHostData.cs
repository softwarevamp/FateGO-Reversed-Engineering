namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Network), HutongGames.PlayMaker.Tooltip("Get the next host data from the master server. \nEach time this action is called it gets the next connected host.This lets you quickly loop through all the connected hosts to get information on each one.")]
    public class MasterServerGetNextHostData : FsmStateAction
    {
        [UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("A miscellaneous comment (can hold data)")]
        public FsmString comment;
        [UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("Currently connected players")]
        public FsmInt connectedPlayers;
        [HutongGames.PlayMaker.Tooltip("Event to send when there are no more hosts.")]
        public FsmEvent finishedEvent;
        [UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("The name of the game (e.g., 'John Does's Game')")]
        public FsmString gameName;
        [HutongGames.PlayMaker.Tooltip("The type of the game (e.g., 'MyUniqueGameType')"), UIHint(UIHint.Variable)]
        public FsmString gameType;
        [UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("The GUID of the host, needed when connecting with NAT punchthrough.")]
        public FsmString guid;
        [HutongGames.PlayMaker.Tooltip("The index into the MasterServer Host List"), UIHint(UIHint.Variable), ActionSection("Result")]
        public FsmInt index;
        [UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("Server IP address.")]
        public FsmString ipAddress;
        [HutongGames.PlayMaker.Tooltip("Event to send for looping."), ActionSection("Set up")]
        public FsmEvent loopEvent;
        private int nextItemIndex;
        private bool noMoreItems;
        [UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("Does the server require a password?")]
        public FsmBool passwordProtected;
        [UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("Maximum players limit")]
        public FsmInt playerLimit;
        [UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("Server port")]
        public FsmInt port;
        [HutongGames.PlayMaker.Tooltip("Does this server require NAT punchthrough?"), UIHint(UIHint.Variable)]
        public FsmBool useNat;

        private void DoGetNextHostData()
        {
            if (this.nextItemIndex >= MasterServer.PollHostList().Length)
            {
                this.nextItemIndex = 0;
                base.Fsm.Event(this.finishedEvent);
            }
            else
            {
                HostData data = MasterServer.PollHostList()[this.nextItemIndex];
                this.index.Value = this.nextItemIndex;
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
                if (this.nextItemIndex >= MasterServer.PollHostList().Length)
                {
                    base.Fsm.Event(this.finishedEvent);
                    this.nextItemIndex = 0;
                }
                else
                {
                    this.nextItemIndex++;
                    if (this.loopEvent != null)
                    {
                        base.Fsm.Event(this.loopEvent);
                    }
                }
            }
        }

        public override void OnEnter()
        {
            this.DoGetNextHostData();
            base.Finish();
        }

        public override void Reset()
        {
            this.finishedEvent = null;
            this.loopEvent = null;
            this.index = null;
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

