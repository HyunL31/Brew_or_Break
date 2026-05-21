using UnityEngine;

public enum AnimState
{
    None,
    Idle,
    Front,
    Side,
    Back,
    Attack,
    Dead
}

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator Anim;

    private AnimState _currentState = AnimState.Idle;

    public void SetState(AnimState state)
    {
        if (_currentState == state)
        {
            return;
        }

        switch(state)
        {
            case AnimState.Idle:
                ResetAnimation();
                break;

            case AnimState.Front:
                Anim.SetBool("Front", true);
                Anim.SetBool("Side", false);
                Anim.SetBool("Back", false);
                break;

            case AnimState.Side:
                Anim.SetBool("Side", true);
                Anim.SetBool("Back", false);
                Anim.SetBool("Front", false);
                break;

            case AnimState.Back:
                Anim.SetBool("Back", true);
                Anim.SetBool("Front", false);
                Anim.SetBool("Side", false);
                break;

            case AnimState.Attack:
            case AnimState.Dead:
                Anim.SetTrigger($"{state}");
                break;
        }

        _currentState = state;
    }

    private void ResetAnimation()
    {
        Anim.SetBool("IsMoving", false);
        Anim.SetBool("Back", false);
        Anim.SetBool("Front", false);
        Anim.SetBool("Side", false);
    }
}
