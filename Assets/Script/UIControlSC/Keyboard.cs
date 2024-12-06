using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Keyboard : MonoBehaviour
{
    public static Keyboard myKeyboard { get; private set; }

    private void Awake()
    {
        if (myKeyboard == null)
        {
            myKeyboard = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int maxSlotSize = 8;
    public GameObject slotPrefab;
    public List<KeyboardSlot> slots;            //Dictionary<KeyboardSlot, int>... �� ���� ����.
    public Transform slotParent;
    
    private void Start()
    {
        CreateKeyboardSlot();
    }

    void CreateKeyboardSlot()
    {
        float spacingX = 0f;

        Vector2 startPosition = new Vector2(-300f, 45f);
        Vector2 componentSize = new Vector2(85f, 85f);

        for (int i = 0; i < maxSlotSize; i++)
        {
            GameObject slotInstance = Instantiate(slotPrefab);
            slotInstance.transform.SetParent(slotParent);
            slotInstance.GetComponent<KeyboardSlot>().assignedKey = KeyCode.Alpha1 + i;

            AddKeyboardSlotRef(slotInstance.GetComponent<KeyboardSlot>());

            RectTransform rectTransform = slotInstance.GetComponent<RectTransform>();
            rectTransform.sizeDelta = componentSize;
            rectTransform.anchoredPosition = new Vector2(startPosition.x + i * (componentSize.x + spacingX), startPosition.y);
            rectTransform.localScale = Vector2.one;
        }
    }

    private void AddKeyboardSlotRef(KeyboardSlot slotRef)
    {
        slots.Add(slotRef);
    }

    /*
        itemType = Consumable, Equipment, ETC,
        subType = ConsumableType, EquipmentType
    */
    public bool SearchItemByType<T>(ItemType itemType, T? subType = null) where T : struct
    {
        bool res = false;
        //slot�鿡 ������ ���Ƽ� �����ϴ� �� ã��
        foreach (KeyboardSlot slot in slots)
        {
            //ĭ�� ����ְ�, Ű���忡 �ش� �������� �������� �ʴٸ� false.
            if (slot.currentItem == null) continue;
            
            ItemData slotItemData = slot.currentItem.GetComponent<ItemDataSC>().GetItem;

            Debug.Log($"current Slot Item Data : {slotItemData}");

            if (slotItemData != null && slotItemData.itemType == itemType)
            {
                Debug.Log($"current Slot Item Data�� ��ġ!! {slotItemData.itemType} == {itemType}");
                switch (itemType)
                {
                    case ItemType.Consumable:
                        Consumable consumable = (Consumable)slotItemData;
                        Debug.Log($"current Slot Item sub type Data {consumable.subType} == {subType}");
                        if (consumable != null && consumable.subType.Equals(subType))
                        {
                            return true;
                        }
                    break;
                }
            }
        }
        return false;
    }

    //keyboarslot������ ȣ���ϱ�.
    public void UpdateKeyboardPreset()
    {
        foreach(KeyboardSlot slot in slots)
        {
            if(slot.gameObject.transform.childCount ==0) continue;
            else
            {
                slot.currentItem = slot.gameObject.transform.GetChild(0).gameObject;
            }
        }
    }
}
