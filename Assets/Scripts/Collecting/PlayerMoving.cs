using Cysharp.Threading.Tasks;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

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
    [SerializeField] private float OverlapRadius = 2f;
    [SerializeField] private Vector3 Offset;

    private float _inputX = 0;
    private float _inputY = 0;
    private bool _isSkillUsing = false;
    private bool _isAlive = true;
    private bool _canCollect = false;
    private Vector3 _playerDirection = Vector3.down;
    private float _playerHP = 100;
    private HPBar _hpBar;
    private DropItem _targetItem;

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

    public void UseBasicSkill(int atk)
    {
        if (!CheckSkillUsable())
        {
            return;
        }

        ChangeAnimation(AnimState.Attack);

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

        StartBasicSkill().Forget();
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

    public void UseProjectileSkill(ProjectileType type, int atk)
    {
        if (!CheckSkillUsable())
        {
            return;
        }

        GameObject gameObject = Instantiate(Prefab_ProjectileSkill, SkillRoot);

        SkillProjectile skillProjectile = gameObject.GetComponent<SkillProjectile>();
        skillProjectile.InitProjectile(type, _playerDirection, atk, OnMonsterCollide);
    }

    private void OnMonsterCollide(int instanceID, int damage)
    {
        Enemy enemy = CollectingManager.Inst.GetMonster(instanceID);

        enemy.TakeDamage(damage);
    }

    public void UseOverlapSkill(OverlapType type, int atk)
    {
        if (!CheckSkillUsable())
        {
            return;
        }

        Vector2 dir = _playerDirection.normalized;

        Vector3 spawnPosition = transform.position + new Vector3(dir.x * Offset.x, dir.y * Offset.x, 0);

        GameObject skillObj = Instantiate(Prefab_OverlapSkill, spawnPosition, Quaternion.identity, SkillRoot);

        SkillOverlap skillOverlap = skillObj.GetComponent<SkillOverlap>();
        skillOverlap.InitOverlap(type, _playerDirection, OverlapRadius, atk, OnMonsterCollide);
    }

    private bool CheckSkillUsable()
    {
        if (_isSkillUsing)
        {
            return false;
        }

        return true;
    }

    private async UniTaskVoid StartBasicSkill()
    {
        _isSkillUsing = true;

        await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: this.GetCancellationTokenOnDestroy());

        _isSkillUsing = false;
    }

    public void SetHPBar(HPBar hpBar)
    {
        _hpBar = hpBar;
    }

    public void TakeDamage(float atk)
    {
        _playerHP -= atk;
        
        if (_hpBar != null)
        {
            _hpBar.UpdateHPBar(_playerHP, 100);
        }

        if (_playerHP <= 0)
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        AnimController.SetState(AnimState.Dead);

        float delay = AnimController.GetAnimDelay();

        yield return new WaitForSeconds(delay);

        CollectingManager.Inst.ClearHPBar();
        GameManager.Inst.SetDay();
        UIManager.Inst.CloseHUD();
        UIManager.Inst.OpenLobbyUI();
        CollectingManager.Inst.DestroyPlayer();
    }

    private void SetCollectTarget()
    {
        _targetItem = null;
        _canCollect = false;

        float radius = 1f;
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach(Collider2D collider in hitColliders)
        {
            if (collider.CompareTag("Item"))
            {
                _targetItem = CollectingManager.Inst.GetTargetItem(collider.GetInstanceID());
                _canCollect = true;

                return;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            SetCollectTarget();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            DropItem item = CollectingManager.Inst.GetTargetItem(collision.GetInstanceID());

            if (item == _targetItem)
            {
                _canCollect = false;
                _targetItem = null;
            }
        }
    }
}