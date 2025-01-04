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

    //�÷��̾���� ��ȣ�ۿ�� �÷���
    private bool isAttackingTarget;
    private bool isDetectingTarget = false;
    private bool isChaseingTarget;

    private Transform attackTarget;
    private Vector3 originalPosition;

    private void Start()
    {
        monsterCurHP = monsterData.HP;
        animator = transform.GetComponent<Animator>();
        originalPosition = transform.position;
        attackTarget = null;
    }

    private void Update()
    {
        //���� ����� �߰����� ��.
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

    //ĳ���Ͱ� ���������� ���� �����̴�.
    private void HandlePlayerDetection()
    {
        float distanceToTarget = Vector3.Distance(transform.position, attackTarget.position);

        if (distanceToTarget <= monsterData.attackableRadius)
        {
            //Ÿ���� ���� ������ �����ִ� ���.
            MonsterAttackHandler();
        }
        else
        {
            //Ÿ���� �νĹ������� ��������, ���� ������ ��� ���.
            if (attackTarget != null)
            {
                ChasePlayer();
            }
        }
    }

    private void ReturnToOriginPosition()
    {
        if (isAttackingTarget) return;

        MoveToward(originalPosition);
    }

    private void ChasePlayer()
    {
        if (isAttackingTarget) return;

        Vector3 targetDirection = attackTarget.position - transform.position;
        Quaternion targetAngle = Quaternion.LookRotation(attackTarget.position);

        //���̿� ���� �ִ��� Ȯ���ϱ�
        if (ExistingObject(targetDirection, targetAngle)) {
            isDetectingTarget = false;
            return;
        }
        else
        {
            isDetectingTarget = true;
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetDirection), Time.deltaTime * monsterData.moveSpeed);
        transform.LookAt(attackTarget);
        MoveToward(attackTarget.position);
    }

    bool ExistingObject(Vector3 direction, Quaternion angle)
    {
        if(Physics.Raycast(transform.position, direction.normalized, out RaycastHit hit, monsterData.detectionRadius, LayerMask.GetMask("Level"))){
            //�߰��� ���� �ִٸ�...
            return true;
        }
        return false;
    }

    private bool IsMovingArea()
    {
        float distance = Vector3.Distance(transform.position, originalPosition);

        return distance < monsterData.movingArea;
    }

    private void MoveToward(Vector3 destination)
    {
        //���� �������̶�� ����...
        if (!isAttackingTarget)
        {
            animator.SetBool("Walk", true);
            Vector3 direction = (destination - transform.position).normalized;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * monsterData.moveSpeed);
            }
            transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * monsterData.moveSpeed);
        }
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
        StartCoroutine(Coroutine_TakenDamage(damage));
    }

    float noDamageTime = 0.4f;
    private bool isMonsterAttackCoolDown = false;           //���� ������ ���� ��Ÿ���� �������� �ִ°�...
    private float monsterAttackCooldownTime = 5f;         //���ʹ� ������ �Ϸ�� �� 1�� �ڿ� ������ �� �ִ�.

    private IEnumerator Coroutine_TakenDamage(int damage)
    {
        canTakeDamage = false;

        animator.SetTrigger("Damaged");
        monsterCurHP -= damage;
        monsterUI.UpdateMonsterUI(monsterCurHP);

        yield return new WaitForSeconds(noDamageTime);
        canTakeDamage = true;
    }

    private IEnumerator Coroutine_AttackCoolDown()
    {
        isMonsterAttackCoolDown = true;
        yield return new WaitForSeconds(monsterAttackCooldownTime);
        isMonsterAttackCoolDown = false;
        Debug.Log("���� ��Ÿ�� ����!");
    }

    public void MonsterAttackHandler()
    {
        //���Ͱ� �÷��̾ ������ �����̰� ���� ���ݵ� ���� ���� ���¿����� ���� �ǽ�.
        if (isDetectingTarget && !isAttackingTarget && !isMonsterAttackCoolDown)
        {
            isAttackingTarget = true;
            StartCoroutine(Coroutine_MonsterAttack());
        }
    }

    private IEnumerator Coroutine_MonsterAttack()
    {
        animator.SetBool("Walk", false);
        animator.SetTrigger("Attack");
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationDuration = stateInfo.length;

        yield return new WaitForSeconds(animationDuration);

        //���� ������ Player�� �����ϴ���...
        Collider[] hitTargets = Physics.OverlapSphere(transform.position, monsterData.attackDamageRadius);

        foreach(var target in  hitTargets)
        {
            if (target.CompareTag("Player"))
            {
                //Player���� ����� �ֱ�
                Debug.Log("Monster hit Player!!");
            }
        }
        isAttackingTarget = false;

        //���� ��ٿ� �����ϱ�.
        StartCoroutine(Coroutine_AttackCoolDown());
    }

    public void Down()
    {
        isDown = true;

        //���� ������ �ִϸ��̼� �� ����.
        animator.SetTrigger("Down");
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