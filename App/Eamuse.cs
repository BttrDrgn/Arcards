using System;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

public static class Eamuse
{
    // Original key and its preprocessed version (_KEY)
    private static readonly byte[] KEY = Encoding.ASCII.GetBytes("?I'llB2c.YouXXXeMeHaYpy!");
    private static readonly byte[] _KEY = KEY.Select(b => (byte)(b * 2)).ToArray();

    // Alphabet for the conversion
    private const string ALPHABET = "0123456789ABCDEFGHJKLMNPRSTUWXYZ";

    // DES3 encryption: encrypts the given byte array using TripleDES (CBC mode, IV = zeros)
    public static byte[] EncDes(byte[] uid)
    {
        using (var tripleDes = TripleDESCryptoServiceProvider.Create())
        {
            tripleDes.Key = _KEY;
            tripleDes.Mode = CipherMode.CBC;
            tripleDes.Padding = PaddingMode.None;
            tripleDes.IV = new byte[8];
            using (var encryptor = tripleDes.CreateEncryptor())
            {
                return encryptor.TransformFinalBlock(uid, 0, uid.Length);
            }
        }
    }

    // DES3 decryption: decrypts the given byte array using TripleDES (CBC mode, IV = zeros)
    public static byte[] DecDes(byte[] uid)
    {
        using (var tripleDes = TripleDESCryptoServiceProvider.Create())
        {
            tripleDes.Key = _KEY;
            tripleDes.Mode = CipherMode.CBC;
            tripleDes.Padding = PaddingMode.None;
            tripleDes.IV = new byte[8];
            using (var decryptor = tripleDes.CreateDecryptor())
            {
                return decryptor.TransformFinalBlock(uid, 0, uid.Length);
            }
        }
    }

    // Computes the checksum over the first 15 bytes/integers of the data.
    // It sums each value multiplied by (index mod 3 + 1) then reduces it until it fits in 5 bits.
    public static int Checksum(int[] data)
    {
        if (data.Length < 15) throw new ArgumentException("Data length must be at least 15 elements.");

        int chk = 0;
        for (int i = 0; i < 15; i++)
        {
            chk += data[i] * ((i % 3) + 1);
        }
        while (chk > 31)
        {
            chk = (chk >> 5) + (chk & 31);
        }
        return chk;
    }

    // Packs an array of 5-bit integers into a byte array.
    public static byte[] Pack5(int[] data)
    {
        // Build binary string: each integer is converted to a 5-bit binary string.
        StringBuilder sb = new StringBuilder();
        foreach (int i in data)
        {
            sb.Append(Convert.ToString(i, 2).PadLeft(5, '0'));
        }
        // Pad with zeros so that the length is a multiple of 8.
        int mod = sb.Length % 8;
        if (mod != 0)
        {
            sb.Append(new string('0', 8 - mod));
        }
        string bits = sb.ToString();
        int byteCount = bits.Length / 8;
        byte[] result = new byte[byteCount];
        for (int i = 0; i < byteCount; i++)
        {
            string byteStr = bits.Substring(i * 8, 8);
            result[i] = Convert.ToByte(byteStr, 2);
        }
        return result;
    }

    // Unpacks a byte array into an array of 5-bit integers.
    public static int[] Unpack5(byte[] data)
    {
        // Convert each byte to its 8-bit binary representation and concatenate.
        StringBuilder sb = new StringBuilder();
        foreach (byte b in data)
        {
            sb.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
        }
        string bits = sb.ToString();
        // Pad with zeros so that the length is a multiple of 5.
        int mod = bits.Length % 5;
        if (mod != 0)
        {
            bits = bits.PadRight(bits.Length + (5 - mod), '0');
        }
        int count = bits.Length / 5;
        int[] result = new int[count];
        for (int i = 0; i < count; i++)
        {
            string chunk = bits.Substring(i * 5, 5);
            result[i] = Convert.ToInt32(chunk, 2);
        }
        return result;
    }

