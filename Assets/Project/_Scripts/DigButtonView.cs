using System;
using UnityEngine;
using UnityEngine.UI;

public class DigButtonView : MonoBehaviour
{
    public Image ItemImage;
    public GameObject Lock;
    public event Action OnClick;

    [SerializeField] 
    private Button _button;

    private void Awake()
    {
        _button.onClick.AddListener(() => OnClick?.Invoke());
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
    }
}
