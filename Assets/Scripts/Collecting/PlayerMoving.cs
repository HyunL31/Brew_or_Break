using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
    [SerializeField] private float Speed = 5f;

    private Rigidbody2D _rigidBody;
    private float _inputX = 0;
    private float _inputY = 0;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _inputX = Input.GetAxisRaw("Horizontal");
        _inputY = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        Move(_inputX, _inputY);
    }

    private void Move(float inputX, float inputY)
    {
        if (inputX == 0 && inputY == 0)
        {
            _rigidBody.linearVelocity = Vector3.zero;
            return;
        }

        _rigidBody.linearVelocity = new Vector2(inputX * Speed, inputY * Speed);

        SetDirection(inputX);
    }

    private void SetDirection(float inputX)
    {
        if (inputX < 0)
        {
            _rigidBody.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (inputX > 0)
        {
            _rigidBody.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}