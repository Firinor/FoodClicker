using UnityEngine;

public class CoreBootstra : MonoBehaviour
{
    public LevelDB LevelData;

    public LevelManager level;
    private PlayerModel player;
    public PlayerData playerData;
    
    private void Awake()
    {
        LevelDB.Initialize(LevelData);
        
        player = new();
        player.Initialize(playerData);
            
        level.Initialize(player);
    }
}
