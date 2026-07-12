using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemView : MonoBehaviour
{
    public Image Icon;
    public TextMeshProUGUI Price;
    public string Discription;

    private ShopItemData data;
    
    //Sold out
    
    public void SetItem(ShopItemData item, int level)
    {
        data = item;

        Icon.sprite = item.Icon;
        Discription = item.Discription;
        Price.text = "" + item.Cost[level];
    }
}