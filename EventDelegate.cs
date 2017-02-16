using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class EventDelegate
{
    [NonSerialized]
    private object[] mArgs;
    [NonSerialized]
    private bool mCached;
    [NonSerialized]
    private Callback mCachedCallback;
    [NonSerialized]
    private MethodInfo mMethod;
    [SerializeField]
    private string mMethodName;
    [NonSerialized]
    private ParameterInfo[] mParameterInfos;
    [SerializeField]
    private Parameter[] mParameters;
    [NonSerialized]
    private bool mRawDelegate;
    [SerializeField]
    private MonoBehaviour mTarget;
    public bool oneShot;
    private static int s_Hash = "EventDelegate".GetHashCode();

    public EventDelegate()
    {
    }

    public EventDelegate(Callback call)
    {
        this.Set(call);
    }

    public EventDelegate(MonoBehaviour target, string methodName)
    {
        this.Set(target, methodName);
    }

    public static void Add(List<EventDelegate> list, EventDelegate ev)
    {
        Add(list, ev, ev.oneShot);
    }

    public static EventDelegate Add(List<EventDelegate> list, Callback callback) => 
        Add(list, callback, false);

    public static void Add(List<EventDelegate> list, EventDelegate ev, bool oneShot)
    {
        if ((ev.mRawDelegate || (ev.target == null)) || string.IsNullOrEmpty(ev.methodName))
        {
            Add(list, ev.mCachedCallback, oneShot);
        }
        else if (list != null)
        {
            int num = 0;
            int count = list.Count;
            while (num < count)
            {
                EventDelegate delegate2 = list[num];
                if ((delegate2 != null) && delegate2.Equals(ev))
                {
                    return;
                }
                num++;
            }
            EventDelegate item = new EventDelegate(ev.target, ev.methodName) {
                oneShot = oneShot
            };
            if ((ev.mParameters != null) && (ev.mParameters.Length > 0))
            {
                item.mParameters = new Parameter[ev.mParameters.Length];
                for (int i = 0; i < ev.mParameters.Length; i++)
                {
                    item.mParameters[i] = ev.mParameters[i];
                }
            }
            list.Add(item);
        }
        else
        {
            Debug.LogWarning("Attempting to add a callback to a list that's null");
        }
    }

    public static EventDelegate Add(List<EventDelegate> list, Callback callback, bool oneShot)
    {
        if (list != null)
        {
            int num = 0;
            int count = list.Count;
            while (num < count)
            {
                EventDelegate delegate2 = list[num];
                if ((delegate2 != null) && delegate2.Equals(callback))
                {
                    return delegate2;
                }
                num++;
            }
            EventDelegate item = new EventDelegate(callback) {
                oneShot = oneShot
            };
            list.Add(item);
            return item;
        }
        Debug.LogWarning("Attempting to add a callback to a list that's null");
        return null;
    }

    private void Cache()
    {
        this.mCached = true;
        if (!this.mRawDelegate && ((((this.mCachedCallback == null) || ((this.mCachedCallback.Target as MonoBehaviour) != this.mTarget)) || (GetMethodName(this.mCachedCallback) != this.mMethodName)) && ((this.mTarget != null) && !string.IsNullOrEmpty(this.mMethodName))))
        {
            System.Type baseType = this.mTarget.GetType();
            this.mMethod = null;
            while (baseType != null)
            {
                try
                {
                    this.mMethod = baseType.GetMethod(this.mMethodName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                    if (this.mMethod != null)
                    {
                        break;
                    }
                }
                catch (Exception)
                {
                }
                baseType = baseType.BaseType;
            }
            if (this.mMethod == null)
            {
                Debug.LogError(string.Concat(new object[] { "Could not find method '", this.mMethodName, "' on ", this.mTarget.GetType() }), this.mTarget);
            }
            else if (this.mMethod.ReturnType != typeof(void))
            {
                Debug.LogError(string.Concat(new object[] { this.mTarget.GetType(), ".", this.mMethodName, " must have a 'void' return type." }), this.mTarget);
            }
            else
            {
                this.mParameterInfos = this.mMethod.GetParameters();
                if (this.mParameterInfos.Length == 0)
                {
                    this.mCachedCallback = (Callback) Delegate.CreateDelegate(typeof(Callback), this.mTarget, this.mMethodName);
                    this.mArgs = null;
                    this.mParameters = null;
                }
                else
                {
                    this.mCachedCallback = null;
                    if ((this.mParameters == null) || (this.mParameters.Length != this.mParameterInfos.Length))
                    {
                        this.mParameters = new Parameter[this.mParameterInfos.Length];
                        int num = 0;
                        int num2 = this.mParameters.Length;
                        while (num < num2)
                        {
                            this.mParameters[num] = new Parameter();
                            num++;
                        }
                    }
                    int index = 0;
                    int length = this.mParameters.Length;
                    while (index < length)
                    {
                        this.mParameters[index].expectedType = this.mParameterInfos[index].ParameterType;
                        index++;
                    }
                }
            }
        }
    }

    public void Clear()
    {
        this.mTarget = null;
        this.mMethodName = null;
        this.mRawDelegate = false;
        this.mCachedCallback = null;
        this.mParameters = null;
        this.mCached = false;
        this.mMethod = null;
        this.mParameterInfos = null;
        this.mArgs = null;
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
        {
            return !this.isValid;
        }
        if (obj is Callback)
        {
            Callback callback = obj as Callback;
            if (callback.Equals(this.mCachedCallback))
            {
                return true;
            }
            MonoBehaviour target = callback.Target as MonoBehaviour;
            return ((this.mTarget == target) && string.Equals(this.mMethodName, GetMethodName(callback)));
        }
        if (obj is EventDelegate)
        {
            EventDelegate delegate2 = obj as EventDelegate;
            return ((this.mTarget == delegate2.mTarget) && string.Equals(this.mMethodName, delegate2.mMethodName));
        }
        return false;
    }

    public bool Execute()
    {
        if (!this.mCached)
        {
            this.Cache();
        }
        if (this.mCachedCallback != null)
        {
            this.mCachedCallback();
            return true;
        }
        if (this.mMethod == null)
        {
            return false;
        }
        if (((this.mParameters == null) ? 0 : this.mParameters.Length) == 0)
        {
            this.mMethod.Invoke(this.mTarget, null);
        }
        else
        {
            if ((this.mArgs == null) || (this.mArgs.Length != this.mParameters.Length))
            {
                this.mArgs = new object[this.mParameters.Length];
            }
            int index = 0;
            int length = this.mParameters.Length;
            while (index < length)
            {
                this.mArgs[index] = this.mParameters[index].value;
                index++;
            }
            try
            {
                this.mMethod.Invoke(this.mTarget, this.mArgs);
            }
            catch (ArgumentException exception)
            {
                string str = "Error calling ";
                if (this.mTarget == null)
                {
                    str = str + this.mMethod.Name;
                }
                else
                {
                    string str2 = str;
                    object[] objArray1 = new object[] { str2, this.mTarget.GetType(), ".", this.mMethod.Name };
                    str = string.Concat(objArray1);
                }
                str = (str + ": " + exception.Message) + "\n  Expected: ";
                if (this.mParameterInfos.Length == 0)
                {
                    str = str + "no arguments";
                }
                else
                {
                    str = str + this.mParameterInfos[0];
                    for (int i = 1; i < this.mParameterInfos.Length; i++)
                    {
                        str = str + ", " + this.mParameterInfos[i].ParameterType;
                    }
                }
                str = str + "\n  Received: ";
                if (this.mParameters.Length == 0)
                {
                    str = str + "no arguments";
                }
                else
                {
                    str = str + this.mParameters[0].type;
                    for (int j = 1; j < this.mParameters.Length; j++)
                    {
                        str = str + ", " + this.mParameters[j].type;
                    }
                }
                Debug.LogError(str + "\n");
            }
            int num6 = 0;
            int num7 = this.mArgs.Length;
            while (num6 < num7)
            {
                if (this.mParameterInfos[num6].IsIn || this.mParameterInfos[num6].IsOut)
                {
                    this.mParameters[num6].value = this.mArgs[num6];
                }
                this.mArgs[num6] = null;
                num6++;
            }
        }
        return true;
    }

    public static void Execute(List<EventDelegate> list)
    {
        if (list != null)
        {
            int index = 0;
            while (index < list.Count)
            {
                EventDelegate delegate2 = list[index];
                if (delegate2 != null)
                {
                    try
                    {
                        delegate2.Execute();
                    }
                    catch (Exception exception)
                    {
                        if (exception.InnerException != null)
                        {
                            Debug.LogError(exception.InnerException.Message);
                        }
                        else
                        {
                            Debug.LogError(exception.Message);
                        }
                    }
                    if (index >= list.Count)
                    {
                        break;
                    }
                    if (list[index] != delegate2)
                    {
                        continue;
                    }
                    if (delegate2.oneShot)
                    {
                        list.RemoveAt(index);
                        continue;
                    }
                }
                index++;
            }
        }
    }

    public override int GetHashCode() => 
        s_Hash;

    private static string GetMethodName(Callback callback) => 
        callback.Method.Name;

    private static bool IsValid(Callback callback) => 
        ((callback != null) && (callback.Method != null));

    public static bool IsValid(List<EventDelegate> list)
    {
        if (list != null)
        {
            int num = 0;
            int count = list.Count;
            while (num < count)
            {
                EventDelegate delegate2 = list[num];
                if ((delegate2 != null) && delegate2.isValid)
                {
                    return true;
                }
                num++;
            }
        }
        return false;
    }

    public static bool Remove(List<EventDelegate> list, EventDelegate ev)
    {
        if (list != null)
        {
            int index = 0;
            int count = list.Count;
            while (index < count)
            {
                EventDelegate delegate2 = list[index];
                if ((delegate2 != null) && delegate2.Equals(ev))
                {
                    list.RemoveAt(index);
                    return true;
                }
                index++;
            }
        }
        return false;
    }

    public static bool Remove(List<EventDelegate> list, Callback callback)
    {
        if (list != null)
        {
            int index = 0;
            int count = list.Count;
            while (index < count)
            {
                EventDelegate delegate2 = list[index];
                if ((delegate2 != null) && delegate2.Equals(callback))
                {
                    list.RemoveAt(index);
                    return true;
                }
                index++;
            }
        }
        return false;
    }

    private void Set(Callback call)
    {
        this.Clear();
        if ((call != null) && IsValid(call))
        {
            this.mTarget = call.Target as MonoBehaviour;
            if (this.mTarget == null)
            {
                this.mRawDelegate = true;
                this.mCachedCallback = call;
                this.mMethodName = null;
            }
            else
            {
                this.mMethodName = GetMethodName(call);
                this.mRawDelegate = false;
            }
        }
    }

    public static void Set(List<EventDelegate> list, EventDelegate del)
    {
        if (list != null)
        {
            list.Clear();
            list.Add(del);
        }
    }

    public static EventDelegate Set(List<EventDelegate> list, Callback callback)
    {
        if (list != null)
        {
            EventDelegate item = new EventDelegate(callback);
            list.Clear();
            list.Add(item);
            return item;
        }
        return null;
    }

    public void Set(MonoBehaviour target, string methodName)
    {
        this.Clear();
        this.mTarget = target;
        this.mMethodName = methodName;
    }

    public override string ToString()
    {
        if (this.mTarget == null)
        {
            return (!this.mRawDelegate ? null : "[delegate]");
        }
        string str = this.mTarget.GetType().ToString();
        int num = str.LastIndexOf('.');
        if (num > 0)
        {
            str = str.Substring(num + 1);
        }
        if (!string.IsNullOrEmpty(this.methodName))
        {
            return (str + "/" + this.methodName);
        }
        return (str + "/[delegate]");
    }

    public bool isEnabled
    {
        get
        {
            if (!this.mCached)
            {
                this.Cache();
            }
            if (this.mRawDelegate && (this.mCachedCallback != null))
            {
                return true;
            }
            if (this.mTarget == null)
            {
                return false;
            }
            MonoBehaviour mTarget = this.mTarget;
            return ((mTarget == null) || mTarget.enabled);
        }
    }

    public bool isValid
    {
        get
        {
            if (!this.mCached)
            {
                this.Cache();
            }
            return ((this.mRawDelegate && (this.mCachedCallback != null)) || ((this.mTarget != null) && !string.IsNullOrEmpty(this.mMethodName)));
        }
    }

    public string methodName
    {
        get => 
            this.mMethodName;
        set
        {
            this.mMethodName = value;
            this.mCachedCallback = null;
            this.mRawDelegate = false;
            this.mCached = false;
            this.mMethod = null;
            this.mParameterInfos = null;
            this.mParameters = null;
        }
    }

    public Parameter[] parameters
    {
        get
        {
            if (!this.mCached)
            {
                this.Cache();
            }
            return this.mParameters;
        }
    }

    public MonoBehaviour target
    {
        get => 
            this.mTarget;
        set
        {
            this.mTarget = value;
            this.mCachedCallback = null;
            this.mRawDelegate = false;
            this.mCached = false;
            this.mMethod = null;
            this.mParameterInfos = null;
            this.mParameters = null;
        }
    }

    public delegate void Callback();

    [Serializable]
    public class Parameter
    {
        [NonSerialized]
        public bool cached;
        [NonSerialized]
        public System.Type expectedType;
        public string field;
        [NonSerialized]
        public FieldInfo fieldInfo;
        [NonSerialized]
        private object mValue;
        public UnityEngine.Object obj;
        [NonSerialized]
        public PropertyInfo propInfo;

        public Parameter()
        {
            this.expectedType = typeof(void);
        }

        public Parameter(object val)
        {
            this.expectedType = typeof(void);
            this.mValue = val;
        }

        public Parameter(UnityEngine.Object obj, string field)
        {
            this.expectedType = typeof(void);
            this.obj = obj;
            this.field = field;
        }

        public System.Type type
        {
            get
            {
                if (this.mValue != null)
                {
                    return this.mValue.GetType();
                }
                return this.obj?.GetType();
            }
        }

        public object value
        {
            get
            {
                if (this.mValue != null)
                {
                    return this.mValue;
                }
                if (!this.cached)
                {
                    this.cached = true;
                    this.fieldInfo = null;
                    this.propInfo = null;
                    if ((this.obj != null) && !string.IsNullOrEmpty(this.field))
                    {
                        System.Type type = this.obj.GetType();
                        this.propInfo = type.GetProperty(this.field);
                        if (this.propInfo == null)
                        {
                            this.fieldInfo = type.GetField(this.field);
                        }
                    }
                }
                if (this.propInfo != null)
                {
                    return this.propInfo.GetValue(this.obj, null);
                }
                if (this.fieldInfo != null)
                {
                    return this.fieldInfo.GetValue(this.obj);
                }
                if (this.obj != null)
                {
                    return this.obj;
                }
                if ((this.expectedType != null) && this.expectedType.IsValueType)
                {
                    return null;
                }
                return Convert.ChangeType(null, this.expectedType);
            }
            set
            {
                this.mValue = value;
            }
        }
    }
}

