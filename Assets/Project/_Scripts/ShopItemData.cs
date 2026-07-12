using UnityEngine;

[CreateAssetMenu(menuName = "Configs/" + nameof(ShopItemData))]
public class ShopItemData : ScriptableObject
{
    public string ID;
    public string Name;
    
    [Multiline] public string Discription;
    
    public Sprite Icon;

    public int Value;
    public int MaxLevel => Cost.Length;

    public int[] Cost;
}