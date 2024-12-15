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

    //������ ��� �ߺ� ȣ�����
    bool isAttacked1 = false;
    bool isAttacked2 = false;

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
        if (isAttacked1)
        {
            return;     //����..
        }
        isAttacked1 = true;
        StartCoroutine(DamageHandler(isAttacked1));

        curHP -= damage;
        //damageText.SetActive(true);
        //damageText.GetComponent<MonsterDamage>().SetDamage(damage);
        UpdateMonsterUI();
    }

    public void UpdateMonsterUI()
    {
        curHPTxt.text = curHP.ToString();
        HP_Bar.fillAmount = (float)curHP / monsterData.HP;
    }

    IEnumerator DamageHandler(bool flag)
    {
        yield return new WaitForSeconds(0.5f);
        flag = false;
    }
}
