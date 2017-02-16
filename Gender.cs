using System;

public class Gender
{
    protected static readonly string[] nameList = new string[] { "MALE", "FEMALE", "OTHER" };
    protected Type type;

    public Gender()
    {
        this.type = Type.OTHER;
    }

    public Gender(Type type)
    {
        this.type = type;
    }

    public Gender(string name)
    {
        this.type = Parse(name);
    }

    public Type Get() => 
        this.type;

    public static Type Parse(string name)
    {
        for (int i = 0; i < nameList.Length; i++)
        {
            if (nameList[i].Equals(name))
            {
                return (Type) (i + 1);
            }
        }
        return Type.OTHER;
    }

    public void Set(Type type)
    {
        this.type = type;
    }

    public void Set(string name)
    {
        this.type = Parse(name);
    }

    public static int ToData(string name)
    {
        for (int i = 0; i < nameList.Length; i++)
        {
            if (nameList[i].Equals(name))
            {
                return (i + 1);
            }
        }
        return 3;
    }

    public int ToInteger() => 
        ((int) this.type);

    public static string ToName(Type type) => 
        nameList[((int) type) - 1];

    public string ToString() => 
        ToName(this.type);

    public Type Value
    {
        get => 
            this.type;
        set
        {
            this.type = value;
        }
    }

    public enum Type
    {
        FEMALE = 2,
        MALE = 1,
        OTHER = 3
    }
}

