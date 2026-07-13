using System.Collections.Generic;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    public Transform Parent;
    public BubbleMessage Prefab;

    public List<BubbleMessage> bubbles = new();
    public float offsetRadius = .5f;
    
    public void SendMassage(Item item, string playerItems, Vector2 point)
    {
        var newMessage = Instantiate(Prefab, Parent);
        Vector2 randomOffset = Random.insideUnitCircle * offsetRadius;
        newMessage.transform.position = point + randomOffset;
        newMessage.SetItem(item, playerItems);
    }
    
    public void SendMassageCrit(Vector2 point)
    {
        var newMessage = Instantiate(Prefab, Parent);
        Vector2 randomOffset = Random.insideUnitCircle * offsetRadius;
        newMessage.transform.position = point + randomOffset;
        newMessage.SetCrit();
    }
}
