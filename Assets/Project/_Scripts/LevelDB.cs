using UnityEngine;

public class LevelDB : MonoBehaviour
{
    [SerializeField] private LevelData data;
    [SerializeField] private ItemsData items;
    [SerializeField] private ShopItemsData shop;
    public static LevelData Data;
    public static ItemsData Items;
    public static ShopItemsData Shop;
    public static Sprite Boss;
    
    public static void Initialize(LevelDB obj)
    {
        Data = obj.data;
        Items = obj.items;
        Shop = obj.shop;
        //Boss = Boss;
    }
}
