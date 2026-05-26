using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Threading;
using UnityEngine;

public enum ProjectileType
{
    None,
    Water,
    Fire,
    Air
}

public class SkillProjectile : SkillBase
{
    [SerializeField] private Animator Anim;
    [SerializeField] private float Speed = 5f;

    private Vector3 _playerDir = Vector3.down;
    private int _damage;

    private void Update()
    {
        Shoot();
    }

    public void InitProjectile(ProjectileType type, Vector3 playerDir, int damage, Action<int, int> onSkillCollision)
    {
        SetProjectileEffect(type, playerDir);

        if (type == ProjectileType.Fire || type == ProjectileType.Water)
        {
            SetProjectileDirection(playerDir);
        }

        CollectingManager.Inst.OnSkillCollision = onSkillCollision;
        _damage = damage;
    }

    public void SetProjectileEffect(ProjectileType type, Vector3 playerDir)
    {
        switch (type)
        {
            case ProjectileType.Water:
                Anim.SetTrigger("Water");
                break;

            case ProjectileType.Fire:
                Anim.SetTrigger("Fire");
                break;

            case ProjectileType.Air:
                Anim.SetTrigger("Air");
                break;
        }

        _playerDir = playerDir;

        float delay = Anim.GetCurrentAnimatorStateInfo(0).length;
        ShootCoroutine(delay, this.GetCancellationTokenOnDestroy()).Forget();
    }

    public void SetProjectileDirection(Vector3 playerDir)
    {
        if (playerDir == Vector3.up)
        {
            transform.eulerAngles = new Vector3(0, 0, 90f);
        }
        else if (playerDir == Vector3.down)
        {
            transform.eulerAngles = new Vector3(0, 0, -90f);
        }
        else if (playerDir == Vector3.left)
        {
            transform.eulerAngles = new Vector3(0, 0, 180f);
        }
        else if (playerDir == Vector3.right)
        {
            transform.eulerAngles = new Vector3(0, 0, 0f);
        }
    }

    public void Shoot()
    {
        transform.position += _playerDir * Speed * Time.deltaTime;
    }

    private async UniTaskVoid ShootCoroutine(float delay, CancellationToken token)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: token);

        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();

            if (enemy == null)
            {
                return;
            }

            int id = enemy.GetInstancedID();

            CollectingManager.Inst.OnSkillCollision?.Invoke(id, _damage);
            Destroy(this.gameObject);
        }
    }
}
