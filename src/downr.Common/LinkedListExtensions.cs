namespace downr.Common
{
    using System.Collections.Generic;

    public static class LinkedListExtensions
    {
        public static LinkedList<T> ToLinedList<T>(this IEnumerable<T> enumerable)
            where T: class
        {
            return new LinkedList<T>(enumerable);
        }

        public static IEnumerable<LinkedListNode<T>> Nodes<T>(this LinkedList<T> list)
            where T: class
        {
            var node = list.First;

            while (node != null)
            {
                yield return node;

                node = node.Next;
            }
        }
    }
}