using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Inst;

    private int Day { get; set; }
    private string CurrentDialogueID { get; set; }

    private void Awake()
    {
        Inst = this;

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

    public string GetCurrentDialogueID()
    {
        return CurrentDialogueID;
    }

    public void SetCurrentDialogueID()
    {
        if (Day >= 10)
        {
            CurrentDialogueID = $"Episode_{Day}_01";
        }
        else
        {
            CurrentDialogueID = $"Episode_0{Day}_01";
        }
    }

    public void SetCurrentDialogueID(string id)
    {
        CurrentDialogueID = id;
    }
}
