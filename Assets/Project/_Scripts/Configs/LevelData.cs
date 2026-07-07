using System;
using System.Numerics;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Conigs/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    public string Name;
    
    public int GoldPerWin;
    
    public LevelIngridientMultipliers[] Multipliers;
    
    public LevelPoint[] Points;
}

[Serializable]
public class LevelPoint
{
    public string Name;
    
    public BigInteger MaxHealth => HP * BigInteger.Pow(10, Pow);
        
    public int HP;
    public int Pow;
    
    public Sprite Image;
    
    public Item[] Requests;
    public Item[] Rewards;
}

[Serializable]
public class LevelIngridientMultipliers
{
    public string ID;
    public float Multiplier;
}