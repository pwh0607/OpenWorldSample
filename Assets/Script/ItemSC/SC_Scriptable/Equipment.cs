using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Cloth
}

[CreateAssetMenu(fileName = "Equipment", menuName = "Items/Equipment")]
public class Equipment : ItemData
{
    public EquipmentType subType;

    private void OnEnable()
    {
        itemType = ItemType.Equipment;
    }

    public override void Use(/*GameObject player*/)
    {
        switch (subType)
        {
            case EquipmentType.Weapon:
                Debug.Log("���� ����");
                break;
            case EquipmentType.Cloth:
                Debug.Log("�� ����");
                break;
        }
    }
}
