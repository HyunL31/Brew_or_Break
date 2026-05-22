using System.Collections;
using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
    [Header("이동")]
    [SerializeField] private float Speed = 5f;
    [SerializeField] private Rigidbody2D RigidBody;
    [SerializeField] private AnimationController AnimController;

    [Header("스킬")]
    [SerializeField] private Collider2D Collider_BasicSkill;
    [SerializeField] private GameObject Prefab_ProjectileSkill;
    [SerializeField] private Transform SkillRoot;
    
    private float _inputX = 0;
    private float _inputY = 0;
    private bool _isSkillUsing = false;
    private Vector2 _playerDirection = Vector2.down;

    private void Awake()
    {
        Collider_BasicSkill.gameObject.SetActive(false);
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

    public void UseBasicSkill()
    {
        ChangeAnimation(AnimState.Attack);
        Collider_BasicSkill.gameObject.SetActive(true);
        StartCoroutine(StartBasicSkill());
    }

    public void UseProjectileSkill()
    {
        if (!CheckSkillUsable())
        {
            return;
        }

        GameObject gameObject = Instantiate(Prefab_ProjectileSkill, SkillRoot);

        SkillProjectile skillProjectile = gameObject.GetComponent<SkillProjectile>();
        skillProjectile.Shoot();
    }

    public void UseOverlapSkill()
    {
        if (!CheckSkillUsable())
        {
            return;
        }
    }

    private bool CheckSkillUsable()
    {
        if (_isSkillUsing)
        {
            return false;
        }

        return true;
    }

    private IEnumerator StartBasicSkill()
    {
        _isSkillUsing = true;
        yield return new WaitForSeconds(1f);
        Collider_BasicSkill.gameObject.SetActive(false);
        _isSkillUsing = false;
    }
}