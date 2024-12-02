using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsumableItemSC : MonoBehaviour
{
    [SerializeField]
    private Consumable consumableItem;  // ����� Consumable ������

    public Consumable GetItem
    {
        get => consumableItem;
        set
        {
            consumableItem = value;
            MapImage();                 //�������� ������ �� �̹����� ����
        }
    }

    private Image iconImg;

    private void Start()
    {
        iconImg = GetComponent<Image>();
    }

    // �������� �������� �̹����� �����ϴ� �Լ�
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
