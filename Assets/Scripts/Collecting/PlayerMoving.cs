using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
    [SerializeField] private float Speed = 5f;
    [SerializeField] private Rigidbody2D RigidBody;
    [SerializeField] private AnimationController AnimController;
    
    private float _inputX = 0;
    private float _inputY = 0;

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
            RigidBody.linearVelocity = Vector3.zero;
            ChangeAnimation(AnimState.Idle);
            return;
        }

        Vector2 moveDirection = new Vector2(inputX, inputY);
        RigidBody.linearVelocity = moveDirection.normalized * Speed;

        SetDirection(inputX, inputY);
    }

    private void SetDirection(float inputX, float inputY)
    {
        if (inputX < 0)
        {
            RigidBody.transform.localScale = new Vector3(-1, 1, 1);
            ChangeAnimation(AnimState.Side);
        }
        else if (inputX > 0)
        {
            RigidBody.transform.localScale = new Vector3(1, 1, 1);
            ChangeAnimation(AnimState.Side);
        }
        else if (inputY < 0)
        {
            ChangeAnimation(AnimState.Front);
        }
        else if (inputY > 0)
        {
            ChangeAnimation(AnimState.Back);
        }
        else
        {
            ChangeAnimation(AnimState.Idle);
        }
    }

    private void ChangeAnimation(AnimState state)
    {
        AnimController.SetState(state);
    }
}