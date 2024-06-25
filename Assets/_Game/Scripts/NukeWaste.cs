using System.Collections;
using UnityEngine;

public class NukeWaste : MonoBehaviour, IItem
{
    public string itemName;
    public float attackInterval = 0.1f;
    public float attackRadius = 5f;
    public int damage = 1;
    public LayerMask enemyLayer;

    private bool isAttacking = false;

    public string ItemName => itemName;

    public void Activate()
    {
        this.gameObject.SetActive(true);
        if (!isAttacking)
        {
            StartCoroutine(AoEAttack());
        }

    }

    public void Deactivate()
    {
        StopCoroutine(AoEAttack());
        isAttacking = false;

        this.gameObject.SetActive(false);
    }

    private IEnumerator AoEAttack()
    {
        isAttacking = true;

        while (isAttacking)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRadius, enemyLayer);
            foreach (var hitCollider in hitColliders)
            {
                var enemy = hitCollider.GetComponent<Spider>();//Change for every type of enemy later
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }

            yield return new WaitForSeconds(attackInterval);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
