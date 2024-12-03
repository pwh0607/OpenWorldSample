using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public enum ItemType
{
    Equipment,
    Consumable,
    ETC
}

[CreateAssetMenu(fileName = "Item Data", menuName = "Scriptable Object/Item Data")]
public abstract class ItemData : ScriptableObject
{
    public string itemName;         //�̸�
    public string description;      //����
    public Sprite icon;             //������
    public ItemType itemType;       //������ type
    public abstract void Use(/*GameObject player*/);
}
