using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAutoAttack : MonoBehaviour
{
    public Image AutoAttackImage;

    private PlayerModel player;

    public event Action onComplete;
    private float playerAttackSpeed;

    public void Initialize(PlayerModel player)
    {
        this.player = player;
        playerAttackSpeed = player.AttackSpeed;
    }
    
    private void OnEnable()
    {
        AutoAttackImage.fillAmount = 0;
    }

    private void OnDisable()
    {
        AutoAttackImage.fillAmount = 0;
    }

    private void Update()
    {
        AutoAttackImage.fillAmount += playerAttackSpeed * Time.deltaTime;
        if (AutoAttackImage.fillAmount >= 1)
        {
            onComplete?.Invoke();//player.Attack();
            AutoAttackImage.fillAmount = 0;
        }
    }
}