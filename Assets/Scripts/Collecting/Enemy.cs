using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

/// <summary>
/// 몬스터 컴포넌트
/// </summary>

public class Enemy : MonoBehaviour
{
    [SerializeField] private Animator Anim;
    [SerializeField] private float Speed = 2f;
    [SerializeField] private float MoveRange = 5f;

    private int _instanceID;
    private Transform _parent;
    private Rigidbody2D _rb;
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
        _rb = GetComponent<Rigidbody2D>();

        _startPos = transform.position;
    }

    private void Start()
    {
        if (_isAlive && !_canAttack)
        {
            MoveRoutine().Forget();
        }
    }

    private void FixedUpdate()
    {
        Move();

        // 플레이어와의 충돌로 인한 밀림을 방지
        _rb.linearVelocity = Vector2.zero;
    }

    // 초기 설정
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

    // 몬스터 공격
    private void Attack()
    {
        Anim.SetTrigger("Attack");

        PlayerMoving player = CollectingManager.Inst.GetPlayer();

        float dir = player.transform.position.x - transform.position.x;
        transform.localScale = new Vector3(dir > 0 ? 1 : -1, 1, 1);

        player.TakeDamage(_ATK);
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

    // 몬스터 이동 (랜덤 방향)
    private void SetMoveTargetPos()
    {
        float xPos = UnityEngine.Random.Range(_startPos.x - MoveRange, _startPos.x + MoveRange);
        float yPos = UnityEngine.Random.Range(_startPos.y - MoveRange, _startPos.y + MoveRange);

        if (xPos < transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        _targetPos = new Vector2(xPos, yPos);
    }

    private void Move()
    {
        Anim.SetBool("Move", _isMoving);

        if (!_isMoving || _canAttack || !_isAlive)
        {
            return;
        }

        // 방향에 따라 스프라이트 플립
        float dir = _targetPos.x - transform.position.x;
        if (Mathf.Abs(dir) > 0.01f)
        {
            transform.localScale = new Vector3(dir > 0 ? 1 : -1, 1, 1);
        }

        _rb.MovePosition(Vector2.MoveTowards(_rb.position, _targetPos, Speed * Time.deltaTime));

        if (Vector2.Distance(transform.position, _targetPos) < 0.05f)
        {
            _isMoving = false;

            MoveRoutine().Forget();
        }
    }

    private async UniTaskVoid MoveRoutine()
    {
        CancelMoveRoutine();
        _moveToken = new CancellationTokenSource();

        var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(_moveToken.Token, this.GetCancellationTokenOnDestroy());

        float waitTime = UnityEngine.Random.Range(2f, 5f);

        await UniTask.Delay(TimeSpan.FromSeconds(waitTime), cancellationToken: linkedToken.Token);

        if (_isAlive && !_canAttack)
        {
            SetMoveTargetPos();
            _isMoving = true;
        }
    }

    // 공격 타겟 설정 및 취소
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _canAttack = true;
            _isMoving = false;
            CancelMoveRoutine();

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
            _isMoving = true;

            CancelAttackRoutine();
        }
    }

    // 맵 밖으로 나가지 않도록 설정
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Monster"))
        {
            _isMoving = false;
            CancelMoveRoutine();

            // 또 맵 밖의 좌표를 설정하는 것을 막기 위해 시작 위치로 되돌아가기
            _targetPos = _startPos;
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

    // 죽음 및 아이템 드롭 처리
    private async UniTask Die()
    {
        _isAlive = false;

        Anim.SetTrigger("Dead");

        float delay = Anim.GetCurrentAnimatorStateInfo(0).length;
        await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: this.GetCancellationTokenOnDestroy());

        CollectingManager.Inst.DropMonsterItem(_monsterData.DropItem, _parent, transform).Forget();

        this.gameObject.SetActive(false);
    }

    public int GetInstancedID()
    {
        return _instanceID;
    }

    // UniTask 토큰 취소
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
}
