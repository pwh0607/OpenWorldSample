using System.Collections.Generic;
using UnityEngine;

public enum NodeState
{
    Success,
    Failure,
    Running
}

public abstract class BTNode
{
    public abstract NodeState Evaluate();
}

public class Selector : BTNode
{
    private List<BTNode> children = new List<BTNode>();

    public Selector(List<BTNode> children)
    {
        this.children = children;
    }   

    public override NodeState Evaluate()
    {
        foreach (var node in children)
        {
            switch (node.Evaluate())
            {
                case NodeState.Success:
                    return NodeState.Success;
                case NodeState.Running:
                    return NodeState.Running;
            }
        }
        return NodeState.Success;
    }
}

public class Sequence : BTNode
{
    private List<BTNode> children;

    public Sequence(List<BTNode> children)
    {
        this.children = children;
    }

    public override NodeState Evaluate()
    {
        bool anyChildRunning = false;

        foreach (var node in children)
        {
            switch (node.Evaluate())
            {
                case NodeState.Failure: return NodeState.Failure;
                case NodeState.Running:
                {
                    anyChildRunning = true;
                    break;
                }
            }
        }

        return anyChildRunning ? NodeState.Running : NodeState.Success; 
    }
}

public class ConditionNode : BTNode
{
    private System.Func<bool> condition;

    public ConditionNode(System.Func<bool> condition)
    {
        this.condition = condition;
    }

    public override NodeState Evaluate()
    {
        return condition.Invoke()? NodeState.Success : NodeState.Failure;
    }
}

public class ActionNode : BTNode
{
    private System.Action action;

    public ActionNode(System.Action action)
    {
        this.action = action;
    }

    public override NodeState Evaluate()
    {
        action.Invoke();
        return NodeState.Success;
    }
}

//Coroutine ó�� Ư�� �ൿ�� ����ؾ��ϴ� ������ ��쿡�� Ŀ�������� node�� �����Ѵ�.
public class LookAtTargetNode : BTNode
{
    private Transform monster;
    private Transform player;
    private Animator animator;
    private float rotationSpeed;

    public LookAtTargetNode(Transform monster, Transform player, Animator animator, float rotationSpeed)
    {
        this.monster = monster;
        this.player = player;
        this.animator = animator;
        this.rotationSpeed = rotationSpeed;
    }

    public override NodeState Evaluate()
    {
        if (player == null) return NodeState.Failure;

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Damaged"))
        {
            Debug.Log("�ǰ� �ִϸ��̼� ������...");
            return NodeState.Running;
        }
        Debug.Log("���� ĳ���� �Ĵٺ��� �ൿ��...");

        Vector3 direction = (player.position - monster.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        monster.transform.rotation = Quaternion.Slerp(monster.transform.rotation, lookRotation, Time.deltaTime * 2f);

        if (Quaternion.Angle(monster.rotation, lookRotation) < 5f)
        {
            Debug.Log("���� ĳ���� �Ĵٺ��� �ൿ �Ϸ�...");
            return NodeState.Success;
        }

        return NodeState.Running;
    }
}

public class AttackTargetNode : BTNode
{
    Animator animator;
    //Transform monster;

    public AttackTargetNode(Animator animator)
    {
        this.animator = animator;
    }

    public override NodeState Evaluate()
    {
        if (animator == null) return NodeState.Failure;

        //�ߺ� ����.
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            return NodeState.Running;
        }

        Debug.Log("���� ���� ����!");
        animator.SetTrigger("Attack");

        return NodeState.Success;
    }
}