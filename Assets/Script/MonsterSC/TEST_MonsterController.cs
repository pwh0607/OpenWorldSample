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

    private void Start()
    {
        animator = transform.GetComponent<Animator>();
        AttackHandler();
    }

    public void SetMonsterUI(MonsterStateUIController monsterUI)
    {
        this.monsterUI = monsterUI;
    }

    public void TakeDamage(int damage)
    {
        monsterUI.TakeDamage(damage);
        animator.SetTrigger("Damaged");
    }

    public void AttackHandler()
    {
        animator.SetTrigger("Attack");
    }

    //ĳ���� �ν� ����
    //���� ������ ���� �����ϱ�.
}