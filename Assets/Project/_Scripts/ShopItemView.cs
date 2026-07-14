using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemView : MonoBehaviour
{
    public Image Icon;
    public TextMeshProUGUI Price;
    public Button Button;
    
    public void SetItem(ShopItemData item, int level)
    {
        Icon.sprite = item.Icon;
        if(level < item.Cost.Length)
            Price.text = "<sprite name=coin>" + item.Cost[level];
        else
            Price.text = "SOLD OUT";
    }
}