using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    List<GameObject> attackableMonsterList = new List<GameObject>();

    public List<GameObject> GetAttackableMonsterList() => attackableMonsterList;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            attackableMonsterList.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            attackableMonsterList.Remove(other.gameObject);
        }
    }

    #region ���Ͱ� �׾��� �� ����
    private void RemoveMonster(GameObject monster)
    {
        attackableMonsterList.Remove(monster);
    }
    #endregion
}
