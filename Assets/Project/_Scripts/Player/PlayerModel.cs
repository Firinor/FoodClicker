using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

public class PlayerModel
{
    private PlayerData data;

    public BigInteger power = 1;
    
    public float AttackSpeed => data.AttackSpeed;
    
    private readonly List<Item> Inventory = new();

    public event Action<Item> OnItemsChange; 
    
    public void Initialize(PlayerData playerData)
    {
        data = playerData;
    }
    
    public int ItemsCount(Item item)
    {
        return Inventory.Find(a => a.ID == item.ID).Count;
    }

    public void AddItem(Item _item)
    {
        if(_item.Count <= 0)
            return;
        
        int playerItem = Inventory.FindIndex(a => a.ID == _item.ID);

        if (playerItem < 0)
        {
            Inventory.Add(_item);
            OnItemsChange?.Invoke(_item);
        }
        else
        {
            Item item = Inventory[playerItem];
            item.Count += _item.Count;
            Inventory[playerItem] = item;
            OnItemsChange?.Invoke(item);
        }
    }
    
    public bool RemoveItem(Item item)
    {
        if (item.Count <= 0)
            throw new Exception("Сannot take away a zero or negative number of items!");
        
        int playerItem = Inventory.FindIndex(a => a.ID == item.ID);

        if (playerItem < 0)
            return false;
        if (Inventory[playerItem].Count < item.Count)
            return false;

        Item currentItem = Inventory[playerItem];
        currentItem.Count -= item.Count;
        Inventory[playerItem] = currentItem;
        
        OnItemsChange?.Invoke(currentItem);
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

    public void RecalculatePower()
    {
        int accuracy = 10000;
        power = 1;
        var Multipliers = LevelDB.Data.Multipliers;
        foreach (var item in Inventory)
        {
            float mult = Multipliers.First(mult => string.Equals(mult.ID, item.ID)).Multiplier;
            mult *= item.Count;
            mult += 1;
            power *= (int)(mult * accuracy);
        }
        foreach (var item in Inventory)
        {
            power /= accuracy;
        }
    }
}