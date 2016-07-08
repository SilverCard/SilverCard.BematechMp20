using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilverCard.BematechMp20
{
    public static class CodificadorAbicomp
    {
        private static Char[] _chars;

        static CodificadorAbicomp()
        {
            _chars = new Char[256];

            byte[] bytes = new byte[0x7f + 1];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte)i;
            }
            Encoding.ASCII.GetChars(bytes, 0, bytes.Length, _chars, 0);

            String AbicompChars = "\x20ÀÁÂÃÄÇÈÉÊËÌÍÎÏÑÒÓÔÕÖŒÙÚÛÜY\"£\'§º¡àáâãäçèéêëìíîïñòóôõöœùúûüyßaº¿±";
            int AbicompStartIndex = 0xa0;

            Array.Copy(AbicompChars.ToCharArray(), 0, _chars, AbicompStartIndex, AbicompChars.Length);

        }

        public static byte Codificar(Char c)
        {
            for (int i = 0; i < _chars.Length; i++)
            {
                if (c == _chars[i])
                {
                    return (byte)i;
                }
            }

            throw new Exception();
        }

        public static byte[] Codificar(String str)
        {
            byte[] bytes = new byte[str.Length];

            for (int i = 0; i < str.Length; i++)
            {
                bytes[i] = Codificar(str[i]);
            }

            return bytes;
        }
    }
}
