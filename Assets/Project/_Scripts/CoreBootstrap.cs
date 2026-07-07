using UnityEngine;

public class CoreBootstra : MonoBehaviour
{
    public LevelData LevelData;
    public ItemsData Items;
    public Sprite Boss;

    public LevelManager level;
    public PlayerAutoAttack playerAutoAttack;
    public PlayerModel player;
    public PlayerData playerData;
    
    private void Awake()
    {
        LevelDB.Initialize(LevelData, Items, Boss);
        
        player = new();
        player.Initialize(playerData);
            
        level.Initialize(player);
        playerAutoAttack.Initialize(player);
    }
}
