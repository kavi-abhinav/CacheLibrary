# CacheLibrary

## What is this repository for?
This repository is an .Net 8 class library implementing an in-memory non-distributed cache client that we can use in our project.

## Supported Caching Strategies and Configuration
Currently the cache client supports following strategies :
1. FIFO (First In First Out) :
    - The eviction logic is based on fifo which means cache key which was added first is removed first when bucket size is reached.
    - Bucket Size : An integer value that represents max no of keys cache can hold, after which it will start eviction based on FIFO
2. LRU (Least Recently used out) :
    - The eviction logic is based on LRU which means cache key which is least recently used is removed when bucket size is reached.
    - Bucket Size : An integer value that represents max no of keys cache can hold, after which it will start eviction based on LRU 
3. LFU (Least Frequenty used out) :
    - The eviction logic is based on LFU which means cache key which is least frequenty used is removed when bucket size is reached.
      In case of a tie i.e two keys with same frequency of use we evict based on LRU.
    - Bucket Size : An integer value that represents max no of keys cache can hold, after which it will start eviction based on LFU
    
## Version and Dependencies
-  Target Framework - .Net 8

