using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConsumableType
{
    HP,
    MP,
    SpeedUp
}

[CreateAssetMenu(fileName = "Consumable", menuName = "Items/Consumable")]
public class Consumable : ItemData
{
    public ConsumableType subType;

    private void OnEnable()
    {
        itemType = ItemType.Consumable;
    }

    public ConsumableType GetItemSubType()
    {
        return subType;
    }

    public override void Use(/*GameObject player*/)
    {
        switch (subType)
        {
            case ConsumableType.HP:
                Debug.Log("HP ���� ���!");
                break;
            case ConsumableType.MP:
                Debug.Log("MP ���� ���!");
                break;
            case ConsumableType.SpeedUp:
                Debug.Log("SpeedUp ���� ���!");
                break;
        }
    }
}