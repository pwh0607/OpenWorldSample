using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//ItemIconController�� ���� ���.
public class ItemContoller : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Transform originalParent;
    private ItemData currentItemData;

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
        if (transform.parent.tag == "KeyBoardPreSet_DragAndDropArea" && eventData.button == PointerEventData.InputButton.Right)
        {
            Destroy(transform.gameObject);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        rectTransform.SetParent(transform.root);        // �������� �ֻ����� �̵� (canvas)
        canvasGroup.blocksRaycasts = false;             // �巡�� �� ����� �������� ����
        currentItemData = GetComponent<ItemDataSC>().GetItem;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / transform.root.GetComponent<Canvas>().scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ItemData itemData = GetComponent<ItemDataSC>().GetItem;
        bool isPresetting = false;

        if (itemData != null && itemData is Consumable consumable)
        {
            isPresetting = consumable.isPresetting;
        }

        canvasGroup.blocksRaycasts = true;
        
        if (eventData.pointerEnter != null)
        {
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
                        SwapItemIcon(transform, eventData.pointerEnter.transform);
                    }
                    else
                    {   
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
                    if (isPresetting)
                    {
                        transform.SetParent(originalParent);
                    }
                    else
                    {
                        isPresetting = true;
                        DuplicateItemIcon(eventData.pointerEnter.transform);
                    }
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
                    Debug.Log("Type10");
                    Destroy(gameObject);
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
        iconInstance.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        transform.transform.SetParent(originalParent);

        ItemData itemData = GetComponent<ItemDataSC>().GetItem;
 
        if (itemData != null && itemData is Consumable consumable)
        {
            consumable.isPresetting = true;
        }
    }

    private void OnDestroy()
    {
        ItemData itemData = GetComponent<ItemDataSC>().GetItem;

        if (itemData != null && itemData is Consumable consumable)
        {
            consumable.isPresetting = false;
        }
    }
}