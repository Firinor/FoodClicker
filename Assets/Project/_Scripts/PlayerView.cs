using System.Numerics;
using FirMath;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    public TextMeshProUGUI GoldText;
    public TextMeshProUGUI PowerText;

    public Image GGImage;
    
    public PlayerAutoAttack AutoAttackImage;

    public void SetAutoAttack(bool toState)
    {
        AutoAttackImage.gameObject.SetActive(toState);
    }

    public void SetPower(BigInteger power)
    {
        PowerText.text = power.FormatNumber();
    }
}