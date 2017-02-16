using System;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;

[Serializable]
public class PropertyReference
{
    private FieldInfo mField;
    [SerializeField]
    private string mName;
    private PropertyInfo mProperty;
    [SerializeField]
    private Component mTarget;
    private static int s_Hash = "PropertyBinding".GetHashCode();

    public PropertyReference()
    {
    }

    public PropertyReference(Component target, string fieldName)
    {
        this.mTarget = target;
        this.mName = fieldName;
    }

    [DebuggerHidden, DebuggerStepThrough]
    private bool Cache()
    {
        if ((this.mTarget != null) && !string.IsNullOrEmpty(this.mName))
        {
            System.Type type = this.mTarget.GetType();
            this.mField = type.GetField(this.mName);
            this.mProperty = type.GetProperty(this.mName);
        }
        else
        {
            this.mField = null;
            this.mProperty = null;
        }
        return ((this.mField != null) || (this.mProperty != null));
    }

    public void Clear()
    {
        this.mTarget = null;
        this.mName = null;
    }

    private bool Convert(ref object value)
    {
        System.Type type;
        if (this.mTarget == null)
        {
            return false;
        }
        System.Type propertyType = this.GetPropertyType();
        if (value == null)
        {
            if (!propertyType.IsClass)
            {
                return false;
            }
            type = propertyType;
        }
        else
        {
            type = value.GetType();
        }
        return Convert(ref value, type, propertyType);
    }

    public static bool Convert(object value, System.Type to)
    {
        if (value == null)
        {
            value = null;
            return Convert(ref value, to, to);
        }
        return Convert(ref value, value.GetType(), to);
    }

    public static bool Convert(System.Type from, System.Type to)
    {
        object obj2 = null;
        return Convert(ref obj2, from, to);
    }

    public static bool Convert(ref object value, System.Type from, System.Type to)
    {
        if (to.IsAssignableFrom(from))
        {
            return true;
        }
        if (to == typeof(string))
        {
            value = (value == null) ? "null" : value.ToString();
            return true;
        }
        if (value != null)
        {
            if (to == typeof(int))
            {
                if (from == typeof(string))
                {
                    int num;
                    if (int.TryParse((string) value, out num))
                    {
                        value = num;
                        return true;
                    }
                }
                else if (from == typeof(float))
                {
                    value = Mathf.RoundToInt((float) value);
                    return true;
                }
            }
            else
            {
                float num2;
                if (((to == typeof(float)) && (from == typeof(string))) && float.TryParse((string) value, out num2))
                {
                    value = num2;
                    return true;
                }
            }
        }
        return false;
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
        {
            return !this.isValid;
        }
        if (obj is PropertyReference)
        {
            PropertyReference reference = obj as PropertyReference;
            return ((this.mTarget == reference.mTarget) && string.Equals(this.mName, reference.mName));
        }
        return false;
    }

    [DebuggerHidden, DebuggerStepThrough]
    public object Get()
    {
        if (((this.mProperty == null) && (this.mField == null)) && this.isValid)
        {
            this.Cache();
        }
        if (this.mProperty != null)
        {
            if (this.mProperty.CanRead)
            {
                return this.mProperty.GetValue(this.mTarget, null);
            }
        }
        else if (this.mField != null)
        {
            return this.mField.GetValue(this.mTarget);
        }
        return null;
    }

    public override int GetHashCode() => 
        s_Hash;

    public System.Type GetPropertyType()
    {
        if (((this.mProperty == null) && (this.mField == null)) && this.isValid)
        {
            this.Cache();
        }
        if (this.mProperty != null)
        {
            return this.mProperty.PropertyType;
        }
        if (this.mField != null)
        {
            return this.mField.FieldType;
        }
        return typeof(void);
    }

    public void Reset()
    {
        this.mField = null;
        this.mProperty = null;
    }

    [DebuggerHidden, DebuggerStepThrough]
    public bool Set(object value)
    {
        if (((this.mProperty == null) && (this.mField == null)) && this.isValid)
        {
            this.Cache();
        }
        if ((this.mProperty != null) || (this.mField != null))
        {
            if (value == null)
            {
                try
                {
                    if (this.mProperty != null)
                    {
                        if (this.mProperty.CanWrite)
                        {
                            this.mProperty.SetValue(this.mTarget, null, null);
                            return true;
                        }
                    }
                    else
                    {
                        this.mField.SetValue(this.mTarget, null);
                        return true;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }
            if (!this.Convert(ref value))
            {
                if (Application.isPlaying)
                {
                    UnityEngine.Debug.LogError(string.Concat(new object[] { "Unable to convert ", value.GetType(), " to ", this.GetPropertyType() }));
                }
            }
            else
            {
                if (this.mField != null)
                {
                    this.mField.SetValue(this.mTarget, value);
                    return true;
                }
                if (this.mProperty.CanWrite)
                {
                    this.mProperty.SetValue(this.mTarget, value, null);
                    return true;
                }
            }
        }
        return false;
    }

    public void Set(Component target, string methodName)
    {
        this.mTarget = target;
        this.mName = methodName;
    }

    public override string ToString() => 
        ToString(this.mTarget, this.name);

    public static string ToString(Component comp, string property)
    {
        if (comp == null)
        {
            return null;
        }
        string str = comp.GetType().ToString();
        int num = str.LastIndexOf('.');
        if (num > 0)
        {
            str = str.Substring(num + 1);
        }
        if (!string.IsNullOrEmpty(property))
        {
            return (str + "." + property);
        }
        return (str + ".[property]");
    }

    public bool isEnabled
    {
        get
        {
            if (this.mTarget == null)
            {
                return false;
            }
            MonoBehaviour mTarget = this.mTarget as MonoBehaviour;
            return ((mTarget == null) || mTarget.enabled);
        }
    }

    public bool isValid =>
        ((this.mTarget != null) && !string.IsNullOrEmpty(this.mName));

    public string name
    {
        get => 
            this.mName;
        set
        {
            this.mName = value;
            this.mProperty = null;
            this.mField = null;
        }
    }

    public Component target
    {
        get => 
            this.mTarget;
        set
        {
            this.mTarget = value;
            this.mProperty = null;
            this.mField = null;
        }
    }
}

