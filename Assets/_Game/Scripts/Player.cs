using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private FloatingJoystick _joystick;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;

    public static Player instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else { Destroy(this); }
    }
    private void Update()
    {
        Vector3 moveDirection = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical);

        transform.Translate(moveDirection * _moveSpeed * Time.deltaTime, Space.World);

        // Rotate the player to face the movement direction
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
}
