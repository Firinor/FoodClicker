using System;
using System.Linq;
using System.Numerics;
using FirMath;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;

public class LevelManager : MonoBehaviour
{
    [SerializeField] 
    private MessageManager messages;
    [SerializeField] 
    private DigButtonView digButtonPrefab;
    [SerializeField] 
    private Transform digButtonParent;
    [SerializeField] 
    private PlayerView playerView;
    [SerializeField] 
    private InventoryView playerInventoryView;
    [SerializeField] 
    private PlayerAutoAttack playerAutoAttack;
    [SerializeField] 
    private EnemyView enemyView;
    [SerializeField] 
    private ItemsListView requestView;
    [SerializeField] 
    private ItemsListView rewardView;
    [SerializeField] 
    private TextMeshProUGUI timerText;
    [SerializeField] 
    private ShopView shopView;
    private float _timer;
    
    [SerializeField] 
    private GameObject minion;
    
    private PlayerModel player;

    private LevelPoint enemyData;
    private EnemyModel enemy;
    
    public void Initialize(PlayerModel player)
    {
        this.player = player;
        InitializeLevelPoints();
        InitializeBattle();
        
        playerInventoryView.Initialize(player);
        player.OnPowerChange += playerView.SetPower;
        playerView.SetPower(player.power);
        player.OnGoldChange += playerView.SetGold;
        playerView.SetGold(player.Gold);
        
        minion.SetActive(player.Minion);
        playerAutoAttack.Initialize(player);
        shopView.Initialize(player);
    }
    private void InitializeBattle()
    {
        playerAutoAttack.onComplete += AutoDamageEnemy;
        enemyView.OnClick += DamageEnemy;
        
        enemyView.AutoClickButton.onClick.AddListener(EnableAutoAttackPlayer);
    }

    private void EnableAutoAttackPlayer()
    {
        playerView.SetAutoAttack(toState: true);
        enemyView.AutoClickButton.gameObject.SetActive(false);
    }

    private void AutoDamageEnemy()
    {
        DamageEnemy(playerAutoAttack.transform.position);
    }
    private void DamageEnemy(Vector2 actionPoint)
    {
        bool needRecalculate = false;
        
        if (enemyData.Requests is not null
            && enemyData.Requests.Length > 0)
        {
            if(!player.HasItems(enemyData.Requests.ToList()))
            {
                playerView.SetAutoAttack(toState: false);
                return;
            }
            if (Random.value < player.NoRemoveItemsChance)
            {
                foreach (var item in enemyData.Requests)
                {
                    player.RemoveItem(item);
                }
                needRecalculate = true;
            }
        }
        
        enemy.CurrentHP -= player.GetAttackPower(out bool isCrit);
        if(isCrit && enemyData.MaxHealth <= player.power)
            messages.SendMassageCrit(actionPoint);
        enemyView.HealthText.text = enemy.CurrentHP.FormatNumber()+"/"+enemyData.MaxHealth.FormatNumber();
        var persentage = BigInteger.Divide(enemy.CurrentHP * 100, enemyData.MaxHealth);
        enemyView.HealthSlider.value = (float)persentage/100;

        if (enemy.CurrentHP <= 0)
        {
            bool isReward = Random.value < player.DobleRewardChance;
            bool isGold = Random.value < player.GoldChance;

            if (isGold)
            {
                player.AddGold(1);
                messages.SendMassage(new Item{ID = "Gold", Count = 1}, player.Gold.ToString(), actionPoint);
            }
            foreach (var item in enemyData.Rewards)
            {
                Item _item = item;
                _item.Count *= isReward ? 2 : 1;
                player.AddItem(_item);
                messages.SendMassage(_item, player.ItemsCount(_item).ToString(), actionPoint);
            }

            enemy.CurrentHP = enemyData.MaxHealth;
            enemyView.HealthText.text = enemy.CurrentHP.FormatNumber()+"/"+enemyData.MaxHealth.FormatNumber();
            enemyView.HealthSlider.value = 1;
            needRecalculate = true;
        }

        if (needRecalculate)
        {
            player.RecalculatePower();
        }
    }
    private void InitializeLevelPoints()
    {
        digButtonParent.ClearAll(instant:true);
        requestView.SetPlayer(player);
        foreach (var levelPoint in LevelDB.Data.Points)
        {
            DigButtonView newPoint = Instantiate(digButtonPrefab, digButtonParent);
            newPoint.ItemImage.sprite = levelPoint.Image;
            newPoint.Lock.gameObject.SetActive(false);
            newPoint.OnClick += () =>
            {
                if (player.AutoAutoClick
                    && levelPoint.Requests.Length == 0)
                    EnableAutoAttackPlayer();
                else
                {
                    enemyView.AutoClickButton.gameObject.SetActive(true);
                    playerView.SetAutoAttack(toState: false);
                }
                enemyView.HealthSlider.value = 1;
                enemyView.NameText.text = levelPoint.Name;
                enemyData = levelPoint;
                enemy = new()
                {
                    CurrentHP = levelPoint.MaxHealth
                };
                enemyView.HealthText.text = levelPoint.MaxHealth.FormatNumber()+"/"+levelPoint.MaxHealth.FormatNumber();
                enemyView.Image.sprite = levelPoint.Image;
                
                requestView.SetItems(levelPoint.Requests);
                rewardView.SetItems(levelPoint.Rewards);
            };
        }

        digButtonParent
            .GetChild(0)
            .GetComponent<DigButtonView>()
            .Click();
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        TimeSpan time = TimeSpan.FromSeconds(_timer);
        if((int)time.TotalHours > 0)
            timerText.text = $@"{(int)time.TotalHours}:{time:mm\:ss\.ff}";
        else
            timerText.text = $@"{time:mm\:ss\.ff}";
    }

    private void OnDestroy()
    {
        playerAutoAttack.onComplete -= AutoDamageEnemy;
        enemyView.OnClick -= DamageEnemy;
        player.OnPowerChange -= playerView.SetPower;
        player.OnGoldChange -= playerView.SetGold;
        
        enemyView.AutoClickButton.onClick.RemoveAllListeners();
    }
}