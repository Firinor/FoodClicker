using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BubbleMessage : MonoBehaviour
{
    public Image Icon;
    public TextMeshProUGUI Text;
    public CanvasGroup Group;
    public Color critColor;

    public float floatSpeed = 15f;
    public float lifeTime = 2f;
    public AnimationCurve Transparency;
    private float lifeTimeLeft;

    private void Awake()
    {
        lifeTimeLeft = lifeTime;
    }

    public void SetItem(Item item, string playerItems)
    {
        if(!string.Equals(item.ID, "Gold"))
            Icon.sprite = LevelDB.Items.Items.First(i => string.Equals(i.name, item.ID));
        Text.text = "+" + item.Count + "("+playerItems+")";
    }
    public void SetCrit()
    {
        Icon.enabled = false;
        Text.text = "Crit!";
        Text.color = critColor;
    }

    private void Update()
    {
        lifeTimeLeft -= Time.deltaTime;
        float alfa = Transparency.Evaluate(lifeTimeLeft/lifeTime);
        Group.alpha = alfa;
        transform.position += Vector3.up * (floatSpeed * Time.deltaTime);
        if(lifeTimeLeft > 0)
            return;
        
        Destroy(gameObject);
    }
}
