using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemContoller : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Transform originalParent;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    
    void Start()
    {
        rectTransform.anchoredPosition = Vector2.zero;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //preset������ ��Ŭ���� �����ϱ�.
        if (transform.parent.tag == "KeyBoardPreSet_DragAndDropArea" && eventData.button == PointerEventData.InputButton.Right)
        {
            Destroy(transform.gameObject);
        }
    }

    // �巡�� ���� �� ȣ��
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        rectTransform.SetParent(transform.root);  // �������� �ֻ����� �̵� (canvas)
        canvasGroup.blocksRaycasts = false;       // �巡�� �� ����� �������� ����
    }

    // �巡�� �� ȣ��
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / transform.root.GetComponent<Canvas>().scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        Debug.Log($"{eventData.pointerEnter.tag}");
        if (eventData.pointerEnter != null)
        {
            if (eventData.pointerEnter.tag == "KeyBoardPreSet_DragAndDropArea")
            {
                DuplicateItemIcon(eventData.pointerEnter.transform);
            }else if (eventData.pointerEnter.tag == "Bag_DragAndDropArea")
            {
                transform.SetParent(eventData.pointerEnter.transform);
            }
            else if (eventData.pointerEnter.tag == "Item")
            {
                //swap
                Transform item1 = transform;
                Transform item2 = eventData.pointerEnter.transform;
                Debug.Log($"{eventData.pointerEnter.name} Swap!");
                SwapItemInBag(item1, item2);
            }
            else
            {
                transform.SetParent(originalParent);
            }
        }
        else
        {
            transform.SetParent(originalParent);
        }
        rectTransform.anchoredPosition = Vector2.zero;
    }
    
    void SwapItemInBag(Transform item1, Transform item2)
    {
        Transform newParent = item2.parent;
        if (item1.parent.tag == "Bag_DragAndDropArea" && item2.parent.tag == "KeyBoardPreSet_DragAndDropArea")
        {
            //����.
            Destroy(item2.gameObject);      //������ �׸��� ����.
            DuplicateItemIcon(newParent);
        }
        else if(item1.parent.tag == "KeyBoardPreSet_DragAndDropArea" && item2.parent.tag == "KeyBoardPreSet_DragAndDropArea")
        {
            item1.SetParent(newParent);
            item2.SetParent(originalParent);
            item2.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
    }

    void DuplicateItemIcon(Transform newTransform)
    {
        GameObject iconInstance = Instantiate(transform.gameObject);
        iconInstance.transform.SetParent(newTransform);
        transform.transform.SetParent(originalParent);
    }
}
