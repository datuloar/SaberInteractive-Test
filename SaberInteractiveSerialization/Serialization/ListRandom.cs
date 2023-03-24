using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SaberInteractive.Serialization
{
    internal sealed class ListRandom
    {
        private const char ContentDivider = ':';
        private const int NullIndex = -1;

        private ListNode _head;
        private ListNode _tail;

        public ListRandom(ListNode head, ListNode tail)
        {
            _head = head;
            _tail = tail;
        }

        public void Serialize(FileStream fileStream)
        {
            var nodeToIndex = new Dictionary<ListNode, int>();
            int index = 0;

            ListNode currentNode = _head;

            while (currentNode != null)
            {
                nodeToIndex[currentNode] = index;
                index++;
                currentNode = currentNode.Next;
            }

            currentNode = _head;

            using var writer = new StreamWriter(fileStream, Encoding.UTF8);

            while (currentNode != null)
            {
                int randomIndex = currentNode.Random != null ? nodeToIndex[currentNode.Random] : NullIndex;
                writer.WriteLine($"{currentNode.Data}{ContentDivider}{randomIndex}");
                currentNode = currentNode.Next;
            }
        }

        public void Deserialize(FileStream fileStream)
        {
            var nodesWithRandIndices = new List<(ListNode Node, int RandIndex)>();

            using (var reader = new StreamReader(fileStream, Encoding.UTF8))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(ContentDivider);
                    var node = new ListNode { Data = parts[0] };

                    if (_head == null)
                    {
                        _head = node;
                    }
                    else
                    {
                        node.Previous = _tail;
                        _tail.Next = node;
                    }

                    _tail = node;
                    int randIndex = int.Parse(parts[1]);
                    nodesWithRandIndices.Add((node, randIndex));
                }
            }

            foreach (var (node, randIndex) in nodesWithRandIndices)
                node.Random = randIndex != NullIndex ? nodesWithRandIndices[randIndex].Node : null;
        }
    }
}
