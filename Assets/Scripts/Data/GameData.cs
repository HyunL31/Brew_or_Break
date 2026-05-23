using System;
using System.Collections.Generic;

[Serializable]
public class GameDataBase
{
    public string ID;
}

[Serializable]
public class Character : GameDataBase
{
    public string Name;
    public string Description;
}

[Serializable]
public class Choice : GameDataBase
{
    public string ReturnID;
    public string Content;
}

[Serializable]
public class Clue : GameDataBase
{
    public string Name;
    public string Description;
    public int Point;
    public string ReturnID;
}

[Serializable]
public class Craft : GameDataBase
{
    public List<string> PotionID;
    public List<string> SuccessID;
    public string FailID;
}

[Serializable]
public class Dialogue : GameDataBase
{
    public string NextID;
    public string Speaker;
    public string Content;
    public string Facial;
    public string BGM;
    public string SFX;
    public string Background;
    public string ResultID;
}

[Serializable]
public class Ingredient : GameDataBase
{
    public string Name;
    public string Description;
    public List<float> RGB;
}

[Serializable]
public class Monster : GameDataBase
{
    public string Name;
    public int HP;
    public int ATK;
    public float CoolTime;
    public List<string> DropItem;
}

[Serializable]
public class Potion : GameDataBase
{
    public string Name;
    public string Description;
    public List<string> Ingredient;
}

[Serializable]
public class Result : GameDataBase
{
    public int Reputation;
    public int Gold;
}

[Serializable]
public class Skill : GameDataBase
{
    public string SkillName;
    public int Level;
    public string Type;
    public string Anim;
}