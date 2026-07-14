using UnityEngine;

[CreateAssetMenu(menuName = "Configs/" + nameof(ShopItemData))]
public class ShopItemData : ScriptableObject
{
    public ShopKey ID;
    public string Name;
    
    [Multiline] public string Discription;
    
    public Sprite Icon;

    public int MaxLevel => Cost.Length;

    public string Format;
    public int[] Value;
    public int[] Cost;

    public string GetEffect(int level)
    {
        return Format.Replace("X", Value[level].ToString());
    }
}