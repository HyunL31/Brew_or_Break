using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 사냥 콘텐츠 플레이어 컨트롤러
/// </summary>

public class PlayerMoving : MonoBehaviour
{
    [Header("이동")]
    [SerializeField] private float Speed = 5f;
    [SerializeField] private Rigidbody2D RigidBody;
    [SerializeField] private AnimationController AnimController;

    [Header("스킬")]
    [SerializeField] private GameObject Prefab_ProjectileSkill;
    [SerializeField] private GameObject Prefab_OverlapSkill;
    [SerializeField] private Transform SkillRoot;

    private float _inputX = 0;
    private float _inputY = 0;
    private bool _isSkillUsing = false;
    private bool _isAlive = true;
    private bool _canCollect = false;
    private Vector3 _playerDirection = Vector3.down;
    private float _playerHP = 100;
    private float _playerStamina = 100;
    private float _maxStamina = 100;
    private HPBar _hpBar;
    private DropItem _targetItem;

    private List<DropItem> _items = new List<DropItem>();

    private void Awake()
    {
        _playerStamina = _playerStamina * StoreManager.Inst.GetCluePoint();
        _maxStamina = _playerStamina;
    }

    private void Update()
    {
        _inputX = Input.GetAxisRaw("Horizontal");
        _inputY = Input.GetAxisRaw("Vertical");

        if (_canCollect && Input.GetKeyDown(KeyCode.E))
        {
            CollectingManager.Inst.CollectItem(_targetItem);

            SetCollectTarget();
        }
    }

    private void FixedUpdate()
    {
        if (_isAlive)
        {
            Move(_inputX, _inputY);
        }
    }

    // 이동 및 애니메이션
    private void Move(float inputX, float inputY)
    {
        if (!_isAlive)
        {
            return;
        }

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

    // 스킬 사용
    public void UseBasicSkill(int atk, float stamina)
    {
        if (!CheckSkillUsable(stamina))
        {
            return;
        }

        ChangeAnimation(AnimState.Attack);

        SetStamina(stamina);

        float radius = 1.5f;
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(SetBasicSkillRange(), radius);

        foreach (Collider2D col in hitColliders)
        {
            if (col.CompareTag("Monster"))
            {
                Enemy enemy = col.GetComponent<Enemy>();
                enemy.TakeDamage(atk);
            }
        }
    }

    private Vector3 SetBasicSkillRange()
    {
        float xPos = 0;
        float yPos = 0;

        if (_playerDirection == Vector3.up)
        {
            xPos = 0;
            yPos = 1f;
        }
        else if (_playerDirection == Vector3.down)
        {
            xPos = 0;
            yPos = -1f;
        }
        else if (_playerDirection == Vector3.left)
        {
            xPos = -1f;
            yPos = 0f;
        }
        else if (_playerDirection == Vector3.right)
        {
            xPos = 1;
            yPos = 0f;
        }

        Vector3 targetPos = transform.position + new Vector3(xPos, yPos, 0);

        return targetPos;
    }

    public void UseProjectileSkill(ProjectileType type, int atk, float stamina)
    {
        if (!CheckSkillUsable(stamina))
        {
            return;
        }

        GameObject gameObject = Instantiate(Prefab_ProjectileSkill, SkillRoot.position, Quaternion.identity, null);

        SkillProjectile skillProjectile = gameObject.GetComponent<SkillProjectile>();
        skillProjectile.InitProjectile(type, _playerDirection, atk, OnMonsterCollide);

        SetStamina(stamina);
    }

    private void OnMonsterCollide(int instanceID, int damage)
    {
        Enemy enemy = CollectingManager.Inst.GetMonster(instanceID);
        enemy.TakeDamage(damage);
    }

    public void UseOverlapSkill(OverlapType type, int atk, float stamina)
    {
        if (!CheckSkillUsable(stamina))
        {
            return;
        }

        Vector2 dir = _playerDirection.normalized;
        Vector3 spawnPosition = transform.position + new Vector3(dir.x * 1.5f, dir.y * 1.5f, 0);
        GameObject skillObj = Instantiate(Prefab_OverlapSkill, spawnPosition, Quaternion.identity, null);

        SkillOverlap skillOverlap = skillObj.GetComponent<SkillOverlap>();
        skillOverlap.InitOverlap(type, _playerDirection, atk, OnMonsterCollide);
        
        SetStamina(stamina);
    }

    private bool CheckSkillUsable(float stamina)
    {
        if (_isSkillUsing || _playerStamina < stamina || !_isAlive)
        {
            return false;
        }

        return true;
    }

    // 플레이어 상태 관리
    public void SetHPBar(HPBar hpBar)
    {
        _hpBar = hpBar;
    }

    private void SetStamina(float stamina)
    {
        _playerStamina -= stamina;
        CollectingManager.Inst.OnChangeStamina?.Invoke(_playerStamina, _maxStamina);
    }

    public void TakeDamage(float atk)
    {
        if (!_isAlive)
        {
            return;
        }

        _playerHP -= atk;
        
        if (_hpBar != null)
        {
            _hpBar.UpdateHPBar(_playerHP, 100);
        }

        if (_playerHP <= 0)
        {
            _isAlive = false;
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        _isAlive = false;

        CollectingManager.Inst.ClearPlayerBubble();

        AnimController.SetState(AnimState.Dead);

        float delay = AnimController.GetAnimDelay();

        yield return new WaitForSeconds(delay);

        CollectingManager.Inst.OnEndCollecting?.Invoke();

        GameManager.Inst.SetDay();
        UIManager.Inst.CloseHUD();
        UIManager.Inst.OpenLobbyUI();
    }

    // 아이템 줍기
    private void SetCollectTarget()
    {
        if (_items.Count == 0)
        {
            if (_canCollect)
            {
                CollectingManager.Inst.OnEnterItem?.Invoke(false);
            }

            _targetItem = null;
            _canCollect = false;

            return;
        }

        DropItem newTarget = _items[0];

        if (_targetItem != newTarget)
        {
            _targetItem = newTarget;
            _canCollect = true;
            CollectingManager.Inst.OnEnterItem?.Invoke(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            DropItem item = CollectingManager.Inst.GetTargetItem(collision.GetInstanceID());

            if (item != null && !_items.Contains(item))
            {
                _items.Add(item);
                SetCollectTarget();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            DropItem item = CollectingManager.Inst.GetTargetItem(collision.GetInstanceID());

            if (item != null && _items.Contains(item))
            {
                _items.Remove(item);
                SetCollectTarget();
            }
        }
    }
}