using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class KeyboardSlot : MonoBehaviour, IDropHandler
{
    public KeyCode assignedKey;
    public GameObject currentItem { get; set; }             //���� �����¿� �����ϴ� ������

    void Update()
    {
        if (transform.childCount == 0)
        {
            currentItem = null;
        }

        if (Input.GetKeyDown(assignedKey))
        {
            if (currentItem != null)
            {
                UseItem();
            }
            else
            {
                Debug.Log("Null Item...");
            }
        }
    }

    void UseItem()
    {
        currentItem.GetComponent<ConsumableItemSC>().GetItem.Use();
    }

    private void AssignItem(GameObject item)
    {
        Debug.Log($"{item} �Ҵ�..");
        currentItem = item;
        Keyboard.myKeyboard.UpdateKeyboardPreset();
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedItem = eventData.pointerDrag;                                 //�巡�� ������ item ����.
        AssignItem(droppedItem);
    }
}