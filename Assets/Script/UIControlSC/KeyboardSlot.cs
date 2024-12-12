using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class KeyboardSlot : DragAndDropSlot
{
    private KeyCode assignedKey;

    public void SetAssigneKey(KeyCode assignedKey) { this.assignedKey = assignedKey; }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(assignedKey))
        {
            if (currentItem != null)
            {
                UseItem();
            }
        }
    }

    void UseItem()
    {
        currentItem.GetComponent<ConsumableItemSC>().GetItem.Use();
    }

    public override void OnDrop(PointerEventData eventData)
    {
        GameObject droppedItem = eventData.pointerDrag;                                 //�巡�� ������ item ����.

        if (CheckVaildItem(droppedItem))
        {
            currentItem = droppedItem;
            PlayerController.myKeyboard.UpdateKeyboardPreset();
        }
    }
}