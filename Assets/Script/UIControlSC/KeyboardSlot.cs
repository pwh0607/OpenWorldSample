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

    public override void OnDrop(PointerEventData eventData)
    {
        GameObject droppedItem = eventData.pointerDrag;                                 //�巡�� ������ item ����.

        if (CheckVaildItem(droppedItem))
        {
            currentItem = droppedItem;
            PlayerController.player.myKeyboard.UpdateKeyboardPreset();
        }
    }
}