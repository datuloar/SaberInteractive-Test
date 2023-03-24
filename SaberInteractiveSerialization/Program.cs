using SaberInteractive.Serialization;
using System;
using System.IO;

namespace SaberInteractive.Example
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var node1 = new ListNode { Data = "Node one" };
            var node2 = new ListNode { Data = "Node two" };
            var node3 = new ListNode { Data = "Node three" };

            node1.Next = node2;
            node2.Previous = node1;
            node2.Next = node3;
            node3.Previous = node2;

            node1.Random = node3;
            node2.Random = node1;
            node3.Random = node2;

            var list = new ListRandom(node1, node3);

            using var stream = new FileStream("SerializedContent.txt", FileMode.Create);
            list.Serialize(stream);
        }
    }
}
