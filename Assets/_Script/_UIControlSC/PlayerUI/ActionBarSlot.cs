using UnityEngine;
using UnityEngine.EventSystems;

public class ActionBarSlot : DragAndDropSlot
{
    private KeyCode assignedKey;

    public void SetAssigneKey(KeyCode assignedKey) { this.assignedKey = assignedKey; }

    public void Update()
    {
        if (Input.GetKeyDown(assignedKey))
        {
            if (currentItem != null)
            {
                UseItem();
            }
            else
            {
                Debug.Log("None Item...");
            }
        }
    }

    void UseItem()
    {
        currentItem.GetComponent<ConsumableItemSC>().GetItem.Use();
    }

    public override void OnDrop(PointerEventData eventData)
    {
        base.OnDrop(eventData);
    }

    public bool CheckConsumableItem(GameObject item)
    {
        return base.CheckVaildItem<ItemType>(item, ItemType.Consumable);
    }
}