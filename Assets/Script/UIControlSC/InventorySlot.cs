using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour
{
    private GameObject currentItem;             //���� �����¿� �����ϴ� ������

    void Update()
    {
        if (transform.childCount == 0)
        {
            currentItem = null;
        }
    }

    private void AssignItem(GameObject item)
    {
        currentItem = item;
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerDrag.gameObject.name);
        GameObject droppedItem = eventData.pointerDrag;                 //�巡�� ������ item ����.
        if (droppedItem != null && droppedItem.GetComponent<ItemContoller>() != null)
        {
            droppedItem.transform.SetParent(transform);
            droppedItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            AssignItem(droppedItem);
        }
    }

    public GameObject GetCurrentItem() { return currentItem; }
}