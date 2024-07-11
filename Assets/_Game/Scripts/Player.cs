using UnityEngine;

public class Player : MonoBehaviour
{
    public LayerMask enemyLayer;
    [SerializeField] private FloatingJoystick _joystick;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;

    [SerializeField] private float _faceRadius;
    [SerializeField] private bool _isFacingEnemy;

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
        _isFacingEnemy = true;
    }

    private void Update()
    {
        Vector3 moveDirection = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical);

        transform.Translate(moveDirection * _moveSpeed * Time.deltaTime, Space.World);

        // Check if the player is using the dual pistols
        if (IsUsingDualPistols())
        {
            FaceNearestEnemy();
        }
        else if (moveDirection != Vector3.zero)
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

    private bool IsUsingDualPistols()
    {
        // Implement your logic to check if the player is using dual pistols
        // This could involve checking the current weapon type, for example:
        // return currentWeapon is DualPistol;
        return _isFacingEnemy; // Replace this with your actual condition
    }

    private void FaceNearestEnemy()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _faceRadius, enemyLayer);
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
            Vector3 directionToEnemy = (nearestEnemy.transform.position - transform.position).normalized;
            float targetAngle = Mathf.Atan2(directionToEnemy.x, directionToEnemy.z) * Mathf.Rad2Deg;

            // Get the current angle
            float currentAngle = transform.eulerAngles.y;

            // Smoothly rotate towards the target angle
            float newAngle = Mathf.LerpAngle(currentAngle, targetAngle, _rotationSpeed * Time.deltaTime / 360f);

            // Apply the new angle to the player
            transform.eulerAngles = new Vector3(0, newAngle, 0);
        }
        else
        {
            _isFacingEnemy = false;
        }
    }

}
