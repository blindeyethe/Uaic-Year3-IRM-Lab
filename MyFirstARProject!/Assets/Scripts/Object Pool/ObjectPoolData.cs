using UnityEngine;
using UnityEngine.Pool;

namespace TheBlindEye.ObjectPoolSystem
{
    public abstract class ObjectPoolData<T> : ScriptableObject where T : Component
    {
        [SerializeField] private T poolingObject;
        [SerializeField] private int defaultCapacity = 10;

        private ObjectPool<T> _objectPool;

        public T Get(Vector3 newPosition, Quaternion newRotation)
        {
            _objectPool ??= new ObjectPool<T>(OnPoolCreate, OnPoolGet, OnPoolRelease, OnPoolDestroy,
                false, defaultCapacity);
            
            var poolObject = _objectPool.Get();
            
            var transform = poolObject.transform;
            transform.position = newPosition; transform.rotation = newRotation;
            
            return poolObject;
        }
        
        public void Release(T  poolObject) =>
            _objectPool.Release(poolObject);

        private T OnPoolCreate()
        {
            var poolObject = Instantiate(poolingObject);
            DontDestroyOnLoad(poolObject);

            return poolObject;
        }
        
        private void OnPoolGet(T poolObject) => poolObject.gameObject.SetActive(true);
        private void OnPoolRelease(T poolObject) => poolObject.gameObject.SetActive(false);
        private void OnPoolDestroy(T poolObject) => Destroy(poolObject.gameObject);
    }
}
