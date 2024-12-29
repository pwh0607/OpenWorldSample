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

    private bool isAttacking = false;

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
        if (attackTarget != null)
        {
            HandlePlayerDetection();
        }
        else
        {
            ReturnToOriginPosition();
        }

        if (monsterCurHP <= 0 && !isDown)
        {
            Down();
        }
    }

    //ĳ���� �ν� ����
    //���� ������ ���� �����ϱ�.
    public void SetAttackTarget(Transform target)
    {
        attackTarget = target;
    }

    private void HandlePlayerDetection()
    {
        float distanceToTarget = Vector3.Distance(transform.position, attackTarget.position);

        if (distanceToTarget <= monsterData.attackDamageRadius)
        {
            if (!isAttacking)
            {
                AttackHandler();
            }
        }
        else if (distanceToTarget <= monsterData.detectionRadius)
        {
            if (!isAttacking)
            {
                ChasePlayer();
            }
        }
        else
        {
            attackTarget = null; // �ν� ���� ���
            ReturnToOriginPosition();
        }
    }

    private void ReturnToOriginPosition()
    {
        if (isAttacking) return;

        MoveToward(originalPosition);

        if (Vector3.Distance(transform.position, originalPosition) < 0.1f)
        {
            animator.SetTrigger("Idle");
        }
    }

    private void ChasePlayer()
    {
        Debug.Log("Monster Chasing Player...");
        transform.LookAt(attackTarget);

        Vector3 dir = attackTarget.position - transform.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * monsterData.moveSpeed);
        MoveToward(attackTarget.position);
    }

    private bool IsMovingArea()
    {
        float distance = Vector3.Distance(transform.position, originalPosition);

        return distance < monsterData.movingArea;
    }

    private void MoveToward(Vector3 destination)
    {
        //���� �������̶�� ����...
        if (!isAttacking)
        {
            animator.SetTrigger("Walk");
            Vector3 direction = (destination - transform.position).normalized;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * monsterData.moveSpeed);
            }
            transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * monsterData.moveSpeed);
        }
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
            return;
        }
        StartCoroutine(HandleDamage(damage));
    }

    float noDamageTime = 0.4f;

    private IEnumerator HandleDamage(int damage)
    {
        canTakeDamage = false;

        animator.SetTrigger("Damaged");
        monsterCurHP -= damage;
        monsterUI.UpdateMonsterUI(monsterCurHP);

        yield return new WaitForSeconds(noDamageTime);
        canTakeDamage = true;
    }

    public void AttackHandler()
    {
        if (!isAttacking)
        {
            animator.SetTrigger("Attack");
            isAttacking = true;

            //�ִϸ��̼��� ������ ������ ĳ���Ͱ� ���� ������ ������ ������ �ֱ�.
            StartCoroutine(MonsterAttack());
        }
    }

    private IEnumerator MonsterAttack()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationDuration = stateInfo.length;

        yield return new WaitForSeconds(animationDuration);

        //���� ������ Player�� �����ϴ���...
        Collider[] hitTargets = Physics.OverlapSphere(transform.position, monsterData.attackRadius);

        foreach(var target in  hitTargets)
        {
            if (target.CompareTag("Player"))
            {
                //Player���� ����� �ֱ�
                Debug.Log("Monster hit Player!!");
            }
        }
        isAttacking = false;
        animator.SetTrigger("Idle");
        yield return new WaitForSeconds(2);
    }

    public void Down()
    {
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
    }

    public void OnDownMonster()
    {
        Destroy(gameObject);
    }
}