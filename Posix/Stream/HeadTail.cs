using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Posix
{
    public class HeadTail
    {

        private static byte[][] Head(byte[] input, int lines = 10)
        {
            List<byte[]> result = new List<byte[]>();

            List<byte> currentLine = new List<byte>();
            int pos = 0;

            for (int n = 0; n < lines; n++)
            {
                do
                {
                    // Fall out of the loop if we're at the end of the array
                    if (pos >= input.Length)
                    {
                        break;
                    }
                    currentLine.Add(input[pos]);
                } while (input[pos++] != '\n');

                // Keep falling
                if (pos >= input.Length)
                {
                    break;
                }

                result.Add(currentLine.ToArray());
                currentLine.Clear();
            }

            return result.ToArray();
        }

        public static void Head(byte[] input, out byte[][] output, int lines = 10)
        {
            output = Head(input, lines);
        }

        public static void Head(byte[] input, out byte[] output, int lines = 10)
        {
            output = Head(input, lines).SelectMany(x => x).ToArray();
        }

        public static void TailLines(Stream input, Stream output, int lines, bool fromTop = false)
        {
            if (!input.CanRead)
            {
                throw new ArgumentException("Unable to read from input stream.");
            }

            if (!output.CanWrite)
            {
                throw new ArgumentException("Unable to read from output stream.");
            }

            // Dump the stream into the buffer for processing
            
            byte[] buffer = new byte[input.Length];
            input.Read(buffer, 0, (int)input.Length);

            // Take input stream, process, and dump to output
            byte[] line = TailLines(buffer, lines, fromTop).SelectMany(x => x).ToArray();
            output.Write(line, 0, line.Length);
        }

        private static byte[][] TailLines(byte[] input, int lines = 10, bool fromTop = false)
        {
            IEnumerable<byte[]> result;
            
            int index;
            if (fromTop)
            {
                List<byte[]> lastLines = new List<byte[]>();
                List<byte> currentLine = new List<byte>();
                index = 0;
                int linesSkipped = 0;
                bool skipping = true;

                // Zip through the buffer until we get past the given number of lines
                do
                {
                    if (input[index] == '\n')
                    {
                        linesSkipped++;
                        if (linesSkipped == lines)
                        {
                            skipping = false;
                        }
                    }
                    index++;

                    // Buffer doesn't have the required number of lines. Return an
                    // empty array.
                    if (index == input.Length)
                    {
                        lastLines.Add(new byte[0]);
                        skipping = false;
                    }
                } while (skipping);

                while (index < input.Length)
                {
                        currentLine.Add(input[index]);

                        if (input[index] == '\n')
                        {
                            lastLines.Add(currentLine.ToArray());
                            currentLine.Clear();
                        }
                        index++;
                }

                result = lastLines;
            }
            else
            {
                Stack<byte[]> lastLines = new Stack<byte[]>();
                Stack<byte> currentLine = new Stack<byte>();
                index = input.Length - 1;
                int linesAdded = 0;

                // Add the last character; makes it a lot easier, since it's
                // probably going to be a newline anyway
                currentLine.Push(input[index]);
                index--;

                while (linesAdded != lines && index >= 0)
                {
                    if (input[index] == '\n')
                    {
                        lastLines.Push(currentLine.ToArray());
                        currentLine.Clear();
                        linesAdded++;
                        if (linesAdded != lines)
                        {
                            currentLine.Push(input[index]);
                        }
                    }
                    else
                    {
                        currentLine.Push(input[index]);
                    }
                    index--;
                }

                // We've ended up slurping the whole file, put first line in.
                if (currentLine.Count != 0)
                {
                    lastLines.Push(currentLine.ToArray());
                }

                result = lastLines;
            }

            return result.ToArray();
        }

        public static void TailLines(byte[] input, out byte[][] output, int lines = 10, bool fromTop = false)
        {
            output = TailLines(input, lines, fromTop);
        }

        public static void TailLines(byte[] input, out byte[] output, int lines = 10, bool fromTop = false)
        {
            output = TailLines(input, lines, fromTop).SelectMany(x => x).ToArray();
        }

        public static void TailChars(Stream input, Stream output, int chars, bool fromTop = false)
        {
            if (!input.CanRead)
            {
                throw new ArgumentException("Unable to read from input stream.");
            }

            if (!output.CanWrite)
            {
                throw new ArgumentException("Unable to read from output stream.");
            }

            // Dump the stream into the buffer for processing
            byte[] buffer = new byte[input.Length];
            input.Read(buffer, 0, (int)input.Length);

            // Take input stream, process, and dump to output
            output.Write(TailChars(buffer, chars, fromTop), 0, chars);
            
        }

        public static void TailChars(byte[] input, out byte[] output, int chars, bool fromTop = false)
        {
            output = TailChars(input, chars, fromTop);
        }

        private static byte[] TailChars(byte[] input, int chars, bool fromTop = false)
        {
            int charOffset;
            byte[] result = new byte[chars];

            if (fromTop)
            {
                charOffset = chars;
            }
            else
            {
                charOffset = input.Length - chars;

                // Wanting to slurp from before the beginning of the buffer, so
                // just return a clone of the buffer
                if (charOffset <= 0)
                {
                    return (byte[])input.Clone();
                }
            }

            for (int n = charOffset; n < input.Length; n++)
            {
                result[n-charOffset] = input[n];
            }

            return result;
        }

        public static int Head(Stream input, Stream output, int lines = 10)
        {
            if (!input.CanRead)
            {
                throw new ArgumentException("Can't read from input stream.");
            }
            if (!output.CanWrite)
            {
                throw new ArgumentException("Can't write to output stream.");
            }
            
            List<byte> inputBuffer = new List<byte>();
            int byteRead = 0;
            int linesRead = 0;
            bool keepReading = true;
            while (keepReading)
            {
                byteRead = input.ReadByte();
                if (byteRead != -1)
                {
                    inputBuffer.Add((byte)byteRead);
                    if (byteRead == '\n')
                    {
                        linesRead++;
                        output.Write(inputBuffer.ToArray(), (int)output.Position, inputBuffer.Count);
                        inputBuffer.Clear();
                        if (linesRead == lines)
                        {
                            keepReading = false;
                        }
                    }
                }
                else
                {
                    output.Write(inputBuffer.ToArray(), (int)output.Position, inputBuffer.Count);
                    keepReading = false;
                }
            }

            return linesRead;
        }
    }
}
