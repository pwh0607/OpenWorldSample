using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConsumeType
{
    HP,
    MP,
    SpeedUp
}

[CreateAssetMenu(fileName = "Consumable", menuName = "Items/Consumable")]
public class Consumable : ItemData
{
    public ConsumeType potionType; // ���� Ÿ��          inspector�� ����

    private void OnEnable()
    {
        itemType = ItemType.Consumable;
    }

    public override void Use(/*GameObject player*/)
    {
        switch (potionType)
        {
            case ConsumeType.HP:
                Debug.Log("HP ���� ���!");
                break;
            case ConsumeType.MP:
                Debug.Log("MP ���� ���!");
                break;
            case ConsumeType.SpeedUp:
                Debug.Log("SpeedUp ���� ���!");
                break;
        }
    }
}