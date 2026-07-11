using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemView : MonoBehaviour
{
    public Image Icon;
    public TextMeshProUGUI Price;
    public string Discription;

    public void SetItem(ShopItem item)
    {
        
    }
}

public struct ShopItem
{
    public string ID;
    public int Price;
}
