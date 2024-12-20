using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TEST_MonsterController : MonoBehaviour
{
    MonsterData monsterData;
    public MonsterData MonsterData { set { monsterData = value; } }

    private Animator animator;

    private MonsterStateUIController monsterUI;

    private bool isDamaged = true;
    private bool isDown = false;

    public int monsterCurHP;

    private void Start()
    {
        monsterCurHP = monsterData.HP;
        animator = transform.GetComponent<Animator>();
        AttackHandler();
    }

    private void Update()
    {
        if(monsterData.HP <= 0 && isDown)
        {
            Down();
        }
    }

    public void SetMonsterUI(MonsterStateUIController monsterUI)
    {
        this.monsterUI = monsterUI;
    }

    public void TakeDamage(int damage)
    {
        animator.SetTrigger("Damaged");
        monsterCurHP -= damage;
        monsterUI.UpdateMonsterUI(monsterCurHP);
        Debug.Log($"���� ���� ������ : {damage}, ���� HP : {monsterCurHP}");
    }

    public void AttackHandler()
    {
        Debug.Log("���� ����!!");
        animator.SetTrigger("Attack");
    }

    public void Down()
    {
        Debug.Log("���� Down!!");
        isDown = true;
        animator.SetTrigger("Down");

        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);

        // �ִϸ��̼� �̸��� ��ġ�ϰ�, ����� �������� Ȯ��
        if(currentState.IsName("SA_Golem_Down") && currentState.normalizedTime >= 1.0f)
        {
            Invoke("OnDownMonster", 2f);
        }
    }

    public void OnDownMonster()
    {
        Destroy(gameObject);
    }

    //ĳ���� �ν� ����
    //���� ������ ���� �����ϱ�.
}