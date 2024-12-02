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
        if (eventData.pointerEnter != null)
        {
            //������ ���� �и�..
            if(originalParent.tag == "KeyBoardPreSet_DragAndDropArea")
            {
                if (eventData.pointerEnter.tag == "KeyBoardPreSet_DragAndDropArea")     //������ -> �� ������(�̵�)
                {
                    Debug.Log("Type01");
                    TransformItemIcon(eventData.pointerEnter.transform);
                }
                else if(eventData.pointerEnter.tag == "Bag_DragAndDropArea")                    //������ -> ����(���濡 �ش� �������� �����ϸ� �ش� ������ ����.
                {
                    Debug.Log("Type02");
                    Destroy(gameObject);
                }
                else if(eventData.pointerEnter.tag == "Item")                                   //������ item1 -> ������ item2 (��ü)
                {
                    if(eventData.pointerEnter.transform.parent.tag == "KeyBoardPreSet_DragAndDropArea")
                    {
                        Debug.Log("Type03");
                        //��ġ ��ȯ
                        SwapItemIcon(transform, eventData.pointerEnter.transform);
                    }
                    else
                    {   //�ٸ� ���ǵ��� ���� ��� ����!
                        Debug.Log("Type04");
                        Destroy(gameObject);
                    }
                }
                else
                {
                    //����
                    Debug.Log("Type05");
                    Destroy(gameObject);
                }

            }
            else if(originalParent.tag == "Bag_DragAndDropArea")
            {
                if (eventData.pointerEnter.tag == "KeyBoardPreSet_DragAndDropArea")     //���� -> ������ (����)
                {
                    Debug.Log("Type06");
                    DuplicateItemIcon(eventData.pointerEnter.transform);
                }
                else if (eventData.pointerEnter.tag == "Bag_DragAndDropArea")                    //���� -> �� ���� ����(�̵�)
                {
                    Debug.Log("Type07");
                    TransformItemIcon(eventData.pointerEnter.transform);
                }
                else if (eventData.pointerEnter.tag == "Item")                                   //���� �� item1 -> ���� �� item2 (��ü)
                {
                    if(eventData.pointerEnter.transform.parent.tag == "Bag_DragAndDropArea")
                    {
                        //���� ���� Item���..
                        Debug.Log("Type08");
                        SwapItemIcon(transform, eventData.pointerEnter.transform);
                    }
                    else if(eventData.pointerEnter.transform.parent.tag == "KeyBoardPreSet_DragAndDropArea")
                    {
                        //������ ���� �������̶�� ������ ���� ������Ʈ�� �����ϰ� ������ ������Ʈ�� �����Ѵ�.
                        Debug.Log("Type09");
                        Destroy(eventData.pointerEnter);
                        DuplicateItemIcon(transform);
                    }
                }
                else
                {
                    //�������� ����...
                    Debug.Log("Type10");
                    //Destroy(gameObject);
                }
            }
        }
        else
        {
            if(originalParent.tag == "KeyBoardPreSet_DragAndDropArea")          //������ -> ������ �ٸ� ����
            {
                Destroy(gameObject);
            }else if (originalParent.tag == "Bag_DragAndDropArea")
            {
                transform.SetParent(originalParent);
            }
        }
        rectTransform.anchoredPosition = Vector2.zero;
    }

    private void TransformItemIcon(Transform item)
    {
        transform.SetParent(item.transform);
    }

    private void SwapItemIcon(Transform item1, Transform item2)
    {
        Transform newParent = item2.parent;
        item1.SetParent(newParent);
        item2.SetParent(originalParent);
        item2.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    private void DuplicateItemIcon(Transform newTransform)
    {
        GameObject iconInstance = Instantiate(transform.gameObject);
        iconInstance.transform.SetParent(newTransform);
        transform.transform.SetParent(originalParent);
    }
}
