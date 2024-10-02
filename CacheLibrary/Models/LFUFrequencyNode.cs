using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheLibrary.Models
{
    internal class LFUFrequencyNode
    {
        internal LFUFrequencyNode(int frequency, LinkedList<LFUCacheNode>? nodeList)
        {
            Frequency = frequency;
            NodeList = nodeList;
        }

        internal int Frequency { get; set; }

        internal LinkedList<LFUCacheNode>? NodeList { get; set; }
    }
}
