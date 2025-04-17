using System.Security.Cryptography;

const int ivSize = 16; // AES block size in bytes
const string plainText = "Hello world!";
byte[] masterKey = RandomNumberGenerator.GetBytes(32); // 256 bits

Console.WriteLine($"Plain Text: {plainText}");

string encrypted = Encrypt(plainText, masterKey);
Console.WriteLine($"Encrypted: {encrypted}");

string decrypted = Decrypt(encrypted, masterKey);
Console.WriteLine($"Decrypted: {decrypted}");

Console.ReadLine();

static string Encrypt(string plainText, byte[] masterKey)
{
    try
    {
        using Aes aes = Aes.Create();
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        aes.Key = masterKey;
        aes.IV = RandomNumberGenerator.GetBytes(ivSize);

        using MemoryStream ms = new();
        ms.Write(aes.IV, 0, ivSize);

        using (ICryptoTransform encryptor = aes.CreateEncryptor())
        using (CryptoStream cs = new(ms, encryptor, CryptoStreamMode.Write))
        using (StreamWriter sw = new(cs))
        {
            sw.Write(plainText);
        }

        return Convert.ToBase64String(ms.ToArray());
    }
    catch (CryptographicException ex)
    {
        throw new InvalidOperationException("Ecryption failed.", ex);
    }
}

static string Decrypt(string cipherText, byte[] masterKey)
{
    try
    {
        byte[] cipherData = Convert.FromBase64String(cipherText);
        if (cipherData.Length < ivSize)
        {
            throw new ArgumentException("Invalid cipher text format.", nameof(cipherText));
        }

        byte[] iv = new byte[ivSize];
        byte[] encryptedData = new byte[cipherData.Length - ivSize];

        Buffer.BlockCopy(cipherData, 0, iv, 0, ivSize); //first 16 bytes are IV
        Buffer.BlockCopy(cipherData, ivSize, encryptedData, 0, encryptedData.Length); // remaining bytes are encrypted data

        using Aes aes = Aes.Create();
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        aes.Key = masterKey;
        aes.IV = iv;

        using MemoryStream ms = new(encryptedData);
        using ICryptoTransform decryptor = aes.CreateDecryptor();
        using CryptoStream cs = new(ms, decryptor, CryptoStreamMode.Read);
        using StreamReader sr = new(cs);

        return sr.ReadToEnd();
    }
    catch (CryptographicException ex)
    {
        throw new InvalidOperationException("Decryption failed.", ex);
    }
}
