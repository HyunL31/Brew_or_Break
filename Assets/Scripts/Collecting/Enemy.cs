using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Threading;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Animator Anim;
    [SerializeField] private float Speed = 2f;
    [SerializeField] private float MoveRange = 5f;

    private int _instanceID;

    private Transform _parent;

    private Vector2 _startPos;
    private Vector2 _targetPos;
    private Monster _monsterData;
    private HPBar _hpBar;
    private float _ATK;
    private float _HP;
    private float _maxHP;
    private float _CoolTime;
    private bool _isAlive = true;
    private bool _canAttack = false;
    private bool _isMoving = false;
    private CancellationTokenSource _tokenSource;
    private CancellationTokenSource _moveToken;

    private void Awake()
    {
        _startPos = transform.position;
    }

    private void Start()
    {
        if (_isAlive && !_canAttack)
        {
            MoveRoutine().Forget();
        }
    }

    private void Update()
    {
        Move();
    }

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

    private void Move()
    {
        Anim.SetBool("Move", _isMoving);

        if (!_isMoving || _canAttack || !_isAlive)
        {
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, _targetPos, Speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, _targetPos) < 0.05f)
        {
            _isMoving = false;

            MoveRoutine().Forget();
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            _isMoving = false;

            SetMoveTargetPos();
            _isMoving = true;
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

    private async UniTask Die()
    {
        _isAlive = false;

        Anim.SetTrigger("Dead");

        float delay = Anim.GetCurrentAnimatorStateInfo(0).length;
        await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: this.GetCancellationTokenOnDestroy());

        CollectingManager.Inst.DropMonsterItem(_monsterData.DropItem, _parent).Forget();

        this.gameObject.SetActive(false);
    }

    private void SetMoveTargetPos()
    {
        float xPos = UnityEngine.Random.Range(_startPos.x - MoveRange, _startPos.x + MoveRange);
        float yPos = UnityEngine.Random.Range(_startPos.y - MoveRange, _startPos.y + MoveRange);

        _targetPos = new Vector2(xPos, yPos);
    }

    private async UniTaskVoid MoveRoutine()
    {
        CancelMoveRoutine();
        _moveToken = new CancellationTokenSource();

        float waitTime = UnityEngine.Random.Range(1f, 3f);

        await UniTask.Delay(TimeSpan.FromSeconds(waitTime), cancellationToken: _moveToken.Token);

        if (_isAlive && !_canAttack)
        {
            SetMoveTargetPos();
            _isMoving = true;
        }
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

    private void CancelMoveRoutine()
    {
        if (_moveToken != null)
        {
            _moveToken.Cancel();
            _moveToken.Dispose();
            _moveToken = null;
        }
    }

    public int GetInstancedID()
    {
        return _instanceID;
    }
}
