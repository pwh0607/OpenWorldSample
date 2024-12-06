using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class InventoryItemIconManager : MonoBehaviour
{
    [SerializeField]
    private List<Consumable> consumableItemList;            //�׽�Ʈ�� �ڵ�...

    [SerializeField]
    public GameObject iconBasePrefab;           //icon Base..

    public Transform scrollContent;

    public void CreateItemIcon(ItemData newItemData, InventorySlot emptySlot)
    {
        StartCoroutine(CreateItemIconCoroutine(newItemData, emptySlot));
    }

    IEnumerator CreateItemIconCoroutine(ItemData newItemData, InventorySlot emptySlot)
    {
        while (!Inventory.myInventory.CheckSlotSize())
        {
            yield return null;
        }

        var newItemIcon = AssignItemData(newItemData, emptySlot);
        newItemIcon.gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        //�κ��丮���� �� ������ ������ �θ�� �����ϱ�.
        newItemIcon.gameObject.transform.SetParent(emptySlot.gameObject.transform);
        newItemIcon.GetComponent<RectTransform>().localScale = Vector2.one;             //ĭ �߾� ����.

        Inventory.myInventory.UpdateInventorySlots();
        yield return null;
    }

    // �������� �����ϰ� �ش� �����Ϳ� �ɸ´� SC�� �����ϴ� �Լ�
    public ItemDataSC AssignItemData(ItemData itemData, InventorySlot emptySlot)
    {
        GameObject item = Instantiate(iconBasePrefab);

        ItemDataSC itemDataSC = item.GetComponent<ItemDataSC>();
        if (itemDataSC != null)
        {
            if (itemData.itemType == ItemType.Consumable && itemData is Consumable consumable)
            {
                ((ConsumableItemSC)itemDataSC).SetItem(consumable);             // Consumable �ʱ�ȭ
            }
            else if (itemData.itemType == ItemType.Equipment && itemData is Equipment equipment)
            {
                //((EquipmentItemSC)itemDataSC).SetItem(equipment);             // Equipment �ʱ�ȭ
            }
            else
            {
                Debug.LogError("�� �� ���� ������ Ÿ���Դϴ�.");
            }
        }
        return itemDataSC;
    }
}