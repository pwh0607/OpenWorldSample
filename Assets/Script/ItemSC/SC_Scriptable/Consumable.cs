using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

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
    public bool isPresetting { get; set; }
    private int consumableCount;

    public event Action OnConsumableUsed;           //�Һ������ ���/ȹ�� �ݹ�.

    private void OnEnable()
    {
        itemType = ItemType.Consumable;
        consumableCount = 1;
        isPresetting = false;
    }

    public int GetConsumableCount() { return consumableCount; }

    public override void Use(/*GameObject player*/)
    {
        if(consumableCount <= 0)
        {
            Debug.Log("�ش� �������� �����ϴ�.");
            return;
        }

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
        consumableCount--;
        OnConsumableUsed?.Invoke();
    }

    public void GetThisItem()
    {
        consumableCount++;
        Debug.Log($"{consumableCount}");
        OnConsumableUsed?.Invoke();
    }

    public void ThrowThisItem()
    {
        consumableCount = 0;
        Debug.Log($"{subType} ���� ; {consumableCount}");
        OnConsumableUsed?.Invoke();
    }
}