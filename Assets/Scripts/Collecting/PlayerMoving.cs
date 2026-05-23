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
    [SerializeField] private GameObject Prefab_OverlapSkill;
    [SerializeField] private Transform SkillRoot;
    [SerializeField] private float OverlapRadius = 5f;
    [SerializeField] private Vector3 Offset;

    private float _inputX = 0;
    private float _inputY = 0;
    private bool _isSkillUsing = false;
    private Vector3 _playerDirection = Vector3.down;

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
        RigidBody.transform.eulerAngles = new Vector3(0, 0, 0);

        if (inputX < 0)
        {
            RigidBody.transform.eulerAngles = new Vector3(0, 180f, 0);
            ChangeAnimation(AnimState.Side);
            _playerDirection = Vector3.left;
        }
        else if (inputX > 0)
        {
            ChangeAnimation(AnimState.Side);
            _playerDirection = Vector3.right;
        }
        else if (inputY < 0)
        {
            ChangeAnimation(AnimState.Front);
            _playerDirection = Vector3.down;
        }
        else if (inputY > 0)
        {
            ChangeAnimation(AnimState.Back);
            _playerDirection = Vector3.up;
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

    public void UseProjectileSkill(ProjectileType type)
    {
        if (!CheckSkillUsable())
        {
            return;
        }

        GameObject gameObject = Instantiate(Prefab_ProjectileSkill, SkillRoot);

        SkillProjectile skillProjectile = gameObject.GetComponent<SkillProjectile>();
        skillProjectile.SetProjectileEffect(type, _playerDirection);

        if (type == ProjectileType.Fire || type == ProjectileType.Water)
        {
            skillProjectile.SetProjectileDirection(_playerDirection);
        }
    }

    public void UseOverlapSkill(OverlapType type)
    {
        if (!CheckSkillUsable())
        {
            return;
        }

        Vector2 dir = _playerDirection.normalized;

        Vector3 spawnPosition = transform.position + new Vector3(dir.x * Offset.x, dir.y * Offset.x, 0);

        //spawnPosition.x += Offset.y * -dir.y; // dir의 수직 벡터 반영 (필요 없다면 제외 가능)
        //spawnPosition.y += Offset.y * dir.x;

        GameObject skillObj = Instantiate(Prefab_OverlapSkill, spawnPosition, Quaternion.identity, SkillRoot);

        SkillOverlap skillOverlap = skillObj.GetComponent<SkillOverlap>();
        skillOverlap.SetOverlapEffect(type, _playerDirection);
        skillOverlap.InvokeOverlapSkill(OverlapRadius);
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