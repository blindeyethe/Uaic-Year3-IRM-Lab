using UnityEngine;

namespace IRM
{
    public interface IAttacker
    {
        void DoDamage(Vector3 attackerPosition, Transform target);
    }
}