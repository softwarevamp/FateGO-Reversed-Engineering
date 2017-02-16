namespace MiniJSON
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Text;

    public static class Json
    {
        public static object Deserialize(string json)
        {
            if (json == null)
            {
                return null;
            }
            return Parser.Parse(json);
        }

        public static string Serialize(object obj) => 
            Serializer.Serialize(obj);

        private sealed class Parser : IDisposable
        {
            [CompilerGenerated]
            private static Dictionary<string, int> <>f__switch$map3;
            private string emptyString = string.Empty;
            private StringBuilder g_s = new StringBuilder();
            private MyStringReader json;
            private const string WORD_BREAK = ",:{}\"[]";

            private Parser(string jsonString)
            {
                this.json = new MyStringReader(jsonString);
            }

            public void Dispose()
            {
                this.json.Dispose();
                this.json = null;
            }

            private void EatWhitespace()
            {
                while (char.IsWhiteSpace(this.PeekChar))
                {
                    this.json.nextChar();
                    if (this.json.Peek() == -1)
                    {
                        break;
                    }
                }
            }

            public static bool IsWordBreak(char c) => 
                (char.IsWhiteSpace(c) || (",:{}\"[]".IndexOf(c) != -1));

            public static object Parse(string jsonString)
            {
                using (Json.Parser parser = new Json.Parser(jsonString))
                {
                    return parser.ParseValue();
                }
            }

            private List<object> ParseArray()
            {
                List<object> list = new List<object>();
                this.json.nextChar();
                bool flag = true;
                while (flag)
                {
                    TOKEN nextToken = this.NextToken;
                    switch (nextToken)
                    {
                        case TOKEN.SQUARED_CLOSE:
                        {
                            flag = false;
                            continue;
                        }
                        case TOKEN.COMMA:
                        {
                            continue;
                        }
                        case TOKEN.NONE:
                            return null;
                    }
                    object item = this.ParseByToken(nextToken);
                    list.Add(item);
                }
                return list;
            }

            private object ParseByToken(TOKEN token)
            {
                switch (token)
                {
                    case TOKEN.CURLY_OPEN:
                        return this.ParseObject();

                    case TOKEN.SQUARED_OPEN:
                        return this.ParseArray();

                    case TOKEN.STRING:
                        return this.ParseString();

                    case TOKEN.NUMBER:
                        return this.ParseNumber();

                    case TOKEN.TRUE:
                        return true;

                    case TOKEN.FALSE:
                        return false;

                    case TOKEN.NULL:
                        return null;
                }
                return null;
            }

            private object ParseNumber()
            {
                double num5;
                bool flag = false;
                int startIndex = this.json.getPos();
                for (char ch = this.PeekChar; !IsWordBreak(ch); ch = this.PeekChar)
                {
                    if (ch == '.')
                    {
                        flag = true;
                    }
                    this.json.nextChar();
                    if (this.json.isEnd())
                    {
                        break;
                    }
                }
                int num2 = this.json.getPos();
                if (!flag)
                {
                    long num3 = 0L;
                    string str = this.json.getString();
                    int num4 = 1;
                    if (str[startIndex] == '-')
                    {
                        num4 = -1;
                        startIndex++;
                    }
                    num3 = str[startIndex++] - '0';
                    while (startIndex < num2)
                    {
                        num3 = ((num3 << 3) + num3) + num3;
                        num3 += str[startIndex++] - '0';
                    }
                    return (num3 * num4);
                }
                double.TryParse(this.json.getString().Substring(startIndex, num2 - startIndex), out num5);
                return num5;
            }

            private Dictionary<string, object> ParseObject()
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                this.json.nextChar();
                while (true)
                {
                    TOKEN nextToken = this.NextToken;
                    switch (nextToken)
                    {
                        case TOKEN.NONE:
                            return null;

                        case TOKEN.CURLY_CLOSE:
                            return dictionary;
                    }
                    if (nextToken != TOKEN.COMMA)
                    {
                        string str = this.ParseString();
                        if (str == null)
                        {
                            return null;
                        }
                        if (this.NextToken != TOKEN.COLON)
                        {
                            return null;
                        }
                        this.json.nextChar();
                        dictionary[str] = this.ParseValue();
                    }
                }
            }

            private string ParseString()
            {
                this.json.nextChar();
                if (this.PeekChar == '\\')
                {
                    return this.ParseStringMeta();
                }
                int pos = this.json.getPos();
                int num2 = 0;
                bool flag = true;
                while (flag)
                {
                    if (this.json.isEnd())
                    {
                        flag = false;
                        num2 = this.json.getPos();
                        break;
                    }
                    char nextChar = this.NextChar;
                    if (nextChar == '"')
                    {
                        flag = false;
                        num2 = this.json.getPos() - 1;
                    }
                    else if (nextChar == '\\')
                    {
                        this.json.setPos(pos);
                        return this.ParseStringMeta();
                    }
                }
                if (pos == num2)
                {
                    return this.emptyString;
                }
                return this.json.getString().Substring(pos, num2 - pos);
            }

            private string ParseStringMeta()
            {
                this.g_s.Length = 0;
                StringBuilder builder = this.g_s;
                bool flag = true;
                while (flag)
                {
                    int num;
                    int num2;
                    int num3;
                    char ch2;
                    if (this.json.isEnd())
                    {
                        flag = false;
                        break;
                    }
                    char nextChar = this.NextChar;
                    char ch3 = nextChar;
                    if (ch3 == '"')
                    {
                        flag = false;
                        continue;
                    }
                    if (ch3 != '\\')
                    {
                        goto Label_023E;
                    }
                    if (this.json.isEnd())
                    {
                        flag = false;
                        continue;
                    }
                    nextChar = this.NextChar;
                    char ch4 = nextChar;
                    switch (ch4)
                    {
                        case 'n':
                        {
                            builder.Append('\n');
                            continue;
                        }
                        case 'r':
                        {
                            builder.Append('\r');
                            continue;
                        }
                        case 't':
                        {
                            builder.Append('\t');
                            continue;
                        }
                        case 'u':
                            num = 0;
                            num2 = 0;
                            num3 = 0;
                            goto Label_01D1;

                        default:
                        {
                            switch (ch4)
                            {
                                case '"':
                                case '/':
                                case '\\':
                                    goto Label_01E7;

                                case 'b':
                                    goto Label_01F4;

                                case 'f':
                                    goto Label_0201;
                            }
                            continue;
                        }
                    }
                Label_00E3:
                    ch2 = this.NextChar;
                    if ((ch2 <= '9') && ('0' <= ch2))
                    {
                        num2 = ch2 - '0';
                    }
                    else
                    {
                        switch (ch2)
                        {
                            case 'A':
                                num2 = 10;
                                break;

                            case 'B':
                                num2 = 11;
                                break;

                            case 'C':
                                num2 = 12;
                                break;

                            case 'D':
                                num2 = 13;
                                break;

                            case 'E':
                                num2 = 14;
                                break;

                            case 'F':
                                num2 = 15;
                                break;

                            case 'a':
                                num2 = 10;
                                break;

                            case 'b':
                                num2 = 11;
                                break;

                            case 'c':
                                num2 = 12;
                                break;

                            case 'd':
                                num2 = 13;
                                break;

                            case 'e':
                                num2 = 14;
                                break;

                            case 'f':
                                num2 = 15;
                                break;
                        }
                    }
                    num = num << 4;
                    num |= num2;
                    num3++;
                Label_01D1:
                    if (num3 < 4)
                    {
                        goto Label_00E3;
                    }
                    builder.Append((char) num);
                    continue;
                Label_01E7:
                    builder.Append(nextChar);
                    continue;
                Label_01F4:
                    builder.Append('\b');
                    continue;
                Label_0201:
                    builder.Append('\f');
                    continue;
                Label_023E:
                    builder.Append(nextChar);
                }
                return builder.ToString();
            }

            private object ParseValue()
            {
                TOKEN nextToken = this.NextToken;
                return this.ParseByToken(nextToken);
            }

            private char NextChar =>
                ((char) this.json.Read());

            private TOKEN NextToken
            {
                get
                {
                    this.EatWhitespace();
                    if (!this.json.isEnd())
                    {
                        char peekChar = this.PeekChar;
                        if ((peekChar <= '9') && ('0' <= peekChar))
                        {
                            return TOKEN.NUMBER;
                        }
                        switch (this.PeekChar)
                        {
                            case '[':
                                return TOKEN.SQUARED_OPEN;

                            case ']':
                                this.json.nextChar();
                                return TOKEN.SQUARED_CLOSE;

                            case '{':
                                return TOKEN.CURLY_OPEN;

                            case '}':
                                this.json.nextChar();
                                return TOKEN.CURLY_CLOSE;

                            case ',':
                                this.json.nextChar();
                                return TOKEN.COMMA;

                            case '-':
                                return TOKEN.NUMBER;

                            case '"':
                                return TOKEN.STRING;

                            case ':':
                                return TOKEN.COLON;
                        }
                        string nextWord = this.NextWord;
                        if (nextWord != null)
                        {
                            int num;
                            if (<>f__switch$map3 == null)
                            {
                                Dictionary<string, int> dictionary = new Dictionary<string, int>(3) {
                                    { 
                                        "false",
                                        0
                                    },
                                    { 
                                        "true",
                                        1
                                    },
                                    { 
                                        "null",
                                        2
                                    }
                                };
                                <>f__switch$map3 = dictionary;
                            }
                            if (<>f__switch$map3.TryGetValue(nextWord, out num))
                            {
                                switch (num)
                                {
                                    case 0:
                                        return TOKEN.FALSE;

                                    case 1:
                                        return TOKEN.TRUE;

                                    case 2:
                                        return TOKEN.NULL;
                                }
                            }
                        }
                    }
                    return TOKEN.NONE;
                }
            }

            private string NextWord
            {
                get
                {
                    int startIndex = this.json.getPos();
                    for (char ch = this.PeekChar; !IsWordBreak(ch); ch = this.PeekChar)
                    {
                        if (this.json.isEnd())
                        {
                            break;
                        }
                        this.json.nextChar();
                    }
                    int num2 = this.json.getPos();
                    return this.json.getString().Substring(startIndex, num2 - startIndex);
                }
            }

            private char PeekChar =>
                ((char) this.json.Peek());

            private class MyStringReader
            {
                private int m_end;
                private int m_pos;
                private string m_s;

                public MyStringReader(string s)
                {
                    this.m_s = s;
                    this.m_pos = 0;
                    this.m_end = this.m_s.Length;
                }

                public void Dispose()
                {
                    this.m_s = null;
                    this.m_pos = 0;
                    this.m_end = 0;
                }

                public int getPos() => 
                    this.m_pos;

                public string getString() => 
                    this.m_s;

                public bool isEnd() => 
                    (this.m_pos == this.m_end);

                public void nextChar()
                {
                    this.m_pos++;
                }

                public int Peek()
                {
                    if (this.m_pos == this.m_end)
                    {
                        return -1;
                    }
                    return this.m_s[this.m_pos];
                }

                public int Read()
                {
                    if (this.m_pos == this.m_end)
                    {
                        return -1;
                    }
                    return this.m_s[this.m_pos++];
                }

                public void setPos(int pos)
                {
                    this.m_pos = pos;
                }
            }

            private enum TOKEN
            {
                NONE,
                CURLY_OPEN,
                CURLY_CLOSE,
                SQUARED_OPEN,
                SQUARED_CLOSE,
                COLON,
                COMMA,
                STRING,
                NUMBER,
                TRUE,
                FALSE,
                NULL
            }
        }

        private sealed class Serializer
        {
            private StringBuilder builder = new StringBuilder();

            private Serializer()
            {
            }

            public static string Serialize(object obj)
            {
                Json.Serializer serializer = new Json.Serializer();
                serializer.SerializeValue(obj);
                return serializer.builder.ToString();
            }

            private void SerializeArray(IList anArray)
            {
                this.builder.Append('[');
                bool flag = true;
                int count = anArray.Count;
                for (int i = 0; i < count; i++)
                {
                    if (!flag)
                    {
                        this.builder.Append(',');
                    }
                    this.SerializeValue(anArray[i]);
                    flag = false;
                }
                this.builder.Append(']');
            }

            private void SerializeObject(IDictionary obj)
            {
                bool flag = true;
                this.builder.Append('{');
                IEnumerator enumerator = obj.Keys.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        object current = enumerator.Current;
                        if (!flag)
                        {
                            this.builder.Append(',');
                        }
                        this.SerializeString(current.ToString());
                        this.builder.Append(':');
                        this.SerializeValue(obj[current]);
                        flag = false;
                    }
                }
                finally
                {
                    IDisposable disposable = enumerator as IDisposable;
                    if (disposable == null)
                    {
                    }
                    disposable.Dispose();
                }
                this.builder.Append('}');
            }

            private void SerializeOther(object value)
            {
                if (value is float)
                {
                    this.builder.Append(((float) value).ToString("R"));
                }
                else if ((((value is int) || (value is uint)) || ((value is long) || (value is sbyte))) || (((value is byte) || (value is short)) || ((value is ushort) || (value is ulong))))
                {
                    this.builder.Append(value);
                }
                else if ((value is double) || (value is decimal))
                {
                    this.builder.Append(Convert.ToDouble(value).ToString("R"));
                }
                else
                {
                    this.SerializeString(value.ToString());
                }
            }

            private void SerializeString(string str)
            {
                this.builder.Append('"');
                char[] chArray = str.ToCharArray();
                int length = chArray.Length;
                for (int i = 0; i < length; i++)
                {
                    int num3;
                    char ch = chArray[i];
                    switch (ch)
                    {
                        case '\b':
                        {
                            this.builder.Append(@"\b");
                            continue;
                        }
                        case '\t':
                        {
                            this.builder.Append(@"\t");
                            continue;
                        }
                        case '\n':
                        {
                            this.builder.Append(@"\n");
                            continue;
                        }
                        case '\f':
                        {
                            this.builder.Append(@"\f");
                            continue;
                        }
                        case '\r':
                        {
                            this.builder.Append(@"\r");
                            continue;
                        }
                        default:
                        {
                            if (ch != '"')
                            {
                                if (ch == '\\')
                                {
                                    break;
                                }
                                goto Label_00F7;
                            }
                            this.builder.Append("\\\"");
                            continue;
                        }
                    }
                    this.builder.Append(@"\\");
                    continue;
                Label_00F7:
                    num3 = Convert.ToInt32(chArray[i]);
                    if ((num3 >= 0x20) && (num3 <= 0x7e))
                    {
                        this.builder.Append(chArray[i]);
                    }
                    else
                    {
                        this.builder.Append(@"\u");
                        this.builder.Append(num3.ToString("x4"));
                    }
                }
                this.builder.Append('"');
            }

            private void SerializeValue(object value)
            {
                if (value == null)
                {
                    this.builder.Append("null");
                }
                else
                {
                    string str = value as string;
                    if (str != null)
                    {
                        this.SerializeString(str);
                    }
                    else if (value is bool)
                    {
                        this.builder.Append(!((bool) value) ? "false" : "true");
                    }
                    else
                    {
                        IList anArray = value as IList;
                        if (anArray != null)
                        {
                            this.SerializeArray(anArray);
                        }
                        else
                        {
                            IDictionary dictionary = value as IDictionary;
                            if (dictionary != null)
                            {
                                this.SerializeObject(dictionary);
                            }
                            else if (value is char)
                            {
                                this.SerializeString(new string((char) value, 1));
                            }
                            else
                            {
                                this.SerializeOther(value);
                            }
                        }
                    }
                }
            }
        }
    }
}

