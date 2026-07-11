using UnityEngine;

public class InventoryView : MonoBehaviour
{
    public Transform ItemParent;
    public InventoryItemView ItemPrefab;

    private PlayerModel player;

    public void Initialize(PlayerModel player)
    {
        this.player = player;
        
        ItemParent.ClearAll();

        foreach (var levelIngridientMultiplier in LevelDB.Data.Multipliers)
        {
            InventoryItemView newItemView = Instantiate(ItemPrefab, ItemParent);
            Item newItem = new Item()
            {
                ID = levelIngridientMultiplier.ID,
            };
            newItem.Count = player.ItemsCount(newItem);
            newItemView.Initialize(newItem);
            player.OnItemsChange += newItemView.SetItem;
        }
    }

    private void OnDestroy()
    {
        for (int i = ItemParent.childCount - 1; i >= 0; i--)
        {
            player.OnItemsChange -= ItemParent.GetChild(i).GetComponent<InventoryItemView>().SetItem;
        }
    }
}