using System;
using System.IO;
using UnityEngine;

public class ScreenShot : MonoBehaviour
{
    public string folder = "ScreenshotFolder";
    private int frameNumber = 1;
    public int frameRate = 30;
    private string realFolder = string.Empty;
    public float waitTime;

    private void Start()
    {
        Time.captureFramerate = this.frameRate;
        this.realFolder = this.folder;
        Directory.CreateDirectory(this.realFolder);
    }

    private void Update()
    {
        if (Time.time > this.waitTime)
        {
            string filename = $"{this.realFolder}/s{this.frameNumber:D04}.png";
            this.frameNumber++;
            Application.CaptureScreenshot(filename);
        }
    }
}

