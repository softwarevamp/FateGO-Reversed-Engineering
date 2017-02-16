﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Device), HutongGames.PlayMaker.Tooltip("Plays a full-screen movie on a handheld device. Please consult the Unity docs for Handheld.PlayFullScreenMovie for proper usage.")]
    public class DevicePlayMovieAsset : FsmStateAction
    {
        [RequiredField, HutongGames.PlayMaker.Tooltip("This action will initiate a transition that fades the screen from your current content to the designated background color of the player. When playback finishes, the player uses another fade effect to transition back to your content.")]
        public FsmColor fadeColor;
        protected bool isMovieStart;
        [RequiredField, HutongGames.PlayMaker.Tooltip("Options for displaying movie playback controls. See Unity docs.")]
        public FullScreenMovieControlMode movieControlMode;
        [RequiredField, HutongGames.PlayMaker.Tooltip("Note that player will stream movie directly from the iPhone disc, therefore you have to provide movie as a separate files and not as an usual asset.\nYou will have to create a folder named StreamingAssets inside your Unity project (inside your Assets folder). Store your movies inside that folder. Unity will automatically copy contents of that folder into the iPhone application bundle.")]
        public FsmString moviePath;
        [HutongGames.PlayMaker.Tooltip("Scaling modes for displaying movies.. See Unity docs.")]
        public FullScreenMovieScalingMode movieScalingMode;

        protected void OnEndDownload(AssetData data)
        {
            Handheld.PlayFullScreenMovie("file://" + data.Path, this.fadeColor.Value, this.movieControlMode, this.movieScalingMode);
            this.isMovieStart = true;
        }

        public override void OnEnter()
        {
            if (this.moviePath.Value.StartsWith("http") || this.moviePath.Value.StartsWith("file://"))
            {
                Handheld.PlayFullScreenMovie(this.moviePath.Value, this.fadeColor.Value, this.movieControlMode, this.movieScalingMode);
                this.isMovieStart = true;
            }
            else
            {
                this.isMovieStart = false;
                AssetManager.downloadAssetStorage("Movie/" + this.moviePath.Value, new AssetLoader.LoadEndDataHandler(this.OnEndDownload));
            }
        }

        public override void OnUpdate()
        {
            if (this.isMovieStart)
            {
                this.isMovieStart = false;
                base.Finish();
            }
        }

        public override void Reset()
        {
            this.moviePath = string.Empty;
            this.fadeColor = Color.black;
            this.movieControlMode = FullScreenMovieControlMode.Full;
            this.movieScalingMode = FullScreenMovieScalingMode.AspectFit;
        }
    }
}

