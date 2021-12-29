using System;
using System.Linq;

namespace RC4
{
    public class RC4
    {
        private readonly int n;
        private readonly byte[] S = new byte[256];
        private int x;
        private int y;

        public RC4(byte[] key, int n = 256)
        {
            this.n = n;
            Init(key);
        }

        private void Init(byte[] key)
        {
            var L = key.Length;
            for (var i = 0; i < n; i++)
                S[i] = (byte)i;

            var j = 0;
            for (var i = 0; i < n; i++)
            {
                j = (j + S[i] + key[i % L]) % n;
                S.Swap(i, j);
            }
        }

        public byte[] Encode(byte[] dataB, int size)
        {
            var data = dataB.Take(size).ToArray();

            var cipher = new byte[data.Length];

            for (var m = 0; m < data.Length; m++)
                cipher[m] = (byte)(data[m] ^ PRGA());

            return cipher;
        }

        public byte[] Decode(byte[] dataB, int size)
        {
            return Encode(dataB, size);
        }

        // Pseudo-Random Generation Algorithm 
        private byte PRGA()
        {
            x = (x + 1) % n;
            y = (y + S[x]) % n;
            S.Swap(x, y);
            var t = (S[x] + S[y]) % n;
            var K = S[t];
            
            return K;
        }
    }

    internal static class SwapExtension
    {
        public static void Swap<T>(this T[] array, int index1, int index2)
        {
            (array[index1], array[index2]) = (array[index2], array[index1]);
        }
    }
}