using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incapsulation.Weights
{

    public class Indexer
    {
        double[] arr;

        int start;

        public int Start
        {
            get { return start; }
            set
            {
                if (value < 0) throw new ArgumentException();
                if (value >= arr.Length) throw new ArgumentException();
                start = value;
            }
        }

        int length;

        public int Length
        {
            get { return length; }
            set
            {
                if (value < 0) throw new ArgumentException();
                if (value > arr.Length) throw new ArgumentException();
                length = value;
            }
        }

        public Indexer(double[] arr, int start, int length)
        {
            // this.arr = arr.Skip(start).Take(length).ToArray();
            this.arr = arr;
            Start = start;
            Length = length;
        }

        public double this[int index]
        {
            set
            {
                //index += Start;
                if (index < 0) throw new IndexOutOfRangeException();
                if (index >= 2) throw new IndexOutOfRangeException();

                arr[Start + index] = value;
            }

            get
            {
                //index += Start;

                if (index < 0) throw new IndexOutOfRangeException();
                if (index >= 2) throw new IndexOutOfRangeException();

                return arr[Start + index];
            }
        }
    }
}