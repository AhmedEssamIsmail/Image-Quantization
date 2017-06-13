using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageQuantization
{
    class BinaryHeap
    {
        Edge[] Data;
        int Count;
        public BinaryHeap (int Size) 
        {
            Data = new Edge[Size];
            Count = 0;
        }
        public void  Add (Edge Edge )
        {
            int position = Count;
            Count++;
            Data[position] = Edge;
            RePosition();
        }
        public void RePosition ()
        {
            int position = Count - 1;
            while (position > 0 && Data[position].Weight > Data[(position-1)/2].Weight)
            {
                Swap(position, (position - 1) / 2);
                position = (position - 1) / 2;
            }
        }
        public Edge GetMin ()
        {
            if (Count == 0)
                return null;
            Count--;
            return  Data[Count];
        }
        void Swap (int P1 , int P2)
        {
            Edge Temp = Data[P1];
            Data[P1] = Data[P2];
            Data[P2] = Temp;
        }
    }
}
