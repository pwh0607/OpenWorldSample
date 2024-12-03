using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsumableItemSC : ItemDataSC
{
    [SerializeField]
    private Consumable consumableItem;

    public override ItemData GetItem => consumableItem;

    private Image iconImg;

    private void Start()
    {
        iconImg = GetComponent<Image>();
    }
    
    public void SetItem(Consumable itemData)
    {
        consumableItem = itemData;      // ������ ����
        MapImage();                     // ������ ���� �� ������ ����
    }

    public void MapImage()
    {
        if (iconImg == null)
            iconImg = GetComponent<Image>();

        if (consumableItem != null && iconImg != null)
        {
            iconImg.sprite = consumableItem.icon;  // ������ ����
        }
        else
        {
            Debug.LogError("������ �Ǵ� Consumable �������� �������� �ʾҽ��ϴ�.");
        }
    }
}
