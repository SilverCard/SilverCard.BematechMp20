using System;
using System.Text;

namespace SilverCard.BematechMp20
{
    public static class CodificadorAbicomp
    {
        private static Char[] _Chars;

        /// <summary>
        /// Preenche o _Chars;
        /// </summary>
        static CodificadorAbicomp()
        {
            _Chars = new Char[256];

            byte[] bytes = new byte[0x7f + 1];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte)i;
            }
            Encoding.ASCII.GetChars(bytes, 0, bytes.Length, _Chars, 0);

            String AbicompChars = "\x20ÀÁÂÃÄÇÈÉÊËÌÍÎÏÑÒÓÔÕÖŒÙÚÛÜY\"£\'§º¡àáâãäçèéêëìíîïñòóôõöœùúûüyßaº¿±";
            int AbicompStartIndex = 0xa0;

            Array.Copy(AbicompChars.ToCharArray(), 0, _Chars, AbicompStartIndex, AbicompChars.Length);
        }

        /// <summary>
        /// Codifica um caractere para byte.
        /// </summary>
        public static byte Codificar(Char c)
        {
            for (int i = 0; i < _Chars.Length; i++)
            {
                if (c == _Chars[i])
                {
                    return (byte)i;
                }
            }            

            throw new InvalidOperationException(String.Format("O cractere {0} não pode ser codificado.", c));
        }

        /// <summary>
        /// Codifica uma string para bytes.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] Codificar(String str)
        {
            if (str == null) throw new ArgumentNullException("str");

            byte[] bytes = new byte[str.Length];

            for (int i = 0; i < str.Length; i++)
            {
                bytes[i] = Codificar(str[i]);
            }

            return bytes;
        }
    }
}
