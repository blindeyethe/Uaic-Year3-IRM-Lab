using UnityEngine;

namespace TheBlindEye.ObjectPoolSystem
{
    public abstract class PoolObject<T> : MonoBehaviour
        where T : Component
    {
        public ObjectPoolData<T> Pool { get; set; }
    }
}