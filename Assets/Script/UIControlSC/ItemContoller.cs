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
        if (transform.parent.tag == "ActionBarSlot" && eventData.button == PointerEventData.InputButton.Right)
        {
            Destroy(transform.gameObject);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        rectTransform.SetParent(transform.root);                // �������� �ֻ����� �̵� (canvas)
        canvasGroup.blocksRaycasts = false;                     // �巡�� �� ����� �������� ����
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
            if(originalParent.tag == "ActionBarSlot")
            {
                if (eventData.pointerEnter.tag == "ActionBarSlot")                      //������ -> �� ������(�̵�)
                {
                    TransformItemIcon(eventData.pointerEnter.transform);
                }
                else if(eventData.pointerEnter.tag == "InventorySlot")                  //������ -> ����(���濡 �ش� �������� �����ϸ� �ش� ������ ����.
                {
                    Destroy(gameObject);
                }
                else if(eventData.pointerEnter.tag == "Item")                           //������ item1 -> ������ item2 (��ü)
                {
                    if(eventData.pointerEnter.transform.parent.tag == "ActionBarSlot")
                    {
                        SwapItemIcon(transform, eventData.pointerEnter.transform);
                    }
                    else
                    {   
                        Destroy(gameObject);
                    }
                }
                else
                {
                    Destroy(gameObject);
                }

            }
            else if(originalParent.tag == "InventorySlot")
            {
                if (eventData.pointerEnter.tag == "ActionBarSlot")                      //���� -> ������ (����)
                {
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
                else if (eventData.pointerEnter.tag == "InventorySlot" || eventData.pointerEnter.tag == "EquipmentSlot")                 //���� -> �� ���� ����(�̵�)
                {
                    TransformItemIcon(eventData.pointerEnter.transform);
                    if (eventData.pointerEnter.tag == "EquipmentSlot")
                    {
                        itemData.Use();
                    }
                }
                else if (eventData.pointerEnter.tag == "Item")                          //���� �� item1 -> ���� �� item2 (��ü)
                {
                    if(eventData.pointerEnter.transform.parent.tag == "InventorySlot")
                    {
                        SwapItemIcon(transform, eventData.pointerEnter.transform);
                    }
                    else if(eventData.pointerEnter.transform.parent.tag == "ActionBarSlot")
                    {
                        Destroy(eventData.pointerEnter);
                        DuplicateItemIcon(transform);
                    }
                    //��� ��ȯ ��� �߰��ϱ�. => Swap �� ���� ������ [Ż��], ���� ������[����]
                }
                else
                {
                    Destroy(gameObject);
                }
            }else if(originalParent.tag == "EquipmentSlot")
            {
                Debug.Log("Ż�� ���μ���1");
                //����
                /*
                 ��� ���� -> ����[�̵�]
                 ��� ���� -> ������ ���[��ȯ]
                 ��� ���� -> ���� �ܺ� [���� ���� �����α�]
                 ��� ���� -> ����� ������(��� �������� �ƴ� ���.. �Һ� Ȥ�� ��Ÿ ������), ����� �ƹ����� ��ġ
                */

                if (eventData.pointerEnter.transform.tag == "InventorySlot")
                {
                    State myState = PlayerController.player.myState;
                    if (itemData is Equipment equipment)
                    {
                        Debug.Log("Ż�� ���μ���2");
                        TransformItemIcon(eventData.pointerEnter.transform);

                    }
                    myState.DetachItem((Equipment)itemData);
                    Debug.Log("Ż�� ���μ���3");
                }
            }
        }
        else
        {
            if(originalParent.tag == "ActionBarSlot")          //������ -> ������ �ٸ� ����
            {
                Destroy(gameObject);
            }else if (originalParent.tag == "InventorySlot")
            {
                transform.SetParent(originalParent);
            }
        }

        rectTransform.anchoredPosition = Vector2.zero;
    }

    private void TransformItemIcon(Transform slot)
    {
        transform.SetParent(slot.transform); 
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