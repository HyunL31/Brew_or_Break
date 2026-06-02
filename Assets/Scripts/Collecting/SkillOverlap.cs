using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public enum OverlapType
{
    None,
    Earth
}

/// <summary>
/// 광역 범위 스킬
/// </summary>

public class SkillOverlap : SkillBase
{
    [SerializeField] private Animator Anim;

    private Vector3 _playerDir = Vector3.down;
    private int _damage;
    private float _radius = 1f;

    public void InitOverlap(OverlapType type, Vector3 playerDir, int damage, Action<int, int> onSkillCollision)
    {
        _damage = damage;

        SetOverlapEffect(type, playerDir);
        InvokeOverlapSkill();

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
        OverlapRoutine(delay).Forget();
    }

    private void InvokeOverlapSkill()
    {
        Vector2 dir = _playerDir.normalized;
        Vector2 center = (Vector2)transform.position + (dir * 1.5f);

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(center, _radius);

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

        Gizmos.DrawWireSphere(center, _radius);
    }

    private async UniTaskVoid OverlapRoutine(float delay)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: destroyCancellationToken);
        Destroy(gameObject);
    }
}