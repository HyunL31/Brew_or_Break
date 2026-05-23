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

    public void SetOverlapEffect(OverlapType type, Vector3 playerDir)
    {
        switch (type)
        {
            case OverlapType.Earth:
                Anim.SetTrigger("Earth");
                break;
        }

        _playerDir = playerDir;
    }

    public void InvokeOverlapSkill(float radius)
    {
        Vector2 center = (Vector2)transform.position;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(center, radius);

        foreach (Collider2D col in hitColliders)
        {
            if (col.gameObject != this.gameObject && col.CompareTag("Monster"))
            {
                // [TODO] 스킬 데미지 및 효과 적용
                Debug.Log($"{col.name}에게 데미지를 입혔습니다!");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 dir = _playerDir.normalized;
        Vector2 center = (Vector2)transform.position + (dir * 1.5f);
        Gizmos.DrawWireSphere(center, 1.0f);
    }
}