using System.Collections;
using UnityEngine;

public class NukeWaste : MonoBehaviour, IItem
{
    public float AttackInterval = 0.1f;
    public float AttackRadius = 5f;
    public int Damage = 1;
    public LayerMask EnemyLayer;

    private bool _isAttacking = false;

    private string _itemName;
    public string ItemName => _itemName;

    public void Activate()
    {
        this.gameObject.SetActive(true);
        if (!_isAttacking)
        {
            StartCoroutine(AoEAttack());
        }

    }

    public void Deactivate()
    {
        StopCoroutine(AoEAttack());
        _isAttacking = false;

        this.gameObject.SetActive(false);
    }

    private IEnumerator AoEAttack()
    {
        _isAttacking = true;

        while (_isAttacking)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, AttackRadius, EnemyLayer);
            foreach (var hitCollider in hitColliders)
            {
                var enemy = hitCollider.GetComponent<Spider>();//Change for every type of enemy later
                if (enemy != null)
                {
                    enemy.TakeDamage(Damage);
                }
            }

            yield return new WaitForSeconds(AttackInterval);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRadius);
    }
}
