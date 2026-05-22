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

    public void SetProjectileEffect(ProjectileType type)
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
    }

    public void Shoot()
    {

    }
}
