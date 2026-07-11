using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemView : MonoBehaviour
{
    public Image Icon;
    public TextMeshProUGUI CountText;
    public TextMeshProUGUI PercentageText;

    public void Initialize(Item item)
    {
        Icon.sprite = LevelDB.Items.Items.First(sprite => string.Equals(sprite.name, item.ID));
        CountText.text = item.Count.ToString();
        var mult = LevelDB.Data.Multipliers.First(mult => string.Equals(mult.ID, item.ID));
        PercentageText.text = "("+mult.Multiplier*100+"%)";
    }
    
    public void SetItem(Item item)
    {
        if(!string.Equals(Icon.sprite.name, item.ID))
            return;
        
        CountText.text = item.Count.ToString();
    }
}