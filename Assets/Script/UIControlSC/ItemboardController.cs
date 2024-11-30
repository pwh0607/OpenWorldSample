using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemboardController : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerDrag.gameObject.name);
        GameObject droppedItem = eventData.pointerDrag;         //�巡�� ������ item ����.
        if (droppedItem != null && droppedItem.GetComponent<ItemContoller>() != null)
        {
            droppedItem.transform.SetParent(transform);
            droppedItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            
            //���� �����ۿ� ���� Ű���� �̺�Ʈ �߰��ϱ�.
        }
    }
}

/*
 public class KeyPreset : MonoBehaviour
{
    public KeyCode assignedKey;
    private GameObject currentItem;

    void Update()
    {
        if (currentItem != null && Input.GetKeyDown(assignedKey))
        {
            UseItem();
        }
    }

    public void AssignItem(GameObject item)
    {
        currentItem = item;
    }

    private void UseItem()
    {
        Debug.Log("���� ������: " + currentItem.name);
        Destroy(currentItem);  // ���÷� ������ ����
    }
}
 */