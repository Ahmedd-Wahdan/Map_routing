using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MAP_routing.model
{
    public class Priority_Queue<T, Priority> where Priority : IComparable<Priority>
    {
        public List<(T Node, Priority weight)> Yaya_Elements = new List<(T, Priority)>();

        public void Enqueue(T item,Priority weight) 
        {
            Yaya_Elements.Add((item, weight));

            int index = Yaya_Elements.Count - 1;
            while (index > 0)
            {
                int Parent = (index - 1) / 2;
                if (Yaya_Elements[Parent].weight.CompareTo(Yaya_Elements[index].weight) <= 0) { break; }
                else 
                { 
                    var temp = Yaya_Elements[Parent];
                    Yaya_Elements[Parent] = Yaya_Elements[index];
                    Yaya_Elements[index] = temp;
                    index = Parent;
                }
            }
        }

        public T Dequeue()
        {
            if(Yaya_Elements.Count == 0 )
                throw new Exception("Queue is empty");

            var result = Yaya_Elements[0].Node;
            Yaya_Elements[0] = Yaya_Elements[Yaya_Elements.Count - 1];
            Yaya_Elements.RemoveAt(Yaya_Elements.Count - 1);

            int index = 0;

            while (true) 
            {
                int Right_Child = 2 * index + 2;
                int Left_Child = 2 * index + 1;
                int Minimum = index;
                if (Right_Child < Yaya_Elements.Count && Yaya_Elements[Right_Child].weight.CompareTo(Yaya_Elements[Minimum].weight) < 0)
                    Minimum = Right_Child;
                if (Left_Child < Yaya_Elements.Count && Yaya_Elements[Left_Child].weight.CompareTo(Yaya_Elements[Minimum].weight) < 0)
                    Minimum = Left_Child;
                if (Minimum == index)
                    break;
                var temp = Yaya_Elements[index];
                Yaya_Elements[index] = Yaya_Elements[Minimum];
                Yaya_Elements[Minimum] = temp;
                index = Minimum;
            }

            return result;
        }
    }
}
