using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BubbleMessage : MonoBehaviour
{
    public Image Icon;
    public TextMeshProUGUI Text;
    public CanvasGroup Group;

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
        Icon.sprite = LevelDB.Items.Items.First(i => string.Equals(i.name, item.ID));
        Text.text = "+" + item.Count + "("+playerItems+")";
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
