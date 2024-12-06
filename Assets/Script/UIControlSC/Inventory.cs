using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory myInventory { get; private set; }

    private void Awake()
    {
        if (myInventory == null)
        {
            myInventory = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        CreateInventorySlot();
        inventoryItemIconManager = GetComponent<InventoryItemIconManager>();
    }

    public GameObject slotPrefab;
    public Transform scrollContent;                 //inventorySlot�� �θ�ü
    public List<InventorySlot> slots;               //Dictionary<InventorySlot, int>... �� ���� ����.
    public int maxSlotSize;

    void CreateInventorySlot()
    {
        int columns = 4;
        float spacingX = 0f;
        float spacingY = 0f;

        Vector2 startPosition = new Vector2(-120f, -50f);
        Vector2 componentSize = new Vector2(80f, 80f);

        for (int i = 0; i < maxSlotSize; i++)
        {
            GameObject slotInstance = Instantiate(slotPrefab);
            slotInstance.transform.SetParent(scrollContent);

            AddInventorySlotRef(slotInstance.GetComponent<InventorySlot>());

            int row = i / columns;
            int column = i % columns;

            RectTransform rectTransform = slotInstance.GetComponent<RectTransform>();
            rectTransform.sizeDelta = componentSize;
            rectTransform.localScale = Vector2.one;
            rectTransform.anchoredPosition = new Vector2(startPosition.x + column * (componentSize.x + spacingX), startPosition.y - row * (componentSize.y + spacingY));
        }
    }

    private void AddInventorySlotRef(InventorySlot slotRef)
    {
        slots.Add(slotRef);
    }

    public bool CheckSlotSize()
    {
        return maxSlotSize == slots.Count;
    }

    public InventorySlot GetEmptyInventorySlot()
    {
        for(int i = 0; i < slots.Count; i++)
        {
            if (slots[i].transform.childCount == 0)
            {
                return slots[i];
            }
        }
        return null;
    }

    private InventoryItemIconManager inventoryItemIconManager; 

    //�̿ϼ�..
    public bool SearchItemByType<T>(ItemType itemType, T? subType = null) where T : struct
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.GetCurrentItem() == null) continue;

            ItemData slotItemData = slot.GetCurrentItem().GetComponent<ItemDataSC>().GetItem;

            if (slot.GetCurrentItem() != null && slotItemData.itemType == itemType)
            {
                if (subType == null) return true;
                if (itemType == ItemType.Consumable && slotItemData is Consumable consumable)
                {
                    if (EqualityComparer<T>.Default.Equals((T)(object)consumable.subType, subType.Value))
                        return true;
                }
                else if (itemType == ItemType.Equipment && slotItemData is Equipment equipment)
                {
                    if (EqualityComparer<T>.Default.Equals((T)(object)equipment.subType, subType.Value))
                        return true;
                }
            }
        }
        return false;
    }
    
    public void GetItem(GameObject item)
    {
        ItemDataSC itemDataSC = item.GetComponent<ItemDataSC>();            //DroppedItemSC�� �����Ѵ�...
        ItemData itemData = itemDataSC.GetItem;         

        //�������� �������� ������ ������ ������ ����.
        //���� ������ �����ϱ�
        if (itemData != null) 
        {
            if (itemData is Consumable consumable)
            {
               if (SearchItemByType<ConsumableType>(itemData.itemType, consumable.subType)){
                    Debug.Log($"{consumable.subType}�� �κ��丮�� ����...");
                    consumable.GetThisItem();
                    return;
               }
               else
               {
                    //���ο� ������ �����ϰ� �� ���Կ� �ְ� �Ҵ��ϱ�.
                    GetNewItem(itemData);
                }
            }
            else if (itemData is Equipment equipment)
            {
                if (SearchItemByType<EquipmentType>(itemData.itemType, equipment.subType))
                {
                    Debug.Log($"{equipment.subType}�� �κ��丮�� ����...");
                    //������ �߰�!.
                    //���������� ���� ������ �������� �ʵ����Ѵ�.
                    //�׳� ������ ������ �������� �� ���Կ� �߰��Ѵ�.
                    return;
                }
            }
        }
        else
        {
            Debug.Log("ItemData is Null...");
        }
    }

    //inventory�� �������� ���� ������ GET
    public void GetNewItem(ItemData newItem)
    {
        InventorySlot emptySlot = GetEmptyInventorySlot();
        inventoryItemIconManager.CreateItemIcon(newItem, emptySlot);
    }

    private void HandleConsumableItem(ConsumableItemSC consumableItemSC)
    {
 
    }

    private void HandleEquipmentItem()
    {

    }

    public void UpdateInventorySlots()
    {
        Debug.Log("������ ����Ʈ ������Ʈ");
        foreach (InventorySlot slot in slots)
        {
            if (slot.transform.childCount == 0) continue;
            else
            {
                slot.AssignItem(slot.transform.GetChild(0).gameObject);
            }
        }
    }
}