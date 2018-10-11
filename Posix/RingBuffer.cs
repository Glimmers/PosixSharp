using System;
using System.Collections.Generic;

namespace Posix
{
    public class RingBuffer<T>
    {
        public delegate void FlowHandler(object sender, EventArgs e);

        public enum OverflowBehavior { DropNew, OverwriteFirst, OverwriteLast, Throw };
        private int _writePos = 0;
        private int _readPos = 0;
        private int _highWater = 0;
        private int _lowWater = 0;
        private OverflowBehavior _overflow = OverflowBehavior.OverwriteFirst;
        private bool _filling;
        private bool _useEvents;
        private int _currentUtilization;
        private T[] _buffer;

        public RingBuffer(int size, OverflowBehavior onOverflow)
        {
            if (size <= 0)
            {
                throw new ArgumentOutOfRangeException("Buffer needs to contain at least one member");
            }
            _buffer = new T[size];
            _filling = false;
            _useEvents = false;
            _overflow = onOverflow;
        }

        public RingBuffer(int size, int highWater, int lowWater, OverflowBehavior onOverflow)
        {
            if (size <= 0)
            {
                throw new ArgumentOutOfRangeException("Buffer needs to contain at least one member");
            }
            _buffer = new T[size];

            if (highWater > size)
            {
                throw new ArgumentOutOfRangeException("High water point for buffer cannot be greater than buffer size.");
            }

            if (highWater < 0 || lowWater < 0)
            {
                throw new ArgumentOutOfRangeException("Buffer alert points cannot be less than zero.");
            }

            if (lowWater >= highWater)
            {
                throw new ArgumentOutOfRangeException("Low water point for buffer cannot be greater than or equal to the high water point.");
            }

            _highWater = highWater;
            _lowWater = lowWater;
            _filling = true;
            _overflow = onOverflow;
        }

        public OverflowBehavior WhenOverflowing
        {
            get
            {
                return _overflow;
            }
            set
            {
                _overflow = value;
            }
        }

        public event FlowHandler Overflow;

        public event FlowHandler Underflow;

        public event FlowHandler HighWater;

        public event FlowHandler LowWater;

        protected virtual void OnOverflow(EventArgs e)
        {
            if (Overflow != null && _useEvents)
            {
                Overflow(this, e);
            }
        }

        protected virtual void OnUnderflow(EventArgs e)
        {
            if (Underflow != null && _useEvents)
            {
                Underflow(this, e);
            }
        }

        protected virtual void OnHighWater(EventArgs e)
        {
            if (HighWater != null && _useEvents)
            {
                HighWater(this, e);
            }
        }

        protected virtual void OnLowWater(EventArgs e)
        {
            if (LowWater != null && _useEvents)
            {
                LowWater(this, e);
            }
        }

        public void Flush()
        {
            _buffer = new T[_buffer.Length];
            _readPos = 0;
            _writePos = 0;
            if (_useEvents)
            {
                _filling = true;
            }
        }

        public int Count
        {
            get
            {
                // For purposes of indexing, the buffer only has cells equal to the
                // number of currently used items
                return _currentUtilization;
            }
        }

        public T this[int i]
        {
            get
            {
                // For purposes of indexing, the buffer only has the number of cells
                // between the read pointer and the write pointer.
                if (i >= _currentUtilization || (i < 0))
                {
                    throw new IndexOutOfRangeException();
                }
                return _buffer[(_readPos + i) % _buffer.Length];
            }
        }

        internal int ReadPosition
        {
            get
            {
                return _readPos;
            }
        }

        public bool MoveNext()
        {
            bool advanced;
            if (_currentUtilization == 0)
            {
                OnUnderflow(new EventArgs());
                advanced = false;
            }
            else
            {
                advanced = true;
                _readPos = (_readPos + 1) % _buffer.Length;
                _currentUtilization--;
                if (_currentUtilization == _lowWater && _filling == false)
                {
                    OnLowWater(new EventArgs());
                }
            }
            return advanced;
        }

        public T Peek()
        {
            return this[0];
        }

        public T Read()
        {
            T retVal = this[0];
            MoveNext();
            return retVal;
        }

        public bool Write(T toWrite)
        {
            bool written = true;

            if (_currentUtilization == _buffer.Length)
            {
                OnOverflow(new EventArgs());
                switch (_overflow)
                {
                    case OverflowBehavior.DropNew:
                        written = false;
                        break;
                    case OverflowBehavior.OverwriteFirst:
                        _buffer[_writePos] = toWrite;
                        _writePos = (_writePos + 1) % _buffer.Length;
                        _readPos = (_readPos + 1) % _buffer.Length;
                        break;
                    case OverflowBehavior.OverwriteLast:
                        if (_writePos != 0)
                        {
                            _buffer[_writePos - 1] = toWrite;
                        }
                        else
                        {
                            _buffer[_buffer.Length - 1] = toWrite;
                        }
                        break;
                    case OverflowBehavior.Throw:
                        throw new OverflowException();
                }
            }
            else if (_currentUtilization == _highWater)
            {
                OnHighWater(new EventArgs());
                _buffer[_writePos] = toWrite;
                _writePos = (_writePos + 1) % _buffer.Length;
                _currentUtilization++;
            }
            else
            {
                _buffer[_writePos] = toWrite;
                _writePos = (_writePos + 1) % _buffer.Length;
                _currentUtilization++;
                if (_filling && (_currentUtilization >= _lowWater))
                {
                    _filling = false;
                }
            }

            return written;
        }
    }


}