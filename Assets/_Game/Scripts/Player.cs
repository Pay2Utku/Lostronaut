using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private FloatingJoystick _joystick;

    [SerializeField] private float _moveSpeed;
    private void Update()
    {
        Vector3 moveDirection = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical);
        transform.Translate(moveDirection * _moveSpeed * Time.deltaTime);
    }
}
