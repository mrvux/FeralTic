using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.Core
{
    public class RefCounter<T> where T : class, IDisposable
    {
        private int refcount = 0;

        public T Element { get; protected set; }

        public RefCounter(T element)
        {
            this.Element = element;
            this.refcount = 1;
        }

        public void AddRef()
        {
            this.refcount++;
        }

        public int Release()
        {
            this.refcount--;
            if (this.refcount == 0)
            {
                this.Element.Dispose();
                this.Element = null;
            }
            return refcount;
        }
    }
}
