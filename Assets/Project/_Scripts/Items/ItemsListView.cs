using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ItemsListView : MonoBehaviour
{
    [SerializeField]
    private ItemView[] items;

    public Color backgroundColor;

    private void Awake()
    {
        foreach (var itemView in items)
        {
            itemView.GetComponent<Image>().color = backgroundColor;
        }
    }

    public void SetItems(Item[] _items)
    {
        for(int i = 0; i < items.Length; i++)
        {
            if (i >= _items.Length)
            {
                items[i].gameObject.SetActive(false);
                continue;
            }
            
            items[i].gameObject.SetActive(true);
            string itemName = string.Concat(_items[i].ID).ToLower();
            items[i].Icon.sprite = LevelDB.Items.Items.First(sprite => string.Equals(sprite.name.ToLower(), itemName));
            items[i].Count.text = _items[i].Count.ToString();
        }
    }
}