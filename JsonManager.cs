using MiniJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

public class JsonManager
{
    private static JsonManager singleton = new JsonManager();
    private static StringBuilder strBuilder = new StringBuilder();

    private JsonManager()
    {
    }

    private static T CreateByDict<T>(object obj)
    {
        if (typeof(T).IsArray)
        {
            if (obj.GetType() == typeof(string))
            {
                obj = Json.Deserialize((string) obj);
            }
            int count = ((IList) obj).Count;
            System.Type elementType = typeof(T).GetElementType();
            object[] args = new object[] { count };
            object obj2 = Activator.CreateInstance(typeof(T), args);
            for (int i = 0; i < count; i++)
            {
                ((object[]) obj2)[i] = Activator.CreateInstance(elementType);
                CreateByDictInside(((object[]) obj2)[i], elementType, (Dictionary<string, object>) ((IList) obj)[i]);
            }
            return (T) obj2;
        }
        if (typeof(T) == typeof(Dictionary<string, object>))
        {
            object obj3 = Activator.CreateInstance(typeof(Dictionary<string, object>));
            CreateByDictInside(obj3, typeof(T), (Dictionary<string, object>) obj);
            return (T) obj3;
        }
        if (obj.GetType() == typeof(string))
        {
            obj = Json.Deserialize((string) obj);
        }
        T dat = Activator.CreateInstance<T>();
        CreateByDictInside(dat, typeof(T), (Dictionary<string, object>) obj);
        return dat;
    }

    private static T[] CreateByDictArray<T>(object obj)
    {
        if (obj.GetType() == typeof(string))
        {
            obj = Json.Deserialize((string) obj);
        }
        int count = ((IList) obj).Count;
        T[] localArray = new T[count];
        if (typeof(T) == typeof(object))
        {
            for (int j = 0; j < count; j++)
            {
                localArray[j] = Activator.CreateInstance<T>();
                CreateByDictInside(localArray[j], typeof(T), (Dictionary<string, object>) ((IList) obj)[j]);
            }
            return localArray;
        }
        for (int i = 0; i < count; i++)
        {
            localArray[i] = Deserialize<T>(((IList) obj)[i]);
        }
        return localArray;
    }

    private static void CreateByDictInside(object dat, System.Type t, Dictionary<string, object> dict)
    {
        if (dict != null)
        {
            foreach (KeyValuePair<string, object> pair in dict)
            {
                FieldInfo field = t.GetField(pair.Key);
                object obj2 = pair.Value;
                if (field != null)
                {
                    if (field.FieldType == typeof(int))
                    {
                        string str = obj2.ToString();
                        char[] chArray = str.ToCharArray();
                        long num = 0L;
                        if (((chArray.Length > 2) && chArray[0].Equals('0')) && chArray[1].Equals('x'))
                        {
                            num = Convert.ToInt32(str, 0x10);
                        }
                        else
                        {
                            num = long.Parse(obj2.ToString());
                        }
                        field.SetValue(dat, (int) num);
                    }
                    else if (field.FieldType == typeof(long))
                    {
                        field.SetValue(dat, long.Parse(obj2.ToString()));
                    }
                    else if (field.FieldType == typeof(bool))
                    {
                        field.SetValue(dat, bool.Parse(obj2.ToString()));
                    }
                    else if (field.FieldType == typeof(string))
                    {
                        field.SetValue(dat, obj2.ToString());
                    }
                    else if (field.FieldType == typeof(float))
                    {
                        if ((obj2.GetType() == typeof(long)) || (obj2.GetType() == typeof(int)))
                        {
                            field.SetValue(dat, (int) long.Parse(obj2.ToString()));
                        }
                        else
                        {
                            field.SetValue(dat, (float) double.Parse(obj2.ToString()));
                        }
                    }
                    else if (field.FieldType == typeof(double))
                    {
                        field.SetValue(dat, double.Parse(obj2.ToString()));
                    }
                    else if (field.FieldType.IsArray)
                    {
                        if (field.FieldType.GetElementType() == typeof(int))
                        {
                            if (obj2 != null)
                            {
                                int[] numArray = new int[((IList) obj2).Count];
                                int length = numArray.Length;
                                for (int i = 0; i < length; i++)
                                {
                                    long num4 = long.Parse(((IList) obj2)[i].ToString());
                                    numArray[i] = (int) num4;
                                }
                                field.SetValue(dat, numArray);
                            }
                        }
                        else if (field.FieldType.GetElementType() == typeof(long))
                        {
                            if (!string.IsNullOrEmpty(obj2.ToString()))
                            {
                                long[] numArray2 = new long[((IList) obj2).Count];
                                int num5 = numArray2.Length;
                                for (int j = 0; j < num5; j++)
                                {
                                    numArray2[j] = long.Parse(((IList) obj2)[j].ToString());
                                }
                                field.SetValue(dat, numArray2);
                            }
                        }
                        else if (field.FieldType.GetElementType() == typeof(double))
                        {
                            if (obj2 != null)
                            {
                                double[] numArray3 = new double[((IList) obj2).Count];
                                int num8 = numArray3.Length;
                                for (int k = 0; k < num8; k++)
                                {
                                    numArray3[k] = double.Parse(((IList) obj2)[k].ToString());
                                }
                                field.SetValue(dat, numArray3);
                            }
                        }
                        else if (field.FieldType.GetElementType() == typeof(string))
                        {
                            string[] strArray = new string[((IList) obj2).Count];
                            int num11 = strArray.Length;
                            for (int m = 0; m < num11; m++)
                            {
                                strArray[m] = (string) ((IList) obj2)[m];
                            }
                            field.SetValue(dat, strArray);
                        }
                        else
                        {
                            Array array = Array.CreateInstance(field.FieldType.GetElementType(), ((IList) obj2).Count);
                            int num13 = array.Length;
                            for (int n = 0; n < num13; n++)
                            {
                                ((object[]) array)[n] = Activator.CreateInstance(field.FieldType.GetElementType());
                                CreateByDictInside(((object[]) array)[n], field.FieldType.GetElementType(), (Dictionary<string, object>) ((IList) obj2)[n]);
                            }
                            field.SetValue(dat, array);
                        }
                    }
                    else if (field.FieldType == typeof(Dictionary<string, object>))
                    {
                        object obj3 = pair.Value;
                        if (obj3.GetType() == typeof(string))
                        {
                            obj3 = Json.Deserialize((string) obj3);
                        }
                        field.SetValue(dat, obj3);
                    }
                    else
                    {
                        object obj4 = Activator.CreateInstance(field.FieldType);
                        CreateByDictInside(obj4, field.FieldType, (Dictionary<string, object>) obj2);
                        field.SetValue(dat, obj4);
                    }
                }
                else if (dat.GetType() == typeof(Dictionary<string, object>))
                {
                    Dictionary<string, object> dictionary = (Dictionary<string, object>) dat;
                    dictionary[pair.Key] = pair.Value;
                }
            }
        }
    }

