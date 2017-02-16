using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class Spawner : SingletonMonoBehaviour<Spawner>
{
    private const int DefaultCacheSize = 0;
    private Queue<RequestByPath> requestsByPath;
    private ResourcePrecacher resourcePrecacher;
    private SpawnerImpl spawnerImpl;

    private void Awake()
    {
        this.resourcePrecacher = new ResourcePrecacher();
        this.requestsByPath = new Queue<RequestByPath>(0x20);
        this.spawnerImpl = new SpawnerImpl(base.transform);
        base.Awake();
    }

    public void Despawn(GameObject objectToDespawn, bool sendsDespawn = true)
    {
        this.spawnerImpl.Despawn(objectToDespawn, sendsDespawn);
    }

    public bool HasCached()
    {
        if (!this.resourcePrecacher.HasCached())
        {
            return false;
        }
        while (0 < this.requestsByPath.Count)
        {
            RequestByPath path = this.requestsByPath.Dequeue();
            UnityEngine.Object resource = this.resourcePrecacher.GetResource(path.path);
            this.Precache(resource, path.cacheSize, path.overflowPolicy);
        }
        return this.spawnerImpl.HasCached();
    }

    public void Precache(string path)
    {
        this.Precache(path, 0);
    }

    public void Precache(UnityEngine.Object prefab)
    {
        this.Precache(prefab, 0);
    }

    public void Precache(string path, int cacheSize)
    {
        this.Precache(path, cacheSize, SpawnerOverflowPolicy.Instantiate);
    }

    public void Precache(UnityEngine.Object prefab, int cacheSize)
    {
        this.Precache(prefab, cacheSize, SpawnerOverflowPolicy.Instantiate);
    }

    public void Precache(string path, int cacheSize, SpawnerOverflowPolicy overflowPolicy)
    {
        if (!this.resourcePrecacher.HasElement(path))
        {
            this.resourcePrecacher.Precache(path);
            this.requestsByPath.Enqueue(new RequestByPath(path, cacheSize, overflowPolicy));
        }
    }

    public void Precache(UnityEngine.Object prefab, int cacheSize, SpawnerOverflowPolicy overflowPolicy)
    {
        this.spawnerImpl.Precache(prefab, cacheSize, overflowPolicy);
    }

    public GameObject Spawn(string path)
    {
        UnityEngine.Object resource = this.resourcePrecacher.GetResource(path);
        return this.Spawn(resource);
    }

    public GameObject Spawn(UnityEngine.Object prefab) => 
        this.spawnerImpl.Spawn(prefab, Vector3.zero, Quaternion.identity, false);

    public GameObject Spawn(string path, Vector3 position, Quaternion rotation)
    {
        UnityEngine.Object resource = this.resourcePrecacher.GetResource(path);
        return this.Spawn(resource, position, rotation);
    }

    public GameObject Spawn(UnityEngine.Object prefab, Vector3 position, Quaternion rotation) => 
        this.spawnerImpl.Spawn(prefab, position, rotation, true);

    [StructLayout(LayoutKind.Sequential)]
    private struct RequestByPath
    {
        public string path;
        public int cacheSize;
        public SpawnerOverflowPolicy overflowPolicy;
        public RequestByPath(string path, int cacheSize, SpawnerOverflowPolicy overflowPolicy)
        {
            this.path = path;
            this.cacheSize = cacheSize;
            this.overflowPolicy = overflowPolicy;
        }
    }

    private class ResourcePrecacher
    {
        private Dictionary<string, Element> elements = new Dictionary<string, Element>(0x20);

        public UnityEngine.Object GetResource(string path)
        {
            UnityEngine.Object asset = null;
            Element element;
            if (this.elements.TryGetValue(path, out element))
            {
                asset = element.asset;
            }
            if (asset == null)
            {
                asset = Resources.Load(path);
            }
            return asset;
        }

        public bool HasCached()
        {
            foreach (Element element in this.elements.Values)
            {
                if (element.request != null)
                {
                    if (element.request.isDone)
                    {
                        element.asset = element.request.asset;
                        element.request = null;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public bool HasElement(string path) => 
            this.elements.ContainsKey(path);

        public bool HasElements() => 
            (0 < this.elements.Count);

        private void OnDestroy()
        {
            this.UnloadAndClear();
        }

        public void Precache(string path)
        {
            if (!this.HasElement(path))
            {
                Element element = new Element {
                    asset = Resources.Load(path)
                };
                this.elements.Add(path, element);
            }
        }

        public void UnloadAndClear()
        {
            foreach (Element element in this.elements.Values)
            {
                if (element.asset != null)
                {
                    Resources.UnloadAsset(element.asset);
                }
            }
            this.elements.Clear();
        }

        public ICollection<string> Paths =>
            this.elements.Keys;

        private class Element
        {
            public UnityEngine.Object asset;
            public ResourceRequest request;
        }
    }

    private class SpawnerImpl
    {
        [CompilerGenerated]
        private static Func<Transform, GameObject> <>f__am$cache7;
        [CompilerGenerated]
        private static Predicate<GameObject> <>f__am$cache8;
        private const int CacheCountPerFrame = 0x20;
        private Dictionary<UnityEngine.Object, ObjectCache> caches;
        private HashSet<GameObject> destroyedUnmanagedObjects;
        private const float GCInterval = 59f;
        private const int InitialCapacityOfCaches = 0x20;
        private float lastGCAt;
        private Dictionary<GameObject, bool> managedObjects;
        private Transform parent;
        private static Queue<Transform> q = new Queue<Transform>();
        private Dictionary<UnityEngine.Object, int> serialNumbers;

        public SpawnerImpl(Transform parent)
        {
            this.parent = parent;
            this.caches = new Dictionary<UnityEngine.Object, ObjectCache>(0x20);
            this.managedObjects = new Dictionary<GameObject, bool>(0x80);
            this.destroyedUnmanagedObjects = new HashSet<GameObject>();
            this.serialNumbers = new Dictionary<UnityEngine.Object, int>(0x20);
        }

        private static List<Transform> BreadthFirstSearch(Transform root)
        {
            List<Transform> list = new List<Transform>();
            q.Enqueue(root);
            while (0 < q.Count)
            {
                Transform item = q.Dequeue();
                list.Add(item);
                for (int i = 0; i < item.childCount; i++)
                {
                    q.Enqueue(item.GetChild(i));
                }
            }
            q.Clear();
            return list;
        }

        private void DeactivateCachedObject(GameObject obj)
        {
            obj.SetActive(false);
            this.managedObjects[obj] = false;
            obj.transform.parent = this.parent;
        }

        public void Despawn(GameObject objectToDespawn, bool sendsDespawn)
        {
            if (this.IsAlive(objectToDespawn))
            {
                if (sendsDespawn)
                {
                    objectToDespawn.BroadcastMessage("OnDespawn", SendMessageOptions.DontRequireReceiver);
                }
                if (<>f__am$cache7 == null)
                {
                    <>f__am$cache7 = x => x.gameObject;
                }
                IEnumerator<GameObject> enumerator = (from x in BreadthFirstSearch(objectToDespawn.transform).Skip<Transform>(1).Select<Transform, GameObject>(<>f__am$cache7)
                    where this.managedObjects.ContainsKey(x)
                    select x).Reverse<GameObject>().GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        GameObject current = enumerator.Current;
                        this.DeactivateCachedObject(current);
                    }
                }
                finally
                {
                    if (enumerator == null)
                    {
                    }
                    enumerator.Dispose();
                }
                if (this.managedObjects.ContainsKey(objectToDespawn))
                {
                    this.DeactivateCachedObject(objectToDespawn);
                }
                else
                {
                    this.DestroyUnmanagedObject(objectToDespawn);
                }
            }
        }

        private void DestroyUnmanagedObject(GameObject obj)
        {
            UnityEngine.Object.Destroy(obj);
            this.destroyedUnmanagedObjects.Add(obj);
            float unscaledTime = Time.unscaledTime;
            if (59f < (unscaledTime - this.lastGCAt))
            {
                this.lastGCAt = unscaledTime;
                this.GC();
            }
        }

        private void GC()
        {
            if (<>f__am$cache8 == null)
            {
                <>f__am$cache8 = x => x == null;
            }
            this.destroyedUnmanagedObjects.RemoveWhere(<>f__am$cache8);
        }

        public bool HasCached()
        {
            int num = 0;
            foreach (ObjectCache cache in this.caches.Values)
            {
                if (!cache.IsInitialized)
                {
                    cache.Initialize();
                    num += cache.CacheSize;
                }
                if (0x20 <= num)
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsAlive(GameObject obj)
        {
            bool flag;
            if (obj == null)
            {
                return false;
            }
            if (this.managedObjects.TryGetValue(obj, out flag))
            {
                return flag;
            }
            return !this.destroyedUnmanagedObjects.Contains(obj);
        }

        private void Number(UnityEngine.Object prefab, GameObject obj)
        {
            int num;
            this.serialNumbers.TryGetValue(prefab, out num);
            this.serialNumbers[prefab] = ++num;
            obj.name = $"{prefab.name}({num})";
        }

        public void Precache(UnityEngine.Object prefab, int cacheSize, SpawnerOverflowPolicy overflowPolicy)
        {
            if (!this.caches.ContainsKey(prefab))
            {
                this.caches[prefab] = new ObjectCache(this, prefab, cacheSize, overflowPolicy);
            }
        }

        public GameObject Spawn(UnityEngine.Object prefab, Vector3 position, Quaternion rotation, bool overridesPositionAndRotation)
        {
            GameObject nextObjectInCache = null;
            ObjectCache cache;
            if (this.caches.TryGetValue(prefab, out cache))
            {
                nextObjectInCache = cache.GetNextObjectInCache();
            }
            if (nextObjectInCache == null)
            {
                if (overridesPositionAndRotation)
                {
                    nextObjectInCache = (GameObject) UnityEngine.Object.Instantiate(prefab, position, rotation);
                }
                else
                {
                    nextObjectInCache = (GameObject) UnityEngine.Object.Instantiate(prefab);
                }
                this.Number(prefab, nextObjectInCache);
                nextObjectInCache.SetActive(true);
                nextObjectInCache.BroadcastMessage("OnSpawn", SendMessageOptions.DontRequireReceiver);
                return nextObjectInCache;
            }
            if (overridesPositionAndRotation)
            {
                nextObjectInCache.transform.position = position;
                nextObjectInCache.transform.rotation = rotation;
            }
            this.managedObjects[nextObjectInCache] = true;
            nextObjectInCache.SetActive(true);
            nextObjectInCache.BroadcastMessage("OnSpawn", SendMessageOptions.DontRequireReceiver);
            return nextObjectInCache;
        }

        private class ObjectCache
        {
            private int cacheIndex;
            private int cacheSize;
            private Vector3 defaultLocalPosition;
            private Quaternion defaultLocalRotation;
            private Vector3 defaultLocalScale;
            private GameObject[] objects;
            private SpawnerOverflowPolicy overflowPolicy;
            private UnityEngine.Object prefab;
            private Spawner.SpawnerImpl spawnerImpl;

            public ObjectCache(Spawner.SpawnerImpl spawnerImpl, UnityEngine.Object prefab, int cacheSize, SpawnerOverflowPolicy overflowPolicy)
            {
                this.spawnerImpl = spawnerImpl;
                this.prefab = prefab;
                this.cacheSize = cacheSize;
                this.overflowPolicy = overflowPolicy;
            }

            public GameObject GetNextObjectInCache()
            {
                if (!this.IsInitialized || (this.cacheSize <= 0))
                {
                    return null;
                }
                GameObject objectToDespawn = null;
                for (int i = 0; i < this.cacheSize; i++)
                {
                    objectToDespawn = this.objects[this.cacheIndex];
                    if (!this.spawnerImpl.managedObjects[objectToDespawn])
                    {
                        break;
                    }
                    this.cacheIndex = (this.cacheIndex + 1) % this.cacheSize;
                }
                if (this.spawnerImpl.managedObjects[objectToDespawn])
                {
                    if (this.overflowPolicy != SpawnerOverflowPolicy.Recycle)
                    {
                        return null;
                    }
                    this.spawnerImpl.Despawn(objectToDespawn, true);
                }
                this.cacheIndex = (this.cacheIndex + 1) % this.cacheSize;
                Transform transform = objectToDespawn.transform;
                transform.parent = null;
                transform.localPosition = this.defaultLocalPosition;
                transform.localRotation = this.defaultLocalRotation;
                transform.localScale = this.defaultLocalScale;
                return objectToDespawn;
            }

            public void Initialize()
            {
                this.objects = new GameObject[this.cacheSize];
                for (int i = 0; i < this.cacheSize; i++)
                {
                    GameObject obj2 = this.objects[i] = (GameObject) UnityEngine.Object.Instantiate(this.prefab);
                    if (i == 0)
                    {
                        Transform transform = obj2.transform;
                        this.defaultLocalPosition = transform.localPosition;
                        this.defaultLocalRotation = transform.localRotation;
                        this.defaultLocalScale = transform.localScale;
                    }
                    this.spawnerImpl.Number(this.prefab, obj2);
                    this.spawnerImpl.DeactivateCachedObject(obj2);
                }
            }

            public int CacheSize =>
                this.cacheSize;

            public bool IsInitialized =>
                (this.objects != null);
        }
    }
}

