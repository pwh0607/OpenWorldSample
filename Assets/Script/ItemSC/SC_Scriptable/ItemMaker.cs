using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemMaker : MonoBehaviour
{
    [SerializeField]
    private List<Consumable> consumableItemList;
    [SerializeField]
    public GameObject iconPrefab;

    public Transform scrollContent;

    private void Start()
    {
        StartCoroutine(MakeItems());
    }

    IEnumerator MakeItems()
    {
        while (!Inventory.myInventory.CheckSlotSize())
        {
            yield return null;
        }

        foreach (var item in consumableItemList)
        {
            // �������� �����ϰ�, ������ �����͸� ����
            var consumableItem = MakeItem(item);
            consumableItem.gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            consumableItem.GetItem.Use();  // ������ ���
        }
        yield return null;
    }

    // �������� �����ϰ� Consumable �����͸� �����ϴ� �Լ�
    public ConsumableItemSC MakeItem(Consumable itemData)
    {
        // ������ �������� �ν��Ͻ�ȭ�ϰ�, ConsumableItemSC ��ũ��Ʈ�� ������
        GameObject item = Instantiate(iconPrefab);

        //�κ��丮���� �� ������ ������ �θ�� �����ϱ�.
        item.transform.SetParent(Inventory.myInventory.GetEmptyInventorySlot().gameObject.transform);
        ConsumableItemSC sc = item.GetComponent<ConsumableItemSC>();
        sc.GetItem = itemData;  // Consumable ������ ����
        return sc;
    }
}