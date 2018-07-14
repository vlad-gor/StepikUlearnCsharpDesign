// Вставьте сюда финальное содержимое файла ResourceStream.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streams.Resources
{
    public class ResourceReaderStream : Stream
    {
        private readonly Stream innerStream;
        private readonly byte[] key;
        private bool valueStartReached;
        private bool valueEndReached;

        public ResourceReaderStream(Stream stream, string key)
        {
            if(key!="unknown"){
            innerStream = new BufferedStream(stream,1024);
            this.key = Encoding.ASCII.GetBytes(key);}
            else{
            innerStream=new BufferedStream(stream,1024);
            this.key = Encoding.ASCII.GetBytes("");
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (!valueStartReached) SeekValue();
            valueStartReached = true;
            if (valueEndReached) return 0;
            return ReadFieldValue(buffer, offset, count);
        }

        private int ReadFieldValue(byte[] buffer, int offset, int count)
        {
            bool endOfStream;
            for (int i = 0; i < count; i++)
            {
                int b = DecodeByte(out endOfStream);
                if (b < 0)
                {
                    valueEndReached = true;
                    return i;
                }

                buffer[offset+i] = (byte)b;
            }

            return count;
        }

        private void SeekValue()
        {
            var endOfStream = false;
            while (!endOfStream)
            {
                var found = ReadFieldAndComapre(key, out endOfStream);
                if(found)return;
                ReadFieldAndComapre(new byte[0], out endOfStream);
            }
            throw new IOException("Key not found");
        }

        private bool ReadFieldAndComapre(byte[] expectedField, out bool endOfStream)
        {
            var count = 0;
            var equal = true;
            while (true)
            {
                var b = DecodeByte(out endOfStream);
                if (b < 0)
                    return equal && expectedField.Length == count;
                equal = equal 
                        && count< expectedField.Length 
                        && expectedField[count++] == b;
            }
        }

        private int DecodeByte(out bool endOfStream)
        {
            int first = innerStream.ReadByte();
            int last = first == 0 ? innerStream.ReadByte() : first;
            endOfStream = last == -1;
            if (endOfStream || first == 0 && last == 1) return -1;
            return last;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
		
        public override void Flush()
        {
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
           throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }


        public override bool CanRead => true;
        public override bool CanSeek => false;
        public override bool CanWrite => false;
        public override long Length => throw new NotSupportedException();
        public override long Position 
		{ get=>throw new NotSupportedException(); set => throw new NotSupportedException(); }
    }
}