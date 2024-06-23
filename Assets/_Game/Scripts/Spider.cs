using UnityEngine;

public class Spider : MonoBehaviour
{
    [SerializeField] private Transform player;  // Reference to the player's transform
    [SerializeField] private float moveSpeed = 3f;  // Speed of the enemy's movement
    private float stopDistance = 0.1f;  // Distance at which the enemy stops moving towards the player
    [SerializeField] private float rotationSpeed = 3600f;  // Speed of rotation in degrees per second

    void Update()
    {
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
}
