using System;
using System.Numerics;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Conigs/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    public string Name;
    
    public int GoldPerWin;
    
    public LevelIngridientMultipliers[] Multipliers;
    
    public LevelPoints[] Points;
}

[Serializable]
public class LevelPoints
{
    public string Name;
    
    public int HP;
    public int Pow;
    
    public Sprite Image;
    
    public LevelIngridientCount[] Rewards;
    public LevelIngridientCount[] Requests;
}

[Serializable]
public class LevelIngridientCount
{
    public string IngridientID;
    public int Count = 1;
}

[Serializable]
public class LevelIngridientMultipliers
{
    public string ID;
    public float Multiplier;
}