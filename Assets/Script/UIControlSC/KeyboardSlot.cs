using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class KeyboardSlot : MonoBehaviour, IDropHandler
{
    public KeyCode assignedKey;
    private GameObject currentItem;             //���� �����¿� �����ϴ� ������

    void Update()
    {
        if (Input.GetKeyDown(assignedKey))
        {
            if (transform.childCount == 0)
            {
                currentItem = null;
            }

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
        currentItem = item;
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerDrag.gameObject.name);
        GameObject droppedItem = eventData.pointerDrag;                                 //�巡�� ������ item ����.
        if (droppedItem != null && droppedItem.GetComponent<ItemContoller>() != null)
        {
            droppedItem.transform.SetParent(transform);
            droppedItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

            //���� �����ۿ� ���� Ű���� �̺�Ʈ �߰��ϱ�.
            AssignItem(droppedItem);
        }
    }
}