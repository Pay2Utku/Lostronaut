using System.Collections;
using UnityEngine;

public class DualPistol : MonoBehaviour, IItem
{
    public string itemName;
    public float attackInterval = 0.1f;
    public float attackRadius = 10f;
    public int damage = 5;
    public LayerMask enemyLayer;

    public Transform leftBarrel;
    public Transform rightBarrel;

    private bool isAttacking = false;
    private bool useLeftBarrel = true;

    public string ItemName => itemName;

    public void Activate()
    {
        this.gameObject.SetActive(true);
        if (!isAttacking)
        {
            StartCoroutine(Shoot());
        }
    }

    public void Deactivate()
    {
        StopCoroutine(Shoot());
        isAttacking = false;

        this.gameObject.SetActive(false);
    }

    private IEnumerator Shoot()
    {
        isAttacking = true;

        while (isAttacking)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRadius, enemyLayer);
            Collider nearestEnemy = null;
            float shortestDistance = Mathf.Infinity;

            foreach (var hitCollider in hitColliders)
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    nearestEnemy = hitCollider;
                }
            }

            if (nearestEnemy != null)
            {
                var enemy = nearestEnemy.GetComponent<Spider>(); // Change for every type of enemy later
                if (enemy != null)
                {
                    Transform shootFrom = useLeftBarrel ? leftBarrel : rightBarrel;
                    enemy.TakeDamage(damage);
                    // You can add effects like muzzle flash or bullet instantiation here
                    useLeftBarrel = !useLeftBarrel;
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