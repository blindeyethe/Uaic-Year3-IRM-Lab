using UnityEngine;
using TheBlindEye.ObjectPoolSystem;

namespace IRM
{
    [CreateAssetMenu(fileName = "Bullet ObjectPool", menuName = "Object Pool/Bullet")]
    internal sealed class BulletObjectPoolData : ObjectPoolData<Bullet>
    { }
}