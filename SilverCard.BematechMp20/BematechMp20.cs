using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Ports;

namespace SilverCard.BematechMp20
{
    public class BematechMp20 : IDisposable
    {
        private MemoryStream _MemoryStream;

        public const Byte EscByte = 0x1b;
        public const int ColunasModoNormal = 48;
        public const int ColunasModoElite = 40;
        public const int ColunasModoComprimido = 60;        

        public BematechMp20()
        {
            _MemoryStream = new MemoryStream();
        }

        /// <summary>
        /// Escreve "ESC b" no buffer.
        /// </summary>
        /// <param name="b">byte</param>
        public void Comando(byte b)
        {
            EscreverByte(EscByte);
            EscreverByte(b);
        }

        /// <summary>
        /// Escreve "ESC b" no buffer.
        /// </summary>
        /// <param name="b">byte</param>
        public void Comando(Char b)
        {
            Comando(CodificadorAbicomp.Codificar(b));
        }

        /// <summary>
        /// Escreve "ESC b x" no buffer.
        /// </summary>
        /// <param name="b">byte</param>
        /// <param name="x">parâmetro</param>
        public void Comando(byte b, byte x)
        {
            EscreverByte(EscByte);
            EscreverByte(b);
            EscreverByte(x);
        }

        /// <summary>
        /// Escreve "ESC b x" no buffer.
        /// </summary>
        /// <param name="b">byte</param>
        /// <param name="x">parâmetro</param>
        public void Comando(Char b, byte x)
        {
            Comando(CodificadorAbicomp.Codificar(b), x);
        }

        /// <summary>
        /// Escreve "ESC b x" no buffer.
        /// </summary>
        /// <param name="b">byte</param>
        /// <param name="x">parâmetro</param>
        public void Comando(Char b, Char x)
        {
            Comando(CodificadorAbicomp.Codificar(b), CodificadorAbicomp.Codificar(x));
        }

        public void EscreverByte(Byte b)
        {            
            _MemoryStream.WriteByte(b);
        }

        public void EscreverBytes(Byte[] b)
        {
            _MemoryStream.Write(b, 0, b.Length);
        }

        /// <summary>
        /// Envia o buffer para uma port COM.
        /// </summary>
        /// <param name="nomePorta">Nome da porta COM.</param>
        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public void EnviarBuffer(String nomePorta)
        {
            using (var serialPort = new SerialPort(nomePorta, 9600))
            {
                serialPort.Open();
                _MemoryStream.CopyTo(serialPort.BaseStream);
                serialPort.Close();
            }
        }

        /// <summary>
        /// Converte o buffer interno para bytes.
        /// </summary>
        /// <returns></returns>
        public byte[] ParaBytes()
        {
            return _MemoryStream.ToArray();
        }

        /// <summary>
        /// Esvazia o buffer interno.
        /// </summary>
        public void EsvaziarBuffer()
        {
            _MemoryStream.SetLength(0);
            _MemoryStream.Position = 0;
        }

        /// <summary>
        /// Escreve uma linha no buffer.
        /// </summary>
        /// <param name="str"></param>
        public void EscreverLinha(String str)
        {
            if (str == null) return;
            EscreverBytes(CodificadorAbicomp.Codificar(str));
            AvancarLinha();
        }

        /// <summary>
        /// Escreve uma nova linha no buffer.
        /// </summary>
        public void AvancarLinha()
        {
            EscreverByte(0x0D);
            EscreverByte(0x0A);
        }
        
        /// <summary>
        /// Avança várias linhas no buffer.
        /// </summary>
        /// <param name="n">Número de linhas.</param>
        public void AvancarLinhas(int n)
        {
            for (int i = 0; i < n; i++)
            {
                AvancarLinha();
            }           
        }

        #region Comandos

        /// <summary>
        /// Reinicializa programação da minimpressora - ESC @
        /// </summary>
        public void Iniciar()
        {
            Comando('@');
        }

        /// <summary>
        /// Sinal sonoro - BEL
        /// </summary>
        public void SinalSonoro()
        {
            Comando(0x7);
        }

        /// <summary>
        /// Velocidade baixa
        /// </summary>
        public void VelocidadeBaixa()
        {
            Comando('s', 0x1);
        }

        /// <summary>
        /// Velocidade normal
        /// </summary>
        public void VelocidadeNormal()
        {
            Comando('s', '0');
        }

        public void SelecionaModoCondensado()
        {
            Comando(0x0F);
        }

        public void CancelaModoCondensado()
        {
            Comando(0x12);
        }

        public void SelecionaModoNormal()
        {
            Comando('M');
        }

        public void SelecionaModoElite()
        {
            Comando('P');
        }

        public void SelecionaModoExpandido()
        {
            Comando('W', '1');
        }

        public void CancelaModoExpandido()
        {
            Comando('W', '0');
        }

        public void UsarTabelaAbicomp()
        {
            Comando('t', '1');
        }

        public void SelecionaModoEnfatizado()
        {
            Comando('E');
        }

        public void CancelaModoEnfatizado()
        {
            Comando('F');
        }

        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if(_MemoryStream != null)
                    {
                        _MemoryStream.Dispose();
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~BematechMp20() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion


    }
}