    private static void CreateJsonString(object obj)
    {
        if (obj == null)
        {
            strBuilder.Append(" null ,");
        }
        else if (obj.GetType() == typeof(Dictionary<string, object>))
        {
            strBuilder.Append(Json.Serialize(obj));
        }
        else if (obj.GetType() == typeof(List<object>))
        {
            strBuilder.Append("[");
            bool flag = false;
            int count = ((List<object>) obj).Count;
            for (int i = 0; i < count; i++)
            {
                if (flag)
                {
                    strBuilder.Append(",");
                }
                else
                {
                    flag = true;
                }
                CreateJsonString(((List<object>) obj)[i]);
            }
            strBuilder.Append("]");
        }
        else if (obj.GetType() == typeof(int))
        {
            strBuilder.Append(((int) obj).ToString());
        }
        else if (obj.GetType() == typeof(long))
        {
            strBuilder.Append(((long) obj).ToString());
        }
        else if (obj.GetType() == typeof(bool))
        {
            strBuilder.Append(!((bool) obj) ? "false" : "true");
        }
        else if (obj.GetType() == typeof(string))
        {
            strBuilder.Append("\"" + ((string) obj) + "\"");
        }
        else if (obj.GetType() == typeof(float))
        {
            strBuilder.Append(((float) obj).ToString());
        }
        else if (obj.GetType() == typeof(double))
        {
            strBuilder.Append(((double) obj).ToString());
        }
        else if (obj.GetType().IsArray)
        {
            System.Type elementType = obj.GetType().GetElementType();
            if ((((elementType == typeof(int)) || (elementType == typeof(long))) || ((elementType == typeof(bool)) || (elementType == typeof(string)))) || ((elementType == typeof(float)) || (elementType == typeof(double))))
            {
                strBuilder.Append(Json.Serialize(obj));
            }
            else
            {
                strBuilder.Append("[");
                Array array = (Array) obj;
                bool flag2 = false;
                int length = array.Length;
                for (int j = 0; j < length; j++)
                {
                    if (flag2)
                    {
                        strBuilder.Append(",");
                    }
                    else
                    {
                        flag2 = true;
                    }
                    CreateJsonString(array.GetValue(j));
                }
                strBuilder.Append("]");
            }
        }
        else
        {
            strBuilder.Append("{");
            FieldInfo[] fields = obj.GetType().GetFields();
            bool flag3 = false;
            int num5 = fields.Length;
            for (int k = 0; k < num5; k++)
            {
                if (flag3)
                {
                    strBuilder.Append(",");
                }
                else
                {
                    flag3 = true;
                }
                strBuilder.Append("\"" + fields[k].Name + "\":");
                CreateJsonString(fields[k].GetValue(obj));
            }
            strBuilder.Append("}");
        }
    }

    public static T Deserialize<T>(object obj) => 
        CreateByDict<T>(obj);

    public static T[] DeserializeArray<T>(object obj) => 
        CreateByDictArray<T>(obj);

    public static Dictionary<string, object> getDictionary(object obj) => 
        ((Dictionary<string, object>) obj);

    public static Dictionary<string, object> getDictionary(string jsonstr)
    {
        Dictionary<string, object> dictionary = (Dictionary<string, object>) Json.Deserialize(jsonstr);
        if (dictionary == null)
        {
        }
        return dictionary;
    }

    public static JsonManager GetInstance() => 
        singleton;

    public static string toJson(object obj)
    {
        strBuilder.Length = 0;
        CreateJsonString(obj);
        return strBuilder.ToString();
    }
}

