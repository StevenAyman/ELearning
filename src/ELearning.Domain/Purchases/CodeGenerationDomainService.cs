using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Domain.Purchases;
public sealed class CodeGenerationDomainService
{
    private const string Alphabet = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
    private static long _counter = DateTime.UtcNow.Ticks;
    
    public string GenerateCode(int length = 16)
    {
        if (length % 4 != 0)
        {
            return null;
        }

        var counterValue = Interlocked.Increment(ref _counter);

        var uniquePart = EncodeToAlphabet(counterValue, 4);

        var randomBytes = new byte[length - 4];
        RandomNumberGenerator.Fill(randomBytes);
        var randomPart = EncodeToAlphabet(BitConverter.ToUInt32(randomBytes), length - 4);

        var stringBuilder = new StringBuilder(uniquePart);
        for (int i = 0; i < length - 4; i+= 4)
        {
            stringBuilder.Append('-');
            stringBuilder.Append(randomPart[i..(i + 4)]);
        }

        return stringBuilder.ToString();
    }

    private string EncodeToAlphabet(long value, int length)
    {
        var result = new char[length];
        for (int i = length - 1; i >= 0; i--)
        {
            result[i] = Alphabet[(int)(value % Alphabet.Length)];
            value /= Alphabet.Length;
        }

        return string.Join("", result);
    }
}
