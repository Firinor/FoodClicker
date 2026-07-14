using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerModel
{
    private PlayerData data;

    public int Gold => data.Gold;
    public BigInteger power = 1;
    
    public float AttackSpeed;
    public float GoldChance;
    public float NoRemoveItemsChance;
    public float DobleRewardChance;
    public float CriticalChance;
    public float CriticalPower;
    public bool AutoAutoClick;
    public bool Minion;
    public float PowerMultipler;
    
    private readonly List<Item> Inventory = new();
    private List<Item> ShopItems = new();

    public event Action<BigInteger> OnPowerChange;
    public event Action<int> OnGoldChange;
    public event Action<Item> OnItemsChange;
    public event Action<Item> OnShopItemsChange;

    public void Initialize(PlayerData playerData)
    {
        data = playerData;
        ShopItems = playerData.ShopItems.ToList();
        RecalculateSkills();
    }
    private void RecalculateSkills()
    {
        bool isDebug = false;
        foreach (var shopItemData in LevelDB.Shop.Items)
        {
            int playerLevel = ItemsCount(new Item{ID = shopItemData.ID.ToString()});
            switch (shopItemData.ID)
            {
                case ShopKey.AutoAutoClick:
                    AutoAutoClick = shopItemData.Value[playerLevel] == 1;//1 = On; 0 = Off
                    if(isDebug)Debug.Log("AutoAutoClick"+AutoAutoClick);
                    break;
                case ShopKey.AutoSpeed:
                    AttackSpeed = shopItemData.Value[playerLevel] / 10f; // 10 pcs = 1 hit per secont; 25 pcs = 2.5 hps;
                    if(isDebug)Debug.Log("AttackSpeed"+AttackSpeed);
                    break;
                case ShopKey.CriticalChance:
                    CriticalChance = shopItemData.Value[playerLevel] / 100f;// 100 pcs = 100%; 25 pcs = 25%;
                    if(isDebug)Debug.Log("CriticalChance"+CriticalChance);
                    break;
                case ShopKey.CriticalPower:
                    CriticalPower = shopItemData.Value[playerLevel];// 1 = +100% * damage; 2 pcs = +200% * damage;
                    if(isDebug)Debug.Log("CriticalPower"+CriticalPower);
                    break;
                case ShopKey.DoubleReward:
                    DobleRewardChance = shopItemData.Value[playerLevel] / 100f;// 100 pcs = 100%; 25 pcs = 25%;
                    if(isDebug)Debug.Log("DobleRewardChance"+DobleRewardChance);
                    break;
                case ShopKey.Gold:
                    GoldChance = shopItemData.Value[playerLevel] / 100f;// 100 pcs = 100%; 25 pcs = 25%;
                    if(isDebug)Debug.Log("GoldChance"+GoldChance);
                    break;
                case ShopKey.Minion:
                    Minion = shopItemData.Value[playerLevel] == 1;//1 = On; 0 = Off
                    if(isDebug)Debug.Log("Minion"+Minion);
                    break;
                case ShopKey.NoRemove:
                    NoRemoveItemsChance = shopItemData.Value[playerLevel] / 100f;// 100 pcs = 100%; 25 pcs = 25%;
                    if(isDebug)Debug.Log("NoRemoveItemsChance"+NoRemoveItemsChance);
                    break;
                default: //ShopKey.PowerMultiple
                    PowerMultipler = shopItemData.Value[playerLevel] / 100f;// 100 pcs = +100%; 25 pcs = +25%;
                    if(isDebug)Debug.Log("PowerMultipler"+PowerMultipler);
                    break;
            }
        }
    }

    public void UpSkill(Item item)
    {
        AddShopItem(item);
        RecalculateSkills();
        RecalculatePower();
    }
    
    public void AddGold(int value)
    {
        data.Gold += value;
        OnGoldChange?.Invoke(data.Gold);
    }
    public bool RemoveGold(int value)
    {
        if (data.Gold < value)
            return false;
        
        data.Gold -= value;
        OnGoldChange?.Invoke(data.Gold);
        return true;
    }
    public int ItemsCount(Item item)
    {
        int index = Inventory.FindIndex(a => a.ID == item.ID);
        if (index >= 0)
            return Inventory[index].Count;
        
        return ShopItems.FirstOrDefault(a => a.ID == item.ID).Count;
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
    public void AddShopItem(Item _item)
    {
        if(_item.Count <= 0)
            return;
        
        int playerItem = ShopItems.FindIndex(a => a.ID == _item.ID);

        if (playerItem < 0)
        {
            ShopItems.Add(_item);
            OnShopItemsChange?.Invoke(_item);
        }
        else
        {
            Item item = ShopItems[playerItem];
            item.Count += _item.Count;
            ShopItems[playerItem] = item;
            OnShopItemsChange?.Invoke(item);
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
        power *=  100 + (int)(100 * PowerMultipler);
        power /= 100;
        foreach (var item in Inventory)
        {
            power /= accuracy;
        }
        OnPowerChange?.Invoke(power);
    }
    
    public BigInteger GetAttackPower(out bool isCrit)
    {
        BigInteger result = power;
        isCrit = false;

        if (Random.value < CriticalChance)
        {
            isCrit = true;
            result *= (int)CriticalPower;
        }
        
        return result;
    }
}