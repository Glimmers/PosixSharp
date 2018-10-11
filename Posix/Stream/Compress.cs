using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Posix
{
    class Compress
    {
        Stream _input, _output;
        int _codeBitSize;

        public Compress(Stream input, Stream output, int codeBitSize = 16)
        {
            _input = input;
            _output = output;
            _codeBitSize = codeBitSize;
        }

        public static byte[] CompressByteArray(byte[] input, int codeBitSize = 16)
        {
            List<UInt16> compressedArray = new List<UInt16>();


            byte[] compressedStream = new byte[compressedArray.Count * 2];

            for (int n = 0; n < compressedArray.Count; n++)
            {
                compressedStream[2 * n] = (byte)(compressedArray[n] >> 8);
                compressedStream[(2*n)+1] = (byte)(compressedArray[n] & 0xff);
            }

            return compressedStream;
        }

    }
}
