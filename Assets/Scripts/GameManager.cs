using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Inst;

    private int Day { get; set; }

    private void Awake()
    {
        Inst = this;

        // [TODO] 세이브에서 처리
        Day = 0;
    }

    public int GetDay()
    {
        return Day;
    }

    public void AddDay()
    {
        Day++;
    }
}
