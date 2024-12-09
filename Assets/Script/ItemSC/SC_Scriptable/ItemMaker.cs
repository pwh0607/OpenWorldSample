using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemMaker : MonoBehaviour
{
    [SerializeField]
    private List<Consumable> consumableItemList;

    [SerializeField]
    public GameObject iconBasePrefab;

    public Transform scrollContent;

    private void Start()
    {
        StartCoroutine(MakeItems());
    }

    IEnumerator MakeItems()
    {
        while (!Inventory.myInventory.CheckSlotSize())
        {
            yield return null;
        }

        foreach (var item in consumableItemList)
        {
            // �������� �����ϰ�, ������ �����͸� ����
            var consumableItem = MakeItem(item);
            consumableItem.gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }

        Inventory.myInventory.SyncUIData();
        yield return null;
    }

    // �������� �����ϰ� �ش� �����Ϳ� �ɸ´� SC�� �����ϴ� �Լ�
    public ItemDataSC MakeItem(ItemData itemData)
    {
        GameObject item = Instantiate(iconBasePrefab);

        //�κ��丮���� �� ������ ������ �θ�� �����ϱ�.
        item.transform.SetParent(Inventory.myInventory.GetEmptyInventorySlot().gameObject.transform);

        item.GetComponent<RectTransform>().localScale = Vector2.one;        //ĭ �߾� ����.

        ItemDataSC itemDataSC = item.GetComponent<ItemDataSC>();

        if(itemDataSC != null)
        {
            if (itemData.itemType == ItemType.Consumable && itemData is Consumable consumable)
            {
                ((ConsumableItemSC)itemDataSC).SetItem(consumable);         // Consumable �ʱ�ȭ
            }else if(itemData.itemType == ItemType.Equipment && itemData is Equipment equipment)
            {
                //((EquipmentItemSC)itemDataSC).SetItem(equipment);         // Consumable �ʱ�ȭ
            }
            else
            {
                Debug.LogError("�� �� ���� ������ Ÿ���Դϴ�.");
            }
        }
        return itemDataSC;
    }
}