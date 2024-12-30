using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemIconController : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
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
        rectTransform.SetParent(transform.root);                        // �������� �ֻ����� �̵� (canvas)
        canvasGroup.blocksRaycasts = false;                             // �巡�� �� ����� �������� ����
        currentItemData = GetComponent<ItemDataSC>().GetItem;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / transform.root.GetComponent<Canvas>().scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ItemData itemData = GetComponent<ItemDataSC>().GetItem;
        Transform dropTarget = eventData.pointerEnter?.transform;

        originalParent.GetComponent<DragAndDropSlot>().CleanCurrentItem();

        canvasGroup.blocksRaycasts = true;

        // ���Ժ� ó��
        if (originalParent.GetComponent<DragAndDropSlot>() is InventorySlot)
        {
            if (!HandleInventorySlot(dropTarget, itemData))
            {
                ResetToOriginalSlot();
                return;
            }
        }
        else if (originalParent.GetComponent<DragAndDropSlot>() is ActionBarSlot)
        {
            if (!HandleActionBarSlot(dropTarget)) {
                ResetToOriginalSlot();
                return;
            } 
        }
        else if (originalParent.GetComponent<DragAndDropSlot>() is EquipmentSlot)
        {
            if(!HandleEquipmentSlot(dropTarget, itemData))
            {
                ResetToOriginalSlot();
                return;
            }
        }

        // ���� ����ȭ
        dropTarget.GetComponent<DragAndDropSlot>().AssignCurrentItem(gameObject);
    }

    private bool HandleInventorySlot(Transform dropTarget, ItemData itemData)
    {
        DragAndDropSlot dropSlot = dropTarget.GetComponent<DragAndDropSlot>();

        if (dropSlot == null)
        {
            //dropTarget ��ü�� ������ ������ �θ� �������� ��������
            dropSlot = dropTarget.GetComponentInParent<DragAndDropSlot>();

            if (dropSlot == null) return false;
        }

        bool hasItem = dropSlot.GetCurrentItem() != null;

        //Inventory ����
        if (dropSlot is ActionBarSlot actionBarSlot)
        {
            //Inventory -> ActionBar
            Debug.Log("Inventory -> ActionBar");
            if (itemData is Consumable consumable && consumable.isPresetting)
            {
                return false;
            }
            else
            {
                DuplicateItemIcon(dropTarget);
            }
        }
        else if (dropSlot is InventorySlot inventorySlot)
        {
            if (!hasItem)
            {
                //Inventory -> Inventory[��]
                Debug.Log("Inventory -> Inventory[��]");
                TransformItemIcon(dropTarget);
            }
            else
            {
                //Inventory -> Inventory[�������� �Ҵ� �� Slot]
                Debug.Log("Inventory -> Inventory[�������� �Ҵ� �� Slot]");
                Transform changeItem = dropTarget.GetComponentInChildren<ItemIconController>().transform;            //������ ������ ������Ʈ�� �������ִ� ������Ʈ�� Transform ��������

                SwapItemIcon(transform, changeItem);
            }
        }
        else if (dropSlot is EquipmentSlot equipmentSlot)
        {
            if (!equipmentSlot.CheckEquipmentItem(gameObject))
            {
                Debug.Log($"{gameObject.name}�� ��� �������� �ƴմϴ�.");
                return false;
            }

            if (equipmentSlot.CheckEquipmentItem(gameObject))
            {
                //�󽽷� �ΰ��...
                if (!hasItem)
                {
                    Debug.Log("Inventory -> Equipment[�� ����]");
                    TransformItemIcon(dropTarget);
                    itemData.Use();
                }
                else
                {
                    Debug.Log("Inventory -> Equipment[�Ҵ�� ����]");
                    //�������� �Ҵ�Ǿ��ִ� ���..
                    Equipment equipedItem = dropTarget.GetComponentInChildren<ItemIconController>().GetComponent<EquipmentItemSC>().GetItem as Equipment;
                    equipedItem.Detach();

                    //�ٲ� ������
                    Equipment newItem = GetComponent<EquipmentItemSC>().GetItem as Equipment;
                    newItem.Use();

                    Transform changeItem = dropTarget.GetComponentInChildren<ItemIconController>().transform;            //������ ������ ������Ʈ�� �������ִ� ������Ʈ�� Transform ��������
                    SwapItemIcon(transform, changeItem);
                }
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
        return true;
    }

    //������ ���� ó��
    private bool HandleActionBarSlot(Transform dropTarget)
    {
        DragAndDropSlot dropSlot = dropTarget.GetComponent<DragAndDropSlot>();
        
        if (dropSlot == null)
        {
            //dropTarget ��ü�� ������ ������ �θ� �������� ��������
            dropSlot = dropTarget.GetComponentInParent<DragAndDropSlot>();

            if (dropSlot == null) return false;
        }

        bool hasItem = dropSlot.GetCurrentItem() != null;
        //actionBar ����
        if (dropSlot is ActionBarSlot)
        {
            if(!hasItem)
            {
                //ActionBar -> ActionBar[��] : �̵�
                Debug.Log("ActionBar -> ActionBar[��]");
                TransformItemIcon(dropTarget);
            }
            else
            {
                //ActionBar -> ActionBar[�������� �Ҵ�� ����]
                Debug.Log("ActionBar -> ActionBar[�������� �Ҵ�� ����]");
                Transform changeItem = dropTarget.GetComponentInChildren<ItemIconController>().transform;            //������ ������ ������Ʈ�� �������ִ� ������Ʈ�� Transform ��������
                SwapItemIcon(transform, changeItem);
            }
        }else if(dropSlot is InventorySlot)
        {
            //ActionBar -> Inventory
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        return true;
    }

    private bool HandleEquipmentSlot(Transform dropTarget, ItemData itemData)
    {
        DragAndDropSlot dropSlot = dropTarget.GetComponent<DragAndDropSlot>();

        if (dropSlot == null)
        {
            //dropTarget ��ü�� ������ ������ �θ� �������� ��������
            dropSlot = dropTarget.GetComponentInParent<DragAndDropSlot>();

            if (dropSlot == null) return false;
        }

        //����
        //Equipment -> Inventory[��] : �̵�
        if (dropSlot is InventorySlot)
        {
            // ��� -> �κ��丮
            if (itemData is Equipment equipment)
            {
                TransformItemIcon(dropTarget);
                equipment.Detach();
            }
        }
        else
        {
            return false;
        }
        return true;
    }

    //�巡�� Begin�� �Ҵ� ����, End�� �Ҵ�.
    private void TransformItemIcon(Transform slot)
    {
        transform.SetParent(slot.transform);
    }

    //item1�� ���� �巡���� ������
    //item2�� ���Կ� ����ִ� ������
    private void SwapItemIcon(Transform item1, Transform item2)
    {
        item1.SetParent(item2.parent);

        DragAndDropSlot slot = GetSlot(originalParent);
        slot.AssignCurrentItem(item2.gameObject);
        item2.SetParent(originalParent);

        item2.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    private void DuplicateItemIcon(Transform newTransform)
    {
        GameObject iconInstance = Instantiate(transform.gameObject);
        iconInstance.transform.SetParent(newTransform);
        iconInstance.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        newTransform.GetComponent<DragAndDropSlot>().AssignCurrentItem(gameObject);

        //���� �������� ��ġ ����
        ResetToOriginalSlot();

        Consumable itemData = GetComponent<ItemDataSC>().GetItem as Consumable;
        itemData.isPresetting = true;
    }

    private void ResetToOriginalSlot()
    {
        Debug.Log($"������ ��ġ ����");
        transform.SetParent(originalParent);
        originalParent.transform.GetComponent<DragAndDropSlot>().AssignCurrentItem(gameObject);
        rectTransform.anchoredPosition = Vector2.zero;
    }

    private void OnDestroy()
    {
        Consumable itemData = GetComponent<ItemDataSC>().GetItem as Consumable;
        itemData.isPresetting = false;
    }

    public DragAndDropSlot GetSlot(Transform dropTarget)
    {
        //�������� �Ҵ�Ǿ��ִ� ������ �������� �޼���.
        DragAndDropSlot slot = dropTarget.GetComponent<DragAndDropSlot>();

        if (slot == null)
        {
            //dropTarget ��ü�� ������ ������ �θ� �������� ��������
            slot = dropTarget.GetComponentInParent<DragAndDropSlot>();

            if (slot == null) return null;
        }

        return slot;
    }
}