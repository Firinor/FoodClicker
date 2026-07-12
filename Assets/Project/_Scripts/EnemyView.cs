using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnemyView : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI HealthText;
    public TextMeshProUGUI NameText;
    public Slider HealthSlider;

    public Image Image;
    public Button AutoClickButton;
    
    public event Action<Vector2> OnClick;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke(Camera.main!.ScreenToWorldPoint(eventData.position));
    }
}
