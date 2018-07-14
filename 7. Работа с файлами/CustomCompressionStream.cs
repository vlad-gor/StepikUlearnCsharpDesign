using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streams.Compression
{
    public class CustomCompressionStream : Stream
    {
        private MemoryStream baseStream;
        private bool forReading;
        private int size = 100;
        private int pointer = 0;
        private List<byte> data = new List<byte>();

        public CustomCompressionStream(MemoryStream baseStream, bool forReading)
        {
            this.baseStream = baseStream;
            this.forReading = forReading;
        }

        #region
        public override bool CanRead
        {
            get
            {
                return baseStream.CanRead;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return baseStream.CanSeek;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return baseStream.CanWrite;
            }
        }

        public override long Length
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override long Position
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override void Flush()
        {
            baseStream.Flush();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }
        #endregion

        public override void Write(byte[] buffer, int offset, int count)
        {
            var compressData = Compress(buffer.ToList());
            baseStream.Write(compressData, offset, compressData.Count());
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var isEnd = false;
            while (!isEnd)
            {
                byte[] buf = new byte[size];
                var cnt = baseStream.Read(buf, 0, size);
                foreach (var elem in Decompress(buf.Take(cnt).ToArray()))
                    data.Add(elem);
                if (cnt == 0)
                    isEnd = true;
            }
            var read = 0;
            if (pointer + count < data.Count)
                read = count;
            else
                read = data.Count - pointer;
            data.CopyTo(pointer, buffer, 0, read);
            pointer += read;
            
            return read;
        }

        public byte[] Compress(List<byte> toWrire)
        {
            var repeat = 1;
            var i = 0;
            var result = new List<byte>();
            var compressData = new byte[toWrire.Count];
            while (i < toWrire.Count)
            {
                while (i < toWrire.Count - 1 && toWrire[i] == toWrire[i + 1])
                {
                    i++;
                    repeat++;
                    if (repeat >= 255)
                        break;
                }
                result.Add(toWrire[i]);
                result.Add(Convert.ToByte(repeat));
                repeat = 1;
                i++;
            }
            return result.ToArray();
        }
        public byte[] Decompress(byte[] buffer)
        {
            var result = new List<byte>();
            var i = 0;
            var j = 0;
            while (i < buffer.Length - 1)
            {
                while (j < buffer[i + 1])
                {
                    result.Add(buffer[i]);
                    j++;
                }
                i += 2;
                j = 0;
            }
            return result.ToArray();
        }
    }
}