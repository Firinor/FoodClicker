using UnityEngine;

public class MessageManager : MonoBehaviour
{
    public Transform Parent;
    public BubbleMessage Prefab;

    public void SendMassage(Item item, string playerItems, Vector2 point)
    {
        var newMessage = Instantiate(Prefab, Parent);
        newMessage.transform.position = point;
        newMessage.SetItem(item, playerItems);
    }
}
