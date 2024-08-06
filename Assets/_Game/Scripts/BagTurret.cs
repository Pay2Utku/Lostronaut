using System.Collections;
using UnityEngine;

public class BagTurret : MonoBehaviour, IItem
{
    public string itemName;
    public float attackInterval = 0.1f;
    public float attackRadius = 10f;
    public int damage = 5;
    public LayerMask enemyLayer;

    private Collider nearestEnemy;

    public Transform turningPointTransform;

    private bool isAttacking = false;

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
    private void Start()
    {
        //FindNearestEnemy();
    }
    private void Update()
    {
        FindNearestEnemy();
        AimAtNearestEnemy(nearestEnemy);
    }

    private IEnumerator Shoot()
    {
        isAttacking = true;

        while (isAttacking)
        {
            if (nearestEnemy != null)
            {
                var enemy = nearestEnemy.GetComponent<Spider>(); // Change for every type of enemy later
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }

            yield return new WaitForSeconds(attackInterval);
        }
    }
    private void FindNearestEnemy()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRadius, enemyLayer);
        nearestEnemy = null;
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
    }
    private void AimAtNearestEnemy(Collider nearestEnemy)
    {
        if (nearestEnemy == null)
        {
            turningPointTransform.rotation = Quaternion.LookRotation(transform.forward, Vector3.up);// Yakýnda düþman yoksa önüne dön
        }
        else
        {

            // Calculate direction to the nearest enemy
            Vector3 directionToEnemy = nearestEnemy.transform.position - turningPointTransform.position;
            directionToEnemy.y = 0; // Ignore the vertical component

            // Calculate the target rotation
            Quaternion targetRotation = Quaternion.LookRotation(directionToEnemy, Vector3.up);

            // Apply the rotation to the turningPointTransform
            turningPointTransform.rotation = targetRotation;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}