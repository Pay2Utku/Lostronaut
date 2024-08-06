using System.Collections;
using UnityEngine;

public class Spider : MonoBehaviour
{
    private Transform player;  // Reference to the player's transform
    [SerializeField] private float moveSpeed = 3f;  // Speed of the enemy's movement
    private float stopDistance = 0.1f;  // Distance at which the enemy stops moving towards the player
    [SerializeField] private float rotationSpeed = 3600f;  // Speed of rotation in degrees per second

    public int _health;
    public int _maxHealth = 100;

    private bool isAttacking = false;
    public float attackInterval = 0.1f;
    public float attackRadius = 2f;
    public int damage = 1;
    public LayerMask attackLayer;

    private void Start()
    {
        _health = _maxHealth;
        player = Player.instance.transform;
        Activate();
        StartCoroutine(AoEAttack());
    }
    void Update()
    {
        if (player == null) { return; }
        // Calculate the direction to the player
        Vector3 direction = player.position - transform.position;
        direction.y = 0;  // Keep movement in the horizontal plane

        // Calculate the distance to the player
        float distance = direction.magnitude;

        // Check if the enemy needs to move towards the player
        if (distance > stopDistance)
        {
            // Normalize the direction vector
            direction.Normalize();

            // Calculate the target angle based on the direction
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            // Get the current angle
            float currentAngle = transform.eulerAngles.y;

            // Smoothly rotate towards the target angle
            float newAngle = Mathf.LerpAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime / 360f);

            // Apply the new angle to the enemy
            transform.eulerAngles = new Vector3(0, newAngle, 0);

            // Move the enemy towards the player
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
    }
    public void Activate()
    {
        //this.gameObject.SetActive(true);
        if (!isAttacking)
        {
            StartCoroutine(AoEAttack());
        }

    }

    public void Deactivate()
    {
        StopCoroutine(AoEAttack());
        isAttacking = false;

        //this.gameObject.SetActive(false);
    }

    private IEnumerator AoEAttack()
    {
        isAttacking = true;
        while (isAttacking)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRadius, attackLayer);
            foreach (var hitCollider in hitColliders)
            {
                Player player = hitCollider.GetComponent<Player>();//Change for every type of enemy later
                if (player != null)
                {
                    player.TakeDamage(damage);
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

    public void TakeDamage(int damage)
    {
        _health -= damage;
        print(-damage);
        if (IsDead())
        {
            Die();
        }
    }
    private bool IsDead()
    {
        if (_health < 0)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    private void Die()
    {
        // Handle enemy death
        Deactivate();
        Destroy(gameObject);
    }
}
