// Вставьте сюда финальное содержимое файла APIObject.cs
using System;
 
namespace Memory.API
{
    public class APIObject : IDisposable
    {
        private int i;
 
        public APIObject(int i)
        {
            this.i = i;
            MagicAPI.Allocate(i);
        }
 
        ~APIObject()
        {
            Dispose(true);
        }
 
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                if (MagicAPI.Contains(i))
                    MagicAPI.Free(i);
        }
 
        public void Dispose()
        {
            Dispose(true);
        }
    }
}