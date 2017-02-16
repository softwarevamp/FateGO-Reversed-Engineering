using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BetterList<T>
{
    public T[] buffer;
    public int size;

    public void Add(T item)
    {
        if ((this.buffer == null) || (this.size == this.buffer.Length))
        {
            this.AllocateMore();
        }
        this.buffer[this.size++] = item;
    }

    private void AllocateMore()
    {
        T[] array = (this.buffer == null) ? new T[0x20] : new T[Mathf.Max(this.buffer.Length << 1, 0x20)];
        if ((this.buffer != null) && (this.size > 0))
        {
            this.buffer.CopyTo(array, 0);
        }
        this.buffer = array;
    }

    public void Clear()
    {
        this.size = 0;
    }

    public bool Contains(T item)
    {
        if (this.buffer != null)
        {
            for (int i = 0; i < this.size; i++)
            {
                if (this.buffer[i].Equals(item))
                {
                    return true;
                }
            }
        }
        return false;
    }

    [DebuggerHidden, DebuggerHidden, DebuggerStepThrough]
    public IEnumerator<T> GetEnumerator() => 
        new <GetEnumerator>c__Iterator40<T> { <>f__this = (BetterList<T>) this };

    public int IndexOf(T item)
    {
        if (this.buffer != null)
        {
            for (int i = 0; i < this.size; i++)
            {
                if (this.buffer[i].Equals(item))
                {
                    return i;
                }
            }
        }
        return -1;
    }

    public void Insert(int index, T item)
    {
        if ((this.buffer == null) || (this.size == this.buffer.Length))
        {
            this.AllocateMore();
        }
        if ((index > -1) && (index < this.size))
        {
            for (int i = this.size; i > index; i--)
            {
                this.buffer[i] = this.buffer[i - 1];
            }
            this.buffer[index] = item;
            this.size++;
        }
        else
        {
            this.Add(item);
        }
    }

    public T Pop()
    {
        if ((this.buffer != null) && (this.size != 0))
        {
            T local = this.buffer[--this.size];
            this.buffer[this.size] = default(T);
            return local;
        }
        return default(T);
    }

    public void Release()
    {
        this.size = 0;
        this.buffer = null;
    }

    public bool Remove(T item)
    {
        if (this.buffer != null)
        {
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < this.size; i++)
            {
                if (comparer.Equals(this.buffer[i], item))
                {
                    this.size--;
                    this.buffer[i] = default(T);
                    for (int j = i; j < this.size; j++)
                    {
                        this.buffer[j] = this.buffer[j + 1];
                    }
                    this.buffer[this.size] = default(T);
                    return true;
                }
            }
        }
        return false;
    }

    public void RemoveAt(int index)
    {
        if (((this.buffer != null) && (index > -1)) && (index < this.size))
        {
            this.size--;
            this.buffer[index] = default(T);
            for (int i = index; i < this.size; i++)
            {
                this.buffer[i] = this.buffer[i + 1];
            }
            this.buffer[this.size] = default(T);
        }
    }

    [DebuggerStepThrough, DebuggerHidden]
    public void Sort(CompareFunc<T> comparer)
    {
        int num = 0;
        int num2 = this.size - 1;
        bool flag = true;
        while (flag)
        {
            flag = false;
            for (int i = num; i < num2; i++)
            {
                if (comparer(this.buffer[i], this.buffer[i + 1]) > 0)
                {
                    T local = this.buffer[i];
                    this.buffer[i] = this.buffer[i + 1];
                    this.buffer[i + 1] = local;
                    flag = true;
                }
                else if (!flag)
                {
                    num = (i != 0) ? (i - 1) : 0;
                }
            }
        }
    }

    public T[] ToArray()
    {
        this.Trim();
        return this.buffer;
    }

    private void Trim()
    {
        if (this.size > 0)
        {
            if (this.size < this.buffer.Length)
            {
                T[] localArray = new T[this.size];
                for (int i = 0; i < this.size; i++)
                {
                    localArray[i] = this.buffer[i];
                }
                this.buffer = localArray;
            }
        }
        else
        {
            this.buffer = null;
        }
    }

    [DebuggerHidden]
    public T this[int i]
    {
        get => 
            this.buffer[i];
        set
        {
            this.buffer[i] = value;
        }
    }

    [CompilerGenerated]
    private sealed class <GetEnumerator>c__Iterator40 : IEnumerator, IDisposable, IEnumerator<T>
    {
        internal T $current;
        internal int $PC;
        internal BetterList<T> <>f__this;
        internal int <i>__0;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    if (this.<>f__this.buffer == null)
                    {
                        goto Label_0089;
                    }
                    this.<i>__0 = 0;
                    break;

                case 1:
                    this.<i>__0++;
                    break;

                default:
                    goto Label_0090;
            }
            if (this.<i>__0 < this.<>f__this.size)
            {
                this.$current = this.<>f__this.buffer[this.<i>__0];
                this.$PC = 1;
                return true;
            }
        Label_0089:
            this.$PC = -1;
        Label_0090:
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        T IEnumerator<T>.Current =>
            this.$current;

        object IEnumerator.Current =>
            this.$current;
    }

    public delegate int CompareFunc(T left, T right);
}