    // Converts a UID (a 16-character hexadecimal string) to a Konami ID.
    public static string ToKonamiId(string uid)
    {
        if (uid.Length != 16) throw new ArgumentException("UID must be 16 characters.");

        int cardType;
        if (uid.ToUpper().StartsWith("E004")) cardType = 1;
        else if (uid.ToUpper().StartsWith("0")) cardType = 2;
        else throw new ArgumentException("Invalid UID prefix.");

        // Convert the hex string to a byte array (8 bytes expected).
        byte[] kid = Enumerable.Range(0, uid.Length / 2)
                               .Select(i => Convert.ToByte(uid.Substring(i * 2, 2), 16))
                               .ToArray();
        if (kid.Length != 8) throw new ArgumentException("ID must be 8 bytes.");

        // Reverse kid
        byte[] kidReversed = kid.Reverse().ToArray();

        // Encrypt the reversed kid and unpack into 5-bit values.
        int[] unpacked = Unpack5(EncDes(kidReversed));
        // Take first 13 numbers and add three zeros.
        int[] outArray = unpacked.Take(13).Concat(new int[] { 0, 0, 0 }).ToArray();

        // XOR the first element with the card type.
        outArray[0] ^= cardType;
        // Set index 13 to 1.
        outArray[13] = 1;
        // For indices 1 to 13, XOR with previous element.
        for (int i = 1; i < 14; i++)
        {
            outArray[i] ^= outArray[i - 1];
        }
        // Set index 14 to card type.
        outArray[14] = cardType;
        // Compute checksum for index 15.
        outArray[15] = Checksum(outArray);

        // Map each 5-bit value to a character from ALPHABET.
        string result = new string(outArray.Select(val => ALPHABET[val]).ToArray());
        return result;
    }

    // Converts a Konami ID (a 16-character string from ALPHABET) back to a UID (hexadecimal string).
    public static string ToUid(string konamiId)
    {
        if (konamiId.Length != 16) throw new ArgumentException("ID must be 16 characters.");
        if (!konamiId.All(c => ALPHABET.Contains(c))) throw new ArgumentException("ID contains invalid characters.");

        int cardType;
        // The card type is determined by the character at index 14.
        if (konamiId[14] == '1') cardType = 1;
        else if (konamiId[14] == '2') cardType = 2;
        else throw new ArgumentException("Invalid ID");

        // Convert each character to its index in ALPHABET.
        int[] card = konamiId.Select(c => ALPHABET.IndexOf(c)).ToArray();

        // Parity checks
        if (card[11] % 2 != card[12] % 2) throw new ArgumentException("Parity check failed");
        if (card[13] != (card[12] ^ 1)) throw new ArgumentException("Card invalid");
        if (card[15] != Checksum(card)) throw new ArgumentException("Checksum failed");

        // Reverse the XOR operations.
        for (int i = 13; i >= 1; i--)
        {
            card[i] ^= card[i - 1];
        }
        card[0] ^= cardType;

        // Pack the first 13 5-bit numbers into bytes and take the first 8 bytes.
        int[] first13 = card.Take(13).ToArray();
        byte[] packed = Pack5(first13);
        byte[] eightBytes = packed.Take(8).ToArray();

        // Decrypt and then reverse the decrypted array.
        byte[] decrypted = DecDes(eightBytes);
        byte[] decryptedReversed = decrypted.Reverse().ToArray();

        // Convert the result to a hexadecimal string.
        string cardId = BitConverter.ToString(decryptedReversed).Replace("-", "").ToUpper();

        if (cardType == 1)
        {
            if (!cardId.StartsWith("E004")) throw new ArgumentException("Invalid card type");
        }
        else if (cardType == 2)
        {
            if (!cardId.StartsWith("0")) throw new ArgumentException("Invalid card type");
        }
        return cardId;
    }

    public static string GenerateRandomCard()
    {
        // Generate a random 8-byte UID
        byte[] uid = new byte[8];
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(uid);
        }

        return "0" + BitConverter.ToString(uid).Replace("-", "").ToUpper().Substring(1);
    }
}
