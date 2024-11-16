using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class MonsterAI : MonoBehaviour
{
    public Transform[] patrolPoints;    // ���Ͱ� �̵��� Ư�� ��ġ��
    public Transform initPoint;
    
    public float waitTime = 2f;         // �� �������� ��� �ð�
    public float moveRadius = 10f;      // ���� �̵� �ݰ�
    public float speed = 10f;

    private NavMeshAgent agent;
    private int currentPointIndex;
    private bool waiting;
    private float waitTimer;
    private bool returning;             //���� ��ġ�� �����ϴ� ���� ����.

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (patrolPoints.Length > 0)
        {
            MoveToNextPatrolPoint();
        }
        else
        {
            MoveToRandomPoint();
        }
    }

    private void Update()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!waiting)
            {
                waiting = true;
                waitTimer = waitTime;
            }
        }

        if (waiting)
        {
            waitTimer -= Time.deltaTime;

            if (waitTimer <= 0f)
            {
                waiting = false;

                if (patrolPoints.Length > 0)
                {
                    MoveToNextPatrolPoint();
                }
                else
                {
                    MoveToRandomPoint();
                }
            }
        }
        DetectPlayer();
    }

    public Transform player;
    public float detectionRadius = 10f;     // ���� �Ÿ�
    public float detectionAngle = 80f;      // ���� ���� (��ä�� ����)
    
    public float backDistance = 50f;

    private void DetectPlayer()
    {
        if (IsPlayerInDetectionArea())
        {
            ChasePlayer();
        }
        else
        {
            //BackInitPoint();
        }
    }

    private void ChasePlayer()
    {
        //�ʹ� �ָ� �Ѿ�Դٸ�...
        if (Vector3.Distance(transform.position, initPoint.position) < backDistance)
        {
            transform.LookAt(player);
            Vector3 dir = player.position - transform.position;
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * speed);
        }
        else
        {
            BackInitPoint();
            returning = true;
            Debug.Log("�ʹ� �ָ����ͼ� ���� ��ġ�� �̵��մϴ�...");
        }
    }

    private bool IsPlayerInDetectionArea()
    {
        if (player == null) return false;

        // �Ÿ� ���
        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer > detectionRadius)
        {
            return false; // ���� �ݰ� ��
        }

        // ���� ���
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer.normalized);

        if (angleToPlayer > detectionAngle / 2)
        {
            return false; // ���� ���� ��
        }

        return true; // �Ÿ��� ������ ��� ������ ����
    }

    private void OnDrawGizmos()
    {
        // ����׿� ��ä�� �ð�ȭ
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Vector3 forward = transform.forward * detectionRadius;
        Quaternion leftRayRotation = Quaternion.Euler(0, -detectionAngle / 2, 0);
        Quaternion rightRayRotation = Quaternion.Euler(0, detectionAngle / 2, 0);

        Vector3 leftRay = leftRayRotation * forward;
        Vector3 rightRay = rightRayRotation * forward;

        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawRay(transform.position, leftRay);
        Gizmos.DrawRay(transform.position, rightRay);
    }

    private void BackInitPoint()
    {
        transform.LookAt(initPoint);
        Vector3 dir = initPoint.position - transform.position;
        transform.position = Vector3.MoveTowards(transform.position, initPoint.position, speed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * speed);
    }

    private void MoveToNextPatrolPoint()
    {
        // ���������� ���� ��ǥ ��ġ ����
        agent.destination = patrolPoints[currentPointIndex].position;
        currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
    }

    private void MoveToRandomPoint()
    {
        // ������ ��ġ�� NavMesh ���� ����
        Vector3 randomDirection = Random.insideUnitSphere * moveRadius;
        randomDirection += transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, moveRadius, NavMesh.AllAreas))
        {
            agent.destination = hit.position;
        }
    }
}