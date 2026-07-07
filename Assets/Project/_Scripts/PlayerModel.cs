using System;
using System.Collections.Generic;
using System.Numerics;

public class PlayerModel
{
    private PlayerData data;

    public BigInteger power = 1;
    public float AttackSpeed => data.AttackSpeed;
    
    private readonly List<Item> Inventory = new();

    public void Initialize(PlayerData playerData)
    {
        data = playerData;
    }
    
    public int ItemsCount(Item item)
    {
        return Inventory.Find(a => a.ID == item.ID).Count;
    }

    public void AddItem(Item item)
    {
        if(item.Count <= 0)
            return;
        
        Item playerItem = Inventory.Find(a => a.ID == item.ID);

        if (string.IsNullOrEmpty(playerItem.ID))
            Inventory.Add(item);
        else
            playerItem.Count += item.Count;
    }
    
    public bool RemoveItem(Item item)
    {
        if (item.Count <= 0)
            throw new Exception("Сannot take away a zero or negative number of items!");
        
        Item playerItem = Inventory.Find(a => a.ID == item.ID);

        if (string.IsNullOrEmpty(playerItem.ID))
            return false;
        if (playerItem.Count < item.Count)
            return false;
        
        playerItem.Count -= item.Count;
        return true;
    }
    
    public bool HasItems(List<Item> items)
    {
        foreach (Item item in items)
        {
            if (item.Count <= 0)
                throw new Exception("Сannot take away a zero or negative number of items!");
            
            Item playerItem = Inventory.Find(a => a.ID == item.ID);

            if (string.IsNullOrEmpty(playerItem.ID))
                return false;
            if (playerItem.Count < item.Count)
                return false;
        }
        return true;
    }
}