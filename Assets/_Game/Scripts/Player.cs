using UnityEngine;

public class Player : MonoBehaviour
{
    public LayerMask enemyLayer;
    [SerializeField] private FloatingJoystick _joystick;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;

    [SerializeField] private float _faceRadius;

    [SerializeField] private int _maxHealth;
    [SerializeField] private float _health;

    public static Player instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else { Destroy(this); }
    }
    private void Start()
    {
        _health = _maxHealth;
    }

    private void Update()
    {
        Vector3 moveDirection = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical);

        transform.Translate(moveDirection * _moveSpeed * Time.deltaTime, Space.World);

        if (moveDirection != Vector3.zero)
        {
            // Calculate the target angle based on movement direction
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;

            // Get the current angle
            float currentAngle = transform.eulerAngles.y;

            // Smoothly rotate towards the target angle
            float newAngle = Mathf.LerpAngle(currentAngle, targetAngle, _rotationSpeed * Time.deltaTime / 360f);

            // Apply the new angle to the player
            transform.eulerAngles = new Vector3(0, newAngle, 0);
        }


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
        print("you died!");
        Destroy(gameObject);
    }
}
