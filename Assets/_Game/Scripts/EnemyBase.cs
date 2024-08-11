using UnityEngine;

abstract public class EnemyBase : MonoBehaviour, IDamageable
{
    abstract public void TakeDamage(int damage);
}
