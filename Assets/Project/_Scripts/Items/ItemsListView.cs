using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ItemsListView : MonoBehaviour
{
    [SerializeField]
    private ItemView[] items;
    private PlayerModel player;
    
    public Color backgroundColor;
    
    private void Awake()
    {
        foreach (var itemView in items)
        {
            itemView.GetComponent<Image>().color = backgroundColor;
        }
    }

    public void SetPlayer(PlayerModel playerModel)
    {
        player = playerModel;
        playerModel.OnItemsChange += RefreshItems;
    }
    
    public void SetItems(Item[] _items)
    {
        if (_items == null
            || _items.Length == 0)
        {
            gameObject.SetActive(false);
            return;
        }
        gameObject.SetActive(true);
        
        for(int i = 0; i < items.Length; i++)
        {
            if (i >= _items.Length)
            {
                items[i].gameObject.SetActive(false);
                continue;
            }
            
            items[i].gameObject.SetActive(true);
            string itemName = string.Concat(_items[i].ID).ToLower();
            items[i].ID = _items[i].ID;
            items[i].Icon.sprite = LevelDB.Items.Items.First(sprite => string.Equals(sprite.name.ToLower(), itemName));
            items[i].DefaultCount = _items[i].Count;
            items[i].Count.text = items[i].DefaultCount.ToString();
            if(player is not null)
                items[i].Count.text += "/"+player.ItemsCount(_items[i]);
        }
    }

    private void RefreshItems(Item item)
    {
        ItemView itemView = items.FirstOrDefault(view => string.Equals(view.ID, item.ID));
        if(itemView is null)
            return;
        itemView.Count.text = itemView.DefaultCount +"/"+ item.Count;
    }
    
    private void OnDestroy()
    {
        if(player is not null)
            player.OnItemsChange -= RefreshItems;
    }
}