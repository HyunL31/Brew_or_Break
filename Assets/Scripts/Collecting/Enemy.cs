using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Threading;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Animator Anim;

    private int _instanceID;
    private string _monsterID;
    private Transform _parent;

    private Monster _monsterData;
    private HPBar _hpBar;
    private float _ATK;
    private float _HP;
    private float _maxHP;
    private float _CoolTime;
    private bool _isAlive = true;
    private bool _canAttack = false;
    private CancellationTokenSource _tokenSource;

    public void SetParent(Transform parent)
    {
        _parent = parent;
    }

    public void SetHPBar(HPBar hpBar)
    {
        _hpBar = hpBar;
    }

    public void InitMonster(int instanceID, string monsterID)
    {
        _instanceID = instanceID;
        _monsterID = monsterID;

        _monsterData = GameDataManager.Inst.GetMonsterData(monsterID);
        _ATK = _monsterData.ATK * GameManager.Inst.PlayerModel.Day;
        _HP = _monsterData.HP;
        _maxHP = _monsterData.HP;
        _CoolTime = _monsterData.CoolTime;
    }

    private async UniTaskVoid AttackRoutine(CancellationToken token)
    {
        while (_isAlive && _canAttack)
        {
            Attack();

            await UniTask.Delay(TimeSpan.FromSeconds(_CoolTime), cancellationToken: token);

            if(!_isAlive || !_canAttack)
            {
                break;
            }
        }
    }

    private void Attack()
    {
        Anim.SetTrigger("Attack");

        PlayerMoving player = CollectingManager.Inst.GetPlayer();
        player.TakeDamage(_ATK);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _canAttack = true;

            if (_tokenSource == null)
            {
                _tokenSource = new CancellationTokenSource();
                AttackRoutine(_tokenSource.Token).Forget();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _canAttack = false;

            CancelAttackRoutine();
        }
    }

    public void TakeDamage(float atk)
    {
        if (!_isAlive)
        {
            return;
        }

        Anim.SetTrigger("Damage");
        _HP -= atk;
        
        if (_hpBar != null)
        {
            _hpBar.UpdateHPBar(_HP, _maxHP);
        }

        if (_HP <= 0)
        {
            CancelAttackRoutine();
            Die().Forget();
        }
    }

    private async UniTaskVoid Die()
    {
        _isAlive = false;

        Anim.SetTrigger("Dead");

        float delay = Anim.GetCurrentAnimatorStateInfo(0).length;
        await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: this.GetCancellationTokenOnDestroy());

        CollectingManager.Inst.DropMonsterItem(_monsterData.DropItem, _parent).Forget();

        this.gameObject.SetActive(false);
    }

    private void CancelAttackRoutine()
    {
        if (_tokenSource != null)
        {
            _tokenSource.Cancel();
            _tokenSource.Dispose();
            _tokenSource = null;
        }
    }

    public int GetInstancedID()
    {
        return _instanceID;
    }
}
