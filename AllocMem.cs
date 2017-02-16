using System;
using System.Text;
using UnityEngine;

[ExecuteInEditMode]
public class AllocMem : MonoBehaviour
{
    private int allocMem;
    private int allocRate;
    private int collectAlloc;
    private float delta;
    private int lastAllocMemory;
    private float lastAllocSet = -9999f;
    private float lastCollect;
    private float lastCollectNum;
    private float lastDeltaTime;
    private int peakAlloc;
    public bool show = true;
    public bool showFPS;
    public bool showInEditor;

    public void OnGUI()
    {
        if (this.show && (Application.isPlaying || this.showInEditor))
        {
            int num = GC.CollectionCount(0);
            if (this.lastCollectNum != num)
            {
                this.lastCollectNum = num;
                this.delta = Time.realtimeSinceStartup - this.lastCollect;
                this.lastCollect = Time.realtimeSinceStartup;
                this.lastDeltaTime = Time.deltaTime;
                this.collectAlloc = this.allocMem;
            }
            this.allocMem = (int) GC.GetTotalMemory(false);
            this.peakAlloc = (this.allocMem <= this.peakAlloc) ? this.peakAlloc : this.allocMem;
            if ((Time.realtimeSinceStartup - this.lastAllocSet) > 0.3f)
            {
                int num2 = this.allocMem - this.lastAllocMemory;
                this.lastAllocMemory = this.allocMem;
                this.lastAllocSet = Time.realtimeSinceStartup;
                if (num2 >= 0)
                {
                    this.allocRate = num2;
                }
            }
            uint monoUsedSize = Profiler.GetMonoUsedSize();
            uint monoHeapSize = Profiler.GetMonoHeapSize();
            uint totalAllocatedMemory = Profiler.GetTotalAllocatedMemory();
            uint totalReservedMemory = Profiler.GetTotalReservedMemory();
            StringBuilder builder = new StringBuilder();
            builder.Append(string.Concat(new object[] { Profiler.usedHeapSize / 0x100000, " / ", SystemInfo.systemMemorySize, " MB\n" }));
            builder.Append("Currently allocated ");
            builder.Append((((float) this.allocMem) / 1000000f).ToString("0"));
            builder.Append("mb\n");
            builder.Append("Peak allocated ");
            builder.Append((((float) this.peakAlloc) / 1000000f).ToString("0"));
            builder.Append("mb (last collect ");
            builder.Append((((float) this.collectAlloc) / 1000000f).ToString("0"));
            builder.Append(" mb)\n");
            builder.Append("Allocation rate ");
            builder.Append((((float) this.allocRate) / 1000000f).ToString("0.0"));
            builder.Append("mb\n");
            builder.Append("Collection frequency ");
            builder.Append(this.delta.ToString("0.00"));
            builder.Append("s\n");
            builder.Append("Last collect delta ");
            builder.Append(this.lastDeltaTime.ToString("0.000"));
            builder.Append("s (");
            builder.Append((1f / this.lastDeltaTime).ToString("0.0"));
            object[] args = new object[] { monoUsedSize / 0x400, monoHeapSize / 0x400, (100.0 * monoUsedSize) / ((double) monoHeapSize), totalAllocatedMemory / 0x400, totalReservedMemory / 0x400, (100.0 * totalAllocatedMemory) / ((double) totalReservedMemory) };
            builder.AppendFormat("\nmono:{0}/{1} kb({2:f1}%)\ntotal:{3}/{4} kb({5:f1}%)\n", args);
            if (this.showFPS)
            {
                builder.Append(" fps)" + ((1f / Time.deltaTime)).ToString("0.0") + " fps");
            }
            GUI.Box(new Rect(5f, 5f, 310f, (float) (0x7d + (!this.showFPS ? 0 : 0x10))), string.Empty);
            GUI.Label(new Rect(10f, 5f, 1000f, 200f), builder.ToString());
        }
    }

    public void Start()
    {
        base.useGUILayout = false;
    }
}

