using System.Collections;
using UnityEngine;

public class Spider : MonoBehaviour, IDamageable
{
    private Transform _player;  // Reference to the player's transform
    [SerializeField] private float _moveSpeed = 3f;  // Speed of the enemy's movement
    private float _stopDistance = 0.1f;  // Distance at which the enemy stops moving towards the player
    [SerializeField] private float _rotationSpeed = 3600f;  // Speed of rotation in degrees per second

    public int Health;
    public int MaxHealth = 100;

    private bool _isAttacking = false;
    public float AttackInterval = 0.1f;
    public float AttackRadius = 2f;
    public int Damage = 1;
    public LayerMask AttackLayer;
    /*
    private void Start()
    {
        Health = MaxHealth;
        _player = Player.Instance.transform;
        Activate();
        StartCoroutine(AoEAttack());
    }*/
    private void OnEnable()
    {
        Health = MaxHealth;
        _player = Player.Instance.transform;
        Activate();
        StartCoroutine(AoEAttack());
    }
    private void OnDisable()
    {
        Deactivate();
    }

    void Update()
    {
        if (_player == null) { return; }
        // Calculate the direction to the player
        Vector3 direction = _player.position - transform.position;
        direction.y = 0;  // Keep movement in the horizontal plane

        // Calculate the distance to the player
        float distance = direction.magnitude;

        // Check if the enemy needs to move towards the player
        if (distance > _stopDistance)
        {
            // Normalize the direction vector
            direction.Normalize();

            // Calculate the target angle based on the direction
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            // Get the current angle
            float currentAngle = transform.eulerAngles.y;

            // Smoothly rotate towards the target angle
            float newAngle = Mathf.LerpAngle(currentAngle, targetAngle, _rotationSpeed * Time.deltaTime / 360f);

            // Apply the new angle to the enemy
            transform.eulerAngles = new Vector3(0, newAngle, 0);

            // Move the enemy towards the player
            transform.Translate(Vector3.forward * _moveSpeed * Time.deltaTime);
        }
    }
    public void Activate()
    {
        //this.gameObject.SetActive(true);
        if (!_isAttacking)
        {
            StartCoroutine(AoEAttack());
        }

    }

    public void Deactivate()
    {
        StopCoroutine(AoEAttack());
        _isAttacking = false;

        //this.gameObject.SetActive(false);
    }

    private IEnumerator AoEAttack()
    {
        _isAttacking = true;
        while (_isAttacking)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, AttackRadius, AttackLayer);
            foreach (var hitCollider in hitColliders)
            {
                Player player = hitCollider.GetComponent<Player>();//Change for every type of enemy later
                if (player != null)
                {
                    player.TakeDamage(Damage);
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

    public void TakeDamage(int damage)
    {

        Health -= damage;
        print("Enemy:" + (-damage));
        if (IsDead())
        {
            Die();
        }
    }
    private bool IsDead()
    {
        if (Health < 0)
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
        //Deactivate();
        //Destroy(gameObject);
        this.gameObject.SetActive(false);
    }


}
