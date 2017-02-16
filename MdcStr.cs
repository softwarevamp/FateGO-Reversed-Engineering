using ICSharpCode.SharpZipLib.BZip2;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

public class MdcStr
{
    protected const int WRITE_BUFFER_SIZE = 0x4000;

    public static string Dc(string str, uint eorData = 0, uint? crc = new uint?())
    {
        byte[] buffer5;
        byte[] buffer = Convert.FromBase64String(str);
        if (crc.HasValue && (Crc32.Compute(buffer) != crc.Value))
        {
            throw new IOException("MdcStr:Dc Crc Error");
        }
        if (eorData != 0)
        {
            byte[] buffer2 = new byte[] { (byte) eorData, (byte) (eorData >> 8), (byte) (eorData >> 0x10), (byte) (eorData >> 0x18) };
            int length = buffer.Length;
            for (int i = 0; i < length; i++)
            {
                buffer[i] = (byte) (buffer[i] ^ buffer2[i % 4]);
            }
        }
        byte[] bytes = Encoding.UTF8.GetBytes(NetworkManager.GetMk());
        byte[] rgbIV = Encoding.UTF8.GetBytes(NetworkManager.GetCv());
        TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider();
        using (MemoryStream stream = new MemoryStream())
        {
            using (CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(bytes, rgbIV), CryptoStreamMode.Write))
            {
                stream2.Write(buffer, 0, buffer.Length);
                stream2.Close();
            }
            buffer5 = stream.ToArray();
            stream.Close();
        }
        using (MemoryStream stream3 = new MemoryStream())
        {
            using (MemoryStream stream4 = new MemoryStream(buffer5))
            {
                using (BZip2InputStream stream5 = new BZip2InputStream(stream4))
                {
                    int num4;
                    byte[] buffer6 = new byte[0x4000];
                    while ((num4 = stream5.Read(buffer6, 0, buffer6.Length)) > 0)
                    {
                        stream3.Write(buffer6, 0, num4);
                    }
                    stream5.Close();
                }
                stream4.Close();
            }
            buffer5 = stream3.ToArray();
            stream3.Close();
        }
        return Encoding.UTF8.GetString(buffer5);
    }

    public static string Ec(ref uint? crc, string str, uint eorData = 0)
    {
        byte[] buffer4;
        byte[] bytes = Encoding.UTF8.GetBytes(str);
        byte[] rgbKey = Encoding.UTF8.GetBytes(NetworkManager.GetMk());
        byte[] rgbIV = Encoding.UTF8.GetBytes(NetworkManager.GetCv());
        TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider();
        using (MemoryStream stream = new MemoryStream())
        {
            using (CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write))
            {
                using (BZip2OutputStream stream3 = new BZip2OutputStream(stream2))
                {
                    stream3.Write(bytes, 0, bytes.Length);
                    stream3.Close();
                }
                stream2.Close();
            }
            buffer4 = stream.ToArray();
            stream.Close();
        }
        if (eorData != 0)
        {
            byte[] buffer5 = new byte[] { (byte) eorData, (byte) (eorData >> 8), (byte) (eorData >> 0x10), (byte) (eorData >> 0x18) };
            int length = buffer4.Length;
            for (int i = 0; i < length; i++)
            {
                buffer4[i] = (byte) (buffer4[i] ^ buffer5[i % 4]);
            }
        }
        if (crc.HasValue)
        {
            crc = new uint?(Crc32.Compute(buffer4));
        }
        return Convert.ToBase64String(buffer4);
    }
}

