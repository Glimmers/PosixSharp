using System;
using System.IO;

namespace Posix
{
    public partial class StreamUtils
    {
        
        public static void Tee(Stream input, Stream output, Stream toFile)
        {
            if (!input.CanRead)
            {
                throw new ArgumentException("Can't read from input stream.");
            }
            if (!output.CanWrite)
            {
                throw new ArgumentException("Can't write to output stream.");
            }
            if (!toFile.CanWrite)
            {
                throw new ArgumentException("Can't write to file stream.");
            }

            bool canRead = true;
            int byteRead;
            byte toWrite;
            while (canRead)
            {
                byteRead = input.ReadByte();
                if (byteRead == -1)
                {
                    canRead = false;
                    toFile.Close();
                }
                else
                {
                    toWrite = (byte)byteRead;
                    output.WriteByte(toWrite);
                    toFile.WriteByte(toWrite);
                }
            }
        }
    }
}
