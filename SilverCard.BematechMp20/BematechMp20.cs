﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilverCard.BematechMp20
{
    public class BematechMp20 : IDisposable
    {
        MemoryStream memoryStream;

        public const Byte EscByte = 0x1b;
        public const int ColunasModoNormal = 48;
        public const int ColunasModoElite = 40;
        public const int ColunasModoComprimido = 60;
        

        public BematechMp20()
        {
            memoryStream = new MemoryStream();
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
            memoryStream.WriteByte(b);
        }

        public void EscreverBytes(Byte[] b)
        {
            memoryStream.Write(b, 0, b.Length);
        }

        public void EnviarBuffer(String nomePorta)
        {
            using (var serialPort = new SerialPort(nomePorta, 9600))
            {
                serialPort.Open();
                var bytes = memoryStream.ToArray();
                serialPort.Write(bytes, 0, bytes.Length);
                serialPort.Close();
            }
        }

        public byte[] ParaBytes()
        {
            return memoryStream.ToArray();
        }

        public void EsvaziarBuffer()
        {
            memoryStream.SetLength(0);
        }

        public void EscreverLinha(String str)
        {
            if (str == null) return;

            EscreverBytes(CodificadorAbicomp.Codificar(str));
            AvancarLinha();
        }

        public void AvancarLinha()
        {
            EscreverByte(0x0D);
            EscreverByte(0x0A);
        }
        
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
                    if(memoryStream != null)
                    {
                        memoryStream.Dispose();
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