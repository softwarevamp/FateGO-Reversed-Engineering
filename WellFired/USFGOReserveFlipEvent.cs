namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerEventHideDuration, USequencerFriendlyName("FGO Reserve Flip"), USequencerEvent("FGO/Reserve Flip")]
    public class USFGOReserveFlipEvent : USEventBase
    {
        public Vector2 EnemyOffset;
        public Vector2 EnemyTiling;
        public string NodeName;
        private Vector2 OrigOffset;
        private Vector2 OrigTiling;
        public Vector2 PlayerOffset;
        public Vector2 PlayerTiling;

        public override void FireEvent()
        {
            if (base.AffectedObject != null)
            {
                Transform transform = base.AffectedObject.transform.getNodeFromName(this.NodeName, true);
                if (transform != null)
                {
                    Material sharedMaterial;
                    Vector2 playerTiling;
                    Vector2 playerOffset;
                    if (Application.isPlaying)
                    {
                        sharedMaterial = transform.GetComponent<Renderer>().material;
                    }
                    else
                    {
                        sharedMaterial = transform.GetComponent<Renderer>().sharedMaterial;
                    }
                    this.OrigTiling = sharedMaterial.mainTextureScale;
                    this.OrigOffset = sharedMaterial.mainTextureOffset;
                    if (!this.IsEnemy())
                    {
                        playerTiling = this.PlayerTiling;
                        playerOffset = this.PlayerOffset;
                    }
                    else
                    {
                        playerTiling = this.EnemyTiling;
                        playerOffset = this.EnemyOffset;
                    }
                    sharedMaterial.mainTextureScale = playerTiling;
                    sharedMaterial.mainTextureOffset = playerOffset;
                }
            }
        }

        protected bool IsEnemy()
        {
            if (SingletonMonoBehaviour<FGOSequenceManager>.Instance.isEditorMode)
            {
                return false;
            }
            return SingletonMonoBehaviour<BattleSequenceManager>.Instance.actor.GetComponent<BattleActorControl>().IsEnemy;
        }

        public override void ProcessEvent(float deltaTime)
        {
        }

        public override void StopEvent()
        {
            this.UndoEvent();
        }

        public override void UndoEvent()
        {
            if (base.AffectedObject != null)
            {
                Transform transform = base.AffectedObject.transform.getNodeFromName(this.NodeName, false);
                if (transform != null)
                {
                    Material sharedMaterial;
                    if (Application.isPlaying)
                    {
                        sharedMaterial = transform.GetComponent<Renderer>().material;
                    }
                    else
                    {
                        sharedMaterial = transform.GetComponent<Renderer>().sharedMaterial;
                    }
                    sharedMaterial.mainTextureScale = this.OrigTiling;
                    sharedMaterial.mainTextureOffset = this.OrigOffset;
                }
            }
        }

        public void Update()
        {
        }
    }
}

