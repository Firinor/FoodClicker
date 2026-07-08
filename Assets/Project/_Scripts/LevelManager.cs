using System.Linq;
using System.Numerics;
using FirMath;
using UnityEngine;

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
    private PlayerAutoAttack playerAutoAttack;
    [SerializeField] 
    private EnemyView enemyView;
    [SerializeField] 
    private ItemsListView requestView;
    [SerializeField] 
    private ItemsListView rewardView;
    
    private PlayerModel player;

    private LevelPoint enemyData;
    private EnemyModel enemy;
    
    public void Initialize(PlayerModel player)
    {
        this.player = player;
        InitializeLevelPoints();
        InitializeBattle();
        
        playerView.SetPower(player.power);
    }

    private void InitializeBattle()
    {
        playerAutoAttack.onComplete += DamageEnemy;
        enemyView.ClickButton.onClick.AddListener(DamageEnemy);
        
        enemyView.AutoClickButton.onClick.AddListener(() =>
            {
                playerView.SetAutoAttack(toState: true);
                enemyView.AutoClickButton.gameObject.SetActive(false);
            });
        
        playerView.SetAutoAttack(toState: false);
    }

    private void DamageEnemy()
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
            foreach (var item in enemyData.Requests)
            {
                player.RemoveItem(item);
            }

            needRecalculate = true;
        }

        enemy.CurrentHP -= player.power;
        enemyView.HealthText.text = enemy.CurrentHP.FormatNumber()+"/"+enemyData.MaxHealth.FormatNumber();
        var persentage = BigInteger.Divide(enemy.CurrentHP * 100, enemyData.MaxHealth);
        enemyView.HealthSlider.value = (float)persentage/100;

        if (enemy.CurrentHP <= 0)
        {
            foreach (var item in enemyData.Rewards)
            {
                player.AddItem(item);
                messages.SendMassage(item, player.ItemsCount(item).ToString(),transform.position);
            }

            enemy.CurrentHP = enemyData.MaxHealth;
            enemyView.HealthText.text = enemy.CurrentHP.FormatNumber()+"/"+enemyData.MaxHealth.FormatNumber();
            enemyView.HealthSlider.value = 1;
            needRecalculate = true;
        }

        if (needRecalculate)
        {
            player.RecalculatePower();
            playerView.SetPower(player.power);
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
                enemyView.AutoClickButton.gameObject.SetActive(true);
                playerView.SetAutoAttack(toState: false);
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
}