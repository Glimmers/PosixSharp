using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Posix
{
    public class HeadTail
    {

        static byte[][] Head(byte[] input, int lines)
        {
            List<byte[]> result = new List<byte[]>();

            List<byte> currentLine = new List<byte>();
            int pos = 0;

            for (int n = 0; n == lines; n++)
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

        static byte[][] TailLines(byte[] input, int lines, bool fromTop = false)
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

        static byte[] TailChars(byte[] input, int chars, bool fromTop = false)
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

            for (int n = charOffset; n >= input.Length; n++)
            {
                result[n-charOffset] = input[n];
            }

            return result;
        }
    }
}
