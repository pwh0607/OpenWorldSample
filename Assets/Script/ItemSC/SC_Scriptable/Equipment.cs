using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Head,           //defend
    Weapon,         //attack
    Cloth,          //defend
    Foot            //speed
}

[CreateAssetMenu(fileName = "Equipment", menuName = "Items/Equipment")]
public class Equipment : ItemData
{
    public EquipmentType subType;
    public float value;

    private void OnEnable()
    {
        itemType = ItemType.Equipment;
    }

    //��� �������� ������ �ǹ�...
    public override void Use(/*GameObject player*/)
    {
        GameObject playerState;
        //���� �߰��ϱ�.
        switch (subType)
        {
            case EquipmentType.Head:
                Debug.Log("��� ����");
                break;
            case EquipmentType.Weapon:
                Debug.Log("���� ����");
                break;
            case EquipmentType.Cloth:
                Debug.Log("�� ����");
                break;
            case EquipmentType.Foot:
                Debug.Log("�Ź� ����");
                break;
        }
    }
}
