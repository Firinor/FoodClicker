using UnityEngine;

[CreateAssetMenu(fileName = "ShopItemsData", menuName = "Configs/ShopItemsData", order = 2)]
public class ShopItemsData : ScriptableObject
{
    public ShopItemData[] Items;
}

public enum ShopKey
{
    AutoAutoClick,
    AutoSpeed,
    CriticalChance,
    CriticalPower,
    DoubleReward,
    Minion,
    Gold,
    NoRemove,
    PowerMultiple
}