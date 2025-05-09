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
        public List<(T Node, Priority weight)> Elements = new List<(T, Priority)>();

        public void Enqueue(T item,Priority weight) 
        {
            Elements.Add((item, weight));

            int index = Elements.Count - 1;
            while (index > 0)
            {
                int Parent = (index - 1) / 2;
                if (Elements[Parent].weight.CompareTo(Elements[index].weight) <= 0) { break; }
                else 
                { 
                    var temp = Elements[Parent];
                    Elements[Parent] = Elements[index];
                    Elements[index] = temp;
                    index = Parent;
                }
            }
        }

        public T Dequeue()
        {
            if(Elements.Count == 0 )
                throw new Exception("Queue is empty");

            var result = Elements[0].Node;
            Elements[0] = Elements[Elements.Count - 1];
            Elements.RemoveAt(Elements.Count - 1);

            int index = 0;

            while (true) 
            {
                int Right_Child = 2 * index + 2;
                int Left_Child = 2 * index + 1;
                int Minimum = index;
                if (Right_Child < Elements.Count && Elements[Right_Child].weight.CompareTo(Elements[Minimum].weight) < 0)
                    Minimum = Right_Child;
                if (Left_Child < Elements.Count && Elements[Left_Child].weight.CompareTo(Elements[Minimum].weight) < 0)
                    Minimum = Left_Child;
                if (Minimum == index)
                    break;
                var temp = Elements[index];
                Elements[index] = Elements[Minimum];
                Elements[Minimum] = temp;
                index = Minimum;
            }

            return result;
        }
    }
}
