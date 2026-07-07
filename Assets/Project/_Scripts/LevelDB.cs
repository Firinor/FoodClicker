using UnityEngine;

public class LevelDB : MonoBehaviour
{
    public static LevelData Data;
    public static ItemsData Items;
    public static Sprite Boss;
    
    public static void Initialize(LevelData Data, ItemsData Items, Sprite Boss)
    {
        LevelDB.Data = Data;
        LevelDB.Items = Items;
        LevelDB.Boss = Boss;
    }
}
