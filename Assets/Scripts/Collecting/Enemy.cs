using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Animator Anim;

    private int _instanceID;
    private string _monsterID;

    private Monster _monsterData;
    private int _ATK;
    private int _HP;
    private float _CoolTime;
    private bool _isAlive = true;
    private bool _canAttack = false;
    private Coroutine attackRoutine;

    private void OnDisable()
    {
        _isAlive = false;
    }

    public void InitMonster(int instanceID, string monsterID)
    {
        _instanceID = instanceID;
        _monsterID = monsterID;

        _monsterData = GameDataManager.Inst.GetMonsterData(monsterID);
        _ATK = _monsterData.ATK * GameManager.Inst.GetDay();
        _HP = _monsterData.HP;
        _CoolTime = _monsterData.CoolTime;
    }

    IEnumerator AttackRoutine()
    {
        while (_isAlive && _canAttack)
        {
            Attack();

            yield return new WaitForSeconds(_CoolTime);

            if(!_isAlive || !_canAttack)
            {
                break;
            }
        }
    }

    private void Attack()
    {
        Anim.SetTrigger("Attack");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _canAttack = true;

            if (attackRoutine == null)
            {
                attackRoutine = StartCoroutine(AttackRoutine());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player");
            _canAttack = false;

            StopCoroutine(attackRoutine);
            attackRoutine = null;
        }
    }
}
