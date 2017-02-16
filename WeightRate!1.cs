using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class WeightRate<T> where T: IComparable<T>, new()
{
    private List<WeightSeed<T>> list;
    private int totalweight;

    public bool checkWeight() => 
        (0 < this.totalweight);

    public int getCount() => 
        this.list?.Count;

    public T getData(int keywieght)
    {
        int totalweight = this.totalweight;
        T local2 = default(T);
        T local = (local2 == null) ? Activator.CreateInstance<T>() : default(T);
        foreach (WeightSeed<T> seed in this.list)
        {
            totalweight -= seed.weight;
            if (totalweight <= keywieght)
            {
                return seed.value;
            }
            local = seed.value;
        }
        return local;
    }

    public int getTotalWeight() => 
        this.totalweight;

    public void removeWeight(T t)
    {
        <removeWeight>c__AnonStorey63<T> storey = new <removeWeight>c__AnonStorey63<T> {
            t = t
        };
        int num = 0;
        this.list.RemoveAll(new Predicate<WeightSeed<T>>(storey.<>m__3E));
        foreach (WeightSeed<T> seed in this.list)
        {
            num += seed.weight;
        }
        this.totalweight = num;
    }

    public void setWeight(int weight, T t)
    {
        if (this.list == null)
        {
            this.list = new List<WeightSeed<T>>();
            this.totalweight = 0;
        }
        this.totalweight += weight;
        this.list.Add(new WeightSeed<T>(weight, t));
    }

    [CompilerGenerated]
    private sealed class <removeWeight>c__AnonStorey63
    {
        internal T t;

        internal bool <>m__3E(WeightRate<T>.WeightSeed s) => 
            (this.t.CompareTo(s.value) == 0);
    }

    private class WeightSeed
    {
        public T value;
        public int weight;

        public WeightSeed(int w, T t)
        {
            this.weight = w;
            this.value = t;
        }
    }
}

