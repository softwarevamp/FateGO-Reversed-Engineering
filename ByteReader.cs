using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class ByteReader
{
    private byte[] mBuffer;
    private int mOffset;
    private static BetterList<string> mTemp = new BetterList<string>();

    public ByteReader(byte[] bytes)
    {
        this.mBuffer = bytes;
    }

    public ByteReader(TextAsset asset)
    {
        this.mBuffer = asset.bytes;
    }

    public static ByteReader Open(string path)
    {
        FileStream stream = File.OpenRead(path);
        if (stream != null)
        {
            stream.Seek(0L, SeekOrigin.End);
            byte[] array = new byte[stream.Position];
            stream.Seek(0L, SeekOrigin.Begin);
            stream.Read(array, 0, array.Length);
            stream.Close();
            return new ByteReader(array);
        }
        return null;
    }

    public BetterList<string> ReadCSV()
    {
        mTemp.Clear();
        string str = string.Empty;
        bool flag = false;
        int startIndex = 0;
        while (this.canRead)
        {
            if (flag)
            {
                string str2 = this.ReadLine(false);
                if (str2 == null)
                {
                    return null;
                }
                str2 = str2.Replace(@"\n", "\n");
                str = str + "\n" + str2;
            }
            else
            {
                str = this.ReadLine(true);
                if (str == null)
                {
                    return null;
                }
                str = str.Replace(@"\n", "\n");
                startIndex = 0;
            }
            int num2 = startIndex;
            int length = str.Length;
            while (num2 < length)
            {
                switch (str[num2])
                {
                    case ',':
                        if (!flag)
                        {
                            mTemp.Add(str.Substring(startIndex, num2 - startIndex));
                            startIndex = num2 + 1;
                        }
                        break;

                    case '"':
                        if (flag)
                        {
                            if ((num2 + 1) >= length)
                            {
                                mTemp.Add(str.Substring(startIndex, num2 - startIndex).Replace("\"\"", "\""));
                                return mTemp;
                            }
                            if (str[num2 + 1] != '"')
                            {
                                mTemp.Add(str.Substring(startIndex, num2 - startIndex).Replace("\"\"", "\""));
                                flag = false;
                                if (str[num2 + 1] == ',')
                                {
                                    num2++;
                                    startIndex = num2 + 1;
                                }
                            }
                            else
                            {
                                num2++;
                            }
                        }
                        else
                        {
                            startIndex = num2 + 1;
                            flag = true;
                        }
                        break;
                }
                num2++;
            }
            if (startIndex < str.Length)
            {
                if (flag)
                {
                    continue;
                }
                mTemp.Add(str.Substring(startIndex, str.Length - startIndex));
            }
            return mTemp;
        }
        return null;
    }

    public Dictionary<string, string> ReadDictionary()
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        char[] separator = new char[] { '=' };
        while (this.canRead)
        {
            string str = this.ReadLine();
            if (str == null)
            {
                return dictionary;
            }
            if (!str.StartsWith("//"))
            {
                string[] strArray = str.Split(separator, 2, StringSplitOptions.RemoveEmptyEntries);
                if (strArray.Length == 2)
                {
                    string str2 = strArray[0].Trim();
                    string str3 = strArray[1].Trim().Replace(@"\n", "\n");
                    dictionary[str2] = str3;
                }
            }
        }
        return dictionary;
    }

    public string ReadLine() => 
        this.ReadLine(true);

    public string ReadLine(bool skipEmptyLines)
    {
        string str;
        int length = this.mBuffer.Length;
        if (skipEmptyLines)
        {
            while ((this.mOffset < length) && (this.mBuffer[this.mOffset] < 0x20))
            {
                this.mOffset++;
            }
        }
        int mOffset = this.mOffset;
        if (mOffset >= length)
        {
            this.mOffset = length;
            return null;
        }
        while (mOffset < length)
        {
            switch (this.mBuffer[mOffset++])
            {
                case 10:
                case 13:
                    goto Label_0087;
            }
        }
        mOffset++;
    Label_0087:
        str = ReadLine(this.mBuffer, this.mOffset, (mOffset - this.mOffset) - 1);
        this.mOffset = mOffset;
        return str;
    }

    private static string ReadLine(byte[] buffer, int start, int count) => 
        Encoding.UTF8.GetString(buffer, start, count);

    public bool canRead =>
        ((this.mBuffer != null) && (this.mOffset < this.mBuffer.Length));
}

