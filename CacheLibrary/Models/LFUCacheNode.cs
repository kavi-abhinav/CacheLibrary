using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheLibrary.Models
{
    internal class LFUCacheNode : CacheNode
    {
        public LFUCacheNode(string key, string value, LinkedListNode<LFUFrequencyNode> parentFrequencyNode) : base(key, value)
        {
            ParentFrequencyNode = parentFrequencyNode;
        }

        internal LinkedListNode<LFUFrequencyNode> ParentFrequencyNode { get; set; }
    }
}

