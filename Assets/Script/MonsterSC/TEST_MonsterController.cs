using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TEST_MonsterController : MonoBehaviour
{
    MonsterData monsterData;
    public MonsterData MonsterData { get { return monsterData; } set { monsterData = value; } }
    private MonsterStateUIController monsterUI;
    private Animator animator;

    public int monsterCurHP;

    private bool canTakeDamage = true;
    private bool isDown = false;

    Transform attackTarget;
    private float lastAttackTime = -Mathf.Infinity;
    private Vector3 originalPosition;

    private void Start()
    {
        monsterCurHP = monsterData.HP;
        animator = transform.GetComponent<Animator>();
        originalPosition = transform.position;
    }

    private void Update()
    {
        float distanceToTarget = Vector3.Distance(transform.position, attackTarget.position);
        DetectPlayer(distanceToTarget);

        if (IsAttackTarget(distanceToTarget))
        {
            AttackHandler();
        }

        if (monsterCurHP <= 0 && !isDown)
        {
            Down();
        }
    }
    
    private void DetectPlayer(float distanceToTarget)
    {
        if(distanceToTarget < monsterData.detectionRadius)
        {
            ChasePlayer(distanceToTarget);
        }
        else
        {
            ReturnOriginPosition();
        }

    }

    private void ReturnOriginPosition()
    {
        MoveToward(originalPosition);
    }

    private void ChasePlayer(float distanceToTarget)
    {
        MoveToward(attackTarget.position);
    }

    private void MoveToward(Vector3 destination)
    {
        float returnSpeed = 10f;

        Vector3 direction = (destination - transform.position).normalized;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * returnSpeed);
        }
        transform.position = Vector3.MoveTowards(transform.position, destination, returnSpeed * Time.deltaTime);
    }

    private bool IsAttackTarget(float distanceToTarget)
    {
        //���� Ÿ���� ���� ���� ������ ��� �Դ°�.
        return(distanceToTarget <= monsterData.attackDamageRadius);
    }

    //UI ����
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
        Debug.Log($"{monsterData.monsterName}�� ������ �����մϴ�!");
        animator.SetTrigger("Attack");

        //�ִϸ��̼��� ������ ������ ĳ���Ͱ� ���� ������ ������ ������ �ֱ�.

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