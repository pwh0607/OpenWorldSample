using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TEST_MonsterController : MonoBehaviour
{
    MonsterData monsterData;
    public MonsterData MonsterData { get { return monsterData; } set { monsterData = value; } }

    private Animator animator;

    private MonsterStateUIController monsterUI;

    private bool canTakeDamage = true;
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
        if (monsterCurHP <= 0 && !isDown)
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
        if (isDown || !canTakeDamage)
        {
            Debug.Log("����� ���� �������� ���� �ʽ��ϴ�.");
            return;
        }
        StartCoroutine(HandleDamage(damage));
    }

    float noDamage = 0.4f;

    IEnumerator HandleDamage(int damage)
    {
        canTakeDamage = false;

        animator.SetTrigger("Damaged");
        monsterCurHP -= damage;
        monsterUI.UpdateMonsterUI(monsterCurHP);
        Debug.Log($"���� ���� ������ : {damage}, ���� HP : {monsterCurHP}");

        yield return new WaitForSeconds(noDamage);
        canTakeDamage = true;
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

        //���� ������ �ִϸ��̼� �� ����.
        animator.Play("SA_Golem_Down");
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);

        Invoke("OnDownMonster", 1.5f);
        
        //����ǰ ���.
        MonsterLootHandler loots = transform.GetComponentInChildren<MonsterLootHandler>();

        if(loots != null)
        {
            loots.ShootLoots();
        }
        else
        {
            Debug.Log("����ǰ �ý��� XXXX");
        }
    }

    public void OnDownMonster()
    {
        Destroy(gameObject);
    }


    //ĳ���� �ν� ����
    //���� ������ ���� �����ϱ�.
}