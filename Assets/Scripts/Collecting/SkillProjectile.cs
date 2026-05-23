using System.Collections;
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

    private void Update()
    {
        Shoot();
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
}
