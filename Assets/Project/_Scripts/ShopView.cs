using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopView : MonoBehaviour
{
    [SerializeField] 
    private ShopItemView[] items;
    [SerializeField] 
    private GameObject Discription;
    [SerializeField] 
    private TextMeshProUGUI discriptionName;
    [SerializeField] 
    private TextMeshProUGUI discriptionLevel;
    [SerializeField] 
    private TextMeshProUGUI discription;
    [SerializeField] 
    private TextMeshProUGUI discriptionEffect;
    [SerializeField] 
    private GameObject buyButton;
    [SerializeField] 
    private GameObject closeDiscriptionButton;
    
    private PlayerModel player;
    public void Initialize(PlayerModel player)
    {
        this.player = player;

        for (int i = 0; i < LevelDB.Shop.Items.Length; i++)
        {
            ShopItemView shopItem = items[i];
            ShopItemData shopItemData = LevelDB.Shop.Items[i];
            int playerLevel = player.ItemsCount(new Item{ ID = shopItemData.ID.ToString() });
            
            shopItem.SetItem(shopItemData, playerLevel);
            shopItem.Button.onClick.AddListener(() =>
            {
                EnableDiscription(shopItemData, shopItem);
            });
        }
    }

    private void EnableDiscription(ShopItemData shopItemData, ShopItemView shopItem)
    {
        Button btn = buyButton.GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        
        int playerLevel = player.ItemsCount(new Item{ID = shopItemData.ID.ToString()});
        
        Discription.SetActive(true);
        closeDiscriptionButton.SetActive(true);
        discriptionName.text = shopItemData.Name;
        discriptionLevel.text = $"{playerLevel}/{shopItemData.MaxLevel}";
        discription.text = shopItemData.Discription;

        if (playerLevel == shopItemData.MaxLevel)
        {
            discriptionEffect.text = shopItemData.GetEffect(playerLevel);
            buyButton.SetActive(false);
        }
        else
        {
            discriptionEffect.text = $"{shopItemData.GetEffect(playerLevel)} => {shopItemData.GetEffect(playerLevel+1)}";
            buyButton.SetActive(true);
            btn.interactable = player.Gold >= shopItemData.Cost[playerLevel];
            btn.onClick.AddListener(() =>
            {
                player.RemoveGold(shopItemData.Cost[playerLevel]);
                player.UpSkill(new Item()
                {
                    ID = shopItemData.ID.ToString(),
                    Count = 1
                });
                shopItem.SetItem(shopItemData, playerLevel+1);
                EnableDiscription(shopItemData, shopItem);
            });
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < items.Length; i++)
        {
            items[i].Button.onClick.RemoveAllListeners();
        }
    }
}
