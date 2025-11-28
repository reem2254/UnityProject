using UnityEngine;


[CreateAssetMenu]
public class Item : ScriptableObject
{
    public bool grabbable;
    public AudioClip audioClip;
    [TextArea(4,1)]
    public string text;

    public Sprite image;
    [Header("Inventory")]
    public bool inventoryItem;
    public string collectMessage;
}