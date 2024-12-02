using System.Collections;
using System.Collections.Generic;
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
    }

    public GameObject slotPrefab;
    public Transform scrollContent;                 //inventorySlot�� �θ�ü
    public List<InventorySlot> slots;               //Dictionary<InventorySlot, int>... �� ���� ����.
    public int maxSlotSize;

    //���Կ� �ִ� ������ ����...
    private Dictionary<InventorySlot, int> slotItemCount = new Dictionary<InventorySlot, int>();

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
            rectTransform.anchoredPosition = new Vector2(startPosition.x + column * (componentSize.x + spacingX), startPosition.y - row * (componentSize.y + spacingY));
        }
    }

    private void AddInventorySlotRef(InventorySlot slotRef)
    {
        slots.Add(slotRef);
    }

    //�ʱ�ȭ �Ϸ� üũ�� 
    public bool CheckSlotSize()
    {
        //���� �ڷ�ƾ�� ����.. üũ�� ����.
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
        //�� ���� ã�� ���� ���...
        return null;
    }

    //ConsumeType? consumeType = null [Nullable �Ű�����!!] �Լ� ȣ��� �ش� �޼���� ���� ���� �ʴ´�...
    //�ش� �������� inventory�� �����ϴ���...
    public bool SearchItemByType(ItemType itemType, ConsumeType? consumeType = null)
    {
        foreach (InventorySlot slot in slots)
        {
            //��������� �Ѿ��.
            if (slot.GetCurrentItem() == null)
                continue;

            ItemData itemData = slot.GetCurrentItem().GetComponent<ItemData>();

            if (itemData != null && itemData.itemType == itemType)
            {
                // Consumable Ÿ���� ��� potionType Ȯ��
                if (itemType == ItemType.Consumable && consumeType.HasValue)
                {
                    Consumable consumable = itemData as Consumable;
                    if (consumable != null && consumable.potionType == consumeType.Value)
                    {
                        return true; // �ش� Consumable ã��
                    }
                }
                else if (itemType == ItemType.Equipment)
                {
                    return true; // Equipment ã��
                }
                else if (itemType == ItemType.ETC)
                {

                }
            }
        }
        return false; // �ش� �������� ã�� ����
    }
}