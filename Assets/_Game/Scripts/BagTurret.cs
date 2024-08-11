using System.Collections;
using UnityEngine;

public class BagTurret : MonoBehaviour, IItem
{

    public float AttackInterval = 0.1f;
    public float AttackRadius = 10f;
    public int Damage = 5;
    public LayerMask EnemyLayer;

    private Collider _nearestEnemy;

    public Transform TurningPointTransform;

    private bool _isAttacking = false;

    [SerializeField] private string _itemName;
    public string ItemName => _itemName;

    public void Activate()
    {
        this.gameObject.SetActive(true);
        if (!_isAttacking)
        {
            StartCoroutine(Shoot());
        }
    }

    public void Deactivate()
    {
        StopCoroutine(Shoot());
        _isAttacking = false;

        this.gameObject.SetActive(false);
    }
    private void Start()
    {
        //FindNearestEnemy();
    }
    private void Update()
    {
        FindNearestEnemy();
        AimAtNearestEnemy(_nearestEnemy);
    }

    private IEnumerator Shoot()
    {
        _isAttacking = true;

        while (_isAttacking)
        {
            if (_nearestEnemy != null)
            {
                var enemy = _nearestEnemy.GetComponent<IDamageable>(); // Change for every type of enemy later
                if (enemy != null)
                {
                    enemy.TakeDamage(Damage);
                }
            }

            yield return new WaitForSeconds(AttackInterval);
        }
    }
    private void FindNearestEnemy()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, AttackRadius, EnemyLayer);
        _nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (var hitCollider in hitColliders)
        {
            float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                _nearestEnemy = hitCollider;
            }
        }
    }
    private void AimAtNearestEnemy(Collider nearestEnemy)
    {
        if (nearestEnemy == null)
        {
            TurningPointTransform.rotation = Quaternion.LookRotation(transform.forward, Vector3.up);// Yakýnda düþman yoksa önüne dön
        }
        else
        {

            // Calculate direction to the nearest enemy
            Vector3 directionToEnemy = nearestEnemy.transform.position - TurningPointTransform.position;
            directionToEnemy.y = 0; // Ignore the vertical component

            // Calculate the target rotation
            Quaternion targetRotation = Quaternion.LookRotation(directionToEnemy, Vector3.up);

            // Apply the rotation to the turningPointTransform
            TurningPointTransform.rotation = targetRotation;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRadius);
    }
}