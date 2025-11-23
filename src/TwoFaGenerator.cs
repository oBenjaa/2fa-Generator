using System;
using System.Text;
using System.Security.Cryptography;
using QRCoder;
using System.Drawing;
using System.IO;

class TwoFaGenerator
{
    static string Base32Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

    static string GenerateSecret(int length = 16)
    {
        byte[] buffer = new byte[length];
        RandomNumberGenerator.Fill(buffer);
        StringBuilder sb = new();
        foreach (byte b in buffer)
            sb.Append(Base32Chars[b % Base32Chars.Length]);
        return sb.ToString();
    }

    static byte[] Base32Decode(string input)
    {
        input = input.ToUpper().TrimEnd('=');
        MemoryStream stream = new();
        int buffer = 0, bitsLeft = 0;
        foreach (char c in input)
        {
            int val = Base32Chars.IndexOf(c);
            if (val < 0) continue;
            buffer = (buffer << 5) | val;
            bitsLeft += 5;
            if (bitsLeft >= 8)
            {
                stream.WriteByte((byte)(buffer >> (bitsLeft - 8)));
                bitsLeft -= 8;
            }
        }
        return stream.ToArray();
    }

    static string GenerateTOTP(string secret, int digits = 6, int period = 30)
    {
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds() / period;
        byte[] key = Base32Decode(secret);
        byte[] counter = BitConverter.GetBytes(timestamp);
        if (BitConverter.IsLittleEndian) Array.Reverse(counter);
        using var hmac = new HMACSHA1(key);
        byte[] hash = hmac.ComputeHash(counter);
        int offset = hash[^1] & 0x0F;
        int code = ((hash[offset] & 0x7F) << 24) |
                   ((hash[offset + 1] & 0xFF) << 16) |
                   ((hash[offset + 2] & 0xFF) << 8) |
                   (hash[offset + 3] & 0xFF);
        return (code % (int)Math.Pow(10, digits)).ToString(new string('0', digits));
    }

    static void Main()
    {
        string secret = GenerateSecret();
        Console.WriteLine("Secret: " + secret);

        string totp = GenerateTOTP(secret);
        Console.WriteLine("Código TOTP atual: " + totp);

        string issuer = "MeuServico";
        string account = "benja@example.com";
        string uri = $"otpauth://totp/{issuer}:{account}?secret={secret}&issuer={issuer}&algorithm=SHA1&digits=6&period=30";
        Console.WriteLine("URI otpauth: " + uri);

        using QRCodeGenerator qrGen = new();
        using QRCodeData data = qrGen.CreateQrCode(uri, QRCodeGenerator.ECCLevel.Q);
        using QRCode qr = new(data);
        Bitmap bmp = qr.GetGraphic(20);
        bmp.Save("totp_qr.png");
        Console.WriteLine("QR Code salvo como totp_qr.png");
    }
}