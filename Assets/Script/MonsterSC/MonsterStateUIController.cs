using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterStateUIController : MonoBehaviour
{
    MonsterData monsterData;

    public TextMeshProUGUI monsterName;            //���� �̸�.
    public TextMeshProUGUI curHPTxt;               //���� ü���� txt�������� ���

    private int curHP;
    public Image HP_Bar;

    public GameObject damageText;          //���Ͱ� �������� �Ծ��� ��, ��µǴ� ������ ����Ʈ.

    public void InitMonsterUI(MonsterData monsterData)
    {
        GetComponent<RectTransform>().localPosition = Vector2.zero;
        this.monsterData = monsterData;
        monsterName.text = monsterData.monsterName;
        curHP = monsterData.HP;
        UpdateMonsterUI();
    }

    public void TakeDamage(int damage)
    {
        curHP -= damage;
        /*
        GameObject damageIcon = Instantiate(damageText);
        damageIcon.GetComponent<MonsterDamage>().SetDamage(damage);
        */
        Debug.Log($"���� ���� ������ : {damage}, {curHP}");

        UpdateMonsterUI();
    }

    public void UpdateMonsterUI()
    {
        curHPTxt.text = curHP.ToString();
        HP_Bar.fillAmount = (float)curHP / monsterData.HP;
    }
}
