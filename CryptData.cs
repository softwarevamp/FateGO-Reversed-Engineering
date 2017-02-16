using ICSharpCode.SharpZipLib.BZip2;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

public class CryptData
{
    private const string CKEY = "b5nHjsMrqaeNliSs3jyOzgpD";
    private const string CVEC = "wuD6keVr";
    private const string FUNNY_CKEY = "ZmF0ZWdvX2FuZHJvaWRfZnVu";
    private const string FUNNY_CVEC = "ZGVzX2l2";
    private const byte mask = 4;
    protected const int WRITE_BUFFER_SIZE = 0x4000;

    public static string Decrypt(string str, bool isPress = false)
    {
        byte[] buffer4;
        byte[] buffer = Convert.FromBase64String(str);
        byte[] bytes = Encoding.UTF8.GetBytes("b5nHjsMrqaeNliSs3jyOzgpD");
        byte[] rgbIV = Encoding.UTF8.GetBytes("wuD6keVr");
        TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider();
        using (MemoryStream stream = new MemoryStream())
        {
            using (CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(bytes, rgbIV), CryptoStreamMode.Write))
            {
                stream2.Write(buffer, 0, buffer.Length);
                stream2.Close();
            }
            buffer4 = stream.ToArray();
            stream.Close();
        }
        if (isPress)
        {
            using (MemoryStream stream3 = new MemoryStream())
            {
                using (MemoryStream stream4 = new MemoryStream(buffer4))
                {
                    using (BZip2InputStream stream5 = new BZip2InputStream(stream4))
                    {
                        int num;
                        byte[] buffer5 = new byte[0x4000];
                        while ((num = stream5.Read(buffer5, 0, buffer5.Length)) > 0)
                        {
                            stream3.Write(buffer5, 0, num);
                        }
                        stream5.Close();
                    }
                    stream4.Close();
                }
                buffer4 = stream3.ToArray();
                stream3.Close();
            }
        }
        return Encoding.UTF8.GetString(buffer4);
    }

    public static string Encrypt(string str, bool isPress = false)
    {
        byte[] buffer4;
        byte[] bytes = Encoding.UTF8.GetBytes(str);
        byte[] rgbKey = Encoding.UTF8.GetBytes("b5nHjsMrqaeNliSs3jyOzgpD");
        byte[] rgbIV = Encoding.UTF8.GetBytes("wuD6keVr");
        TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider();
        using (MemoryStream stream = new MemoryStream())
        {
            using (CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write))
            {
                if (isPress)
                {
                    using (BZip2OutputStream stream3 = new BZip2OutputStream(stream2))
                    {
                        stream3.Write(bytes, 0, bytes.Length);
                        stream3.Close();
                    }
                }
                else
                {
                    stream2.Write(bytes, 0, bytes.Length);
                }
                stream2.Close();
            }
            buffer4 = stream.ToArray();
            stream.Close();
        }
        return Convert.ToBase64String(buffer4);
    }

    public static string EncryptMD5(string str)
    {
        MD5 md = new MD5CryptoServiceProvider();
        byte[] bytes = Encoding.UTF8.GetBytes(str);
        byte[] buffer2 = md.ComputeHash(bytes);
        StringBuilder builder = new StringBuilder();
        foreach (byte num in buffer2)
        {
            builder.AppendFormat("{0:x2}", num);
        }
        return builder.ToString();
    }

    public static string EncryptMD5Usk(string usk) => 
        EncryptMD5(GetDecryptFunnyKey() + usk);

    public static string FunnyKeyDecrypt(string str)
    {
        byte[] buffer4;
        byte[] buffer = Convert.FromBase64String(str);
        byte[] bytes = Encoding.UTF8.GetBytes("ZmF0ZWdvX2FuZHJvaWRfZnVu");
        byte[] rgbIV = Encoding.UTF8.GetBytes("ZGVzX2l2");
        TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider();
        using (MemoryStream stream = new MemoryStream())
        {
            using (CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(bytes, rgbIV), CryptoStreamMode.Write))
            {
                stream2.Write(buffer, 0, buffer.Length);
                stream2.Close();
            }
            buffer4 = stream.ToArray();
            stream.Close();
        }
        using (MemoryStream stream3 = new MemoryStream())
        {
            using (MemoryStream stream4 = new MemoryStream(buffer4))
            {
                int num;
                byte[] buffer5 = new byte[0x4000];
                while ((num = stream4.Read(buffer5, 0, buffer5.Length)) > 0)
                {
                    stream3.Write(buffer5, 0, num);
                }
                stream4.Close();
            }
            buffer4 = stream3.ToArray();
            stream3.Close();
        }
        return Encoding.UTF8.GetString(buffer4);
    }

    public static string GetDecryptFunnyKey() => 
        FunnyKeyDecrypt(Marshal.PtrToStringAuto(GetFunnyKey()));

    [DllImport("funnykey")]
    private static extern IntPtr GetFunnyKey();
    public static string ResponseDecrypt(string str)
    {
        byte[] bytes = Convert.FromBase64String(str);
        return Encoding.UTF8.GetString(bytes);
    }

    public static string TextDecrypt(string str)
    {
        try
        {
            byte[] buffer = Convert.FromBase64String(str);
            byte[] bytes = new byte[buffer.Length];
            int length = buffer.Length;
            for (int i = 0; i < length; i++)
            {
                bytes[i] = (byte) (buffer[i] ^ 4);
            }
            return Encoding.UTF8.GetString(bytes);
        }
        catch (Exception exception)
        {
            Debug.LogError(exception.Message);
            return null;
        }
    }

    public static string TextEncrypt(string str)
    {
        try
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            byte[] inArray = new byte[bytes.Length];
            int length = bytes.Length;
            for (int i = 0; i < length; i++)
            {
                inArray[i] = (byte) (bytes[i] ^ 4);
            }
            return Convert.ToBase64String(inArray);
        }
        catch (Exception exception)
        {
            Debug.LogError(exception.Message);
            return null;
        }
    }
}

