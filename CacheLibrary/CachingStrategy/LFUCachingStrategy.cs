using CacheLibrary.Models;
using System.Collections.Generic;

namespace CacheLibrary.CachingStrategy
{
    internal class LFUCachingStrategy : ICachingStrategy
    {
        private int _bucketSize;
        private Dictionary<string, LFUCacheNode> _cacheNodeMap;
        private LinkedList<LFUFrequencyNode> _cacheFrequencyList;

        public LFUCachingStrategy(int bucketSize)
        {
            _bucketSize = bucketSize;
            _cacheNodeMap = new ();
            _cacheFrequencyList = new ();
        }

        public void Add(string key, string value)
        {
            /*
             1. Check if key already exists -> error
             2. if bucket is full - evict
             3. add node
                3.1 check if 0 freq node exists, if not create
                3.2 create cache node
                3.3 attach to freq node in LRU fashion i.e at the front
            */

            if(_cacheNodeMap.ContainsKey(key))
                throw new ArgumentException($"Cache Key - '{key}' already exists. Cache keys must be unique.");

            if(_cacheNodeMap.Count() == _bucketSize)
                EvictNode();

            AddNode(key, value);
        }

        private void AddNode(string key, string value)
        {
            LinkedListNode<LFUFrequencyNode>? frequencyLLNode;
            //lastNode should be zero as we will maintain the freq LL in ascending order, last pointing to 0 and first pointing to max frequency so far
           
            if(_cacheFrequencyList.Last != null)
                frequencyLLNode = _cacheFrequencyList.Last;
            else             
                frequencyLLNode = _cacheFrequencyList.AddFirst(new LFUFrequencyNode(0, new())); 
            

            var node = new LFUCacheNode(key, value, frequencyLLNode);

            AddToFrequencyNodeListByLRU(frequencyLLNode.Value.NodeList, node);

            //add to map for faster retrievals
            _cacheNodeMap.Add(key, node);
        }

        private void AddToFrequencyNodeListByLRU(LinkedList<LFUCacheNode>? list, LFUCacheNode node)
        {
            list?.AddFirst(node);
        }

        private void EvictNode()
        {
            //Get least freq node i.e first node (To do this we remove freq node if list is empty). This will make this O(1)
            var leastFreqNode = _cacheFrequencyList.First();
            var nodeList = leastFreqNode?.NodeList;

            if(leastFreqNode!= null && nodeList!= null && nodeList.Count()>0) 
            {
                var lruNode = nodeList.Last();

                //remove from map
                _cacheNodeMap.Remove(lruNode.Key);

                //remove from list
                nodeList.RemoveLast();

                //remove frequency node if empty
                RemoveFrequencyNodeIfEmpty(leastFreqNode);
            }
        }

        public string Get(string key)
        {
            /*
             1. Check if key exists if not -> error
             2. Check if next freq node exists; as they are sorted next freq node = freq+1, if not create one
             3. Update parent freq node
             4. Add node to new list
             5. Remove node from prev list
             6. Return value
            */
           if (_cacheNodeMap.TryGetValue(key, out var node))
           {
                UpdateNodeFrequencyLocation(node);
                return node.Value;
            }
           else throw new ArgumentException($"Cache key - '{key}' does not exists");
        }

        private void UpdateNodeFrequencyLocation(LFUCacheNode node)
        {
            if (node?.ParentFrequencyNode != null && node?.ParentFrequencyNode?.Value?.Frequency != null)
            {
                var frequencyNode = node.ParentFrequencyNode;
                var currentFrequency = frequencyNode.Value.Frequency;
                LinkedListNode<LFUFrequencyNode>? newFrequencyNode;

                if (frequencyNode?.Next?.Value?.Frequency == currentFrequency + 1)
                    newFrequencyNode = frequencyNode?.Next;

                else
                    newFrequencyNode = _cacheFrequencyList.AddAfter(node.ParentFrequencyNode, new LFUFrequencyNode(currentFrequency + 1, new()));


                if (newFrequencyNode != null)
                {
                    //change parent freq node
                    node.ParentFrequencyNode = newFrequencyNode;

                    //Add to Node list
                    AddToFrequencyNodeListByLRU(newFrequencyNode?.Value?.NodeList, node);

                    //Remove from old list
                    RemoveFromFrequencyNodeList(frequencyNode?.Value?.NodeList, node);

                    //Remove freq node if empty
                    RemoveFrequencyNodeIfEmpty(frequencyNode?.Value);
                   
                }

                else
                    throw new Exception("An error occured while getting value");

            }
            else
                throw new Exception("An error occured while getting value");
        }

        private void RemoveFrequencyNodeIfEmpty(LFUFrequencyNode? frequencyNode)
        {
            if (frequencyNode?.NodeList?.Count() == 0)
            {
                _cacheFrequencyList.Remove(frequencyNode);
            }
        }

        private void RemoveFromFrequencyNodeList(LinkedList<LFUCacheNode>? nodeList, LFUCacheNode node)
        {
           nodeList?.Remove(node);
        }

        public void Update(string key, string value)
        { /*
             1. Check if key exists if not -> error
             2. Check if next freq node exists; as they are sorted next freq node = freq+1, if not create one
             3. Update parent freq node
             4. Add node to new list
             5. Remove node from prev list
             6. Update value
            */
            if (_cacheNodeMap.TryGetValue(key, out var node))
            {
                UpdateNodeFrequencyLocation(node);
                node.Value = value;
            }
            else throw new ArgumentException($"Cache key - '{key}' does not exists");
        }
    }
}
