using System;
using System.Collections;
using UnityEngine;

public enum OverlapType
{
    None,
    Earth
}

public class SkillOverlap : SkillBase
{
    [SerializeField] private Animator Anim;

    private Vector3 _playerDir = Vector3.down;
    private int _damage;

    public void InitOverlap(OverlapType type, Vector3 playerDir, float radius, int damage, Action<int, int> onSkillCollision)
    {
        _damage = damage;

        SetOverlapEffect(type, playerDir);
        InvokeOverlapSkill(radius);

        CollectingManager.Inst.OnSkillCollision = onSkillCollision;
    }

    private void SetOverlapEffect(OverlapType type, Vector3 playerDir)
    {
        switch (type)
        {
            case OverlapType.Earth:
                Anim.SetTrigger("Earth");
                break;
        }

        _playerDir = playerDir;

        float delay = Anim.GetCurrentAnimatorStateInfo(0).length;
        StartCoroutine(OverlapCoroutine(delay));
    }

    private void InvokeOverlapSkill(float radius)
    {
        Vector2 dir = _playerDir.normalized;
        Vector2 center = (Vector2)transform.position + (dir * 1.5f);

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(center, radius);

        foreach (Collider2D col in hitColliders)
        {
            if (col.gameObject != this.gameObject && col.CompareTag("Monster"))
            {
                Enemy enemy = col.gameObject.GetComponent<Enemy>();

                CollectingManager.Inst.OnSkillCollision?.Invoke(enemy.GetInstancedID(), _damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 dir = _playerDir.normalized;

        Vector2 center = (Vector2)transform.position + (dir * 1.5f);

        Gizmos.DrawWireSphere(center, 2);
    }

    private IEnumerator OverlapCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}