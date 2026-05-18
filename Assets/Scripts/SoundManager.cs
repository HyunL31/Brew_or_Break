using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Inst;

    private void Awake()
    {
        Inst = this;
    }
}
