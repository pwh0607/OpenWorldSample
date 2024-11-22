using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class SectorMonsterSpawner : MonoBehaviour
{
    private ObjectPooling ObjectPooling;                                    // ObjectPooling�� ������ SC
    private List<GameObject> spawnedMonsters = new List<GameObject>();      // ��ȯ�� ������ List
    public List<Transform> spawnPosition;                                   //sector ���� ���� ��ȯ ��ġ�� Inspector�� ���� ���� �����ϱ�!
    
    private bool isSpawning;
    public Vector2Int sectorVec;
    
    void Start()
    {
        ObjectPooling = GetComponent<ObjectPooling>();
        isSpawning = false;
        SpawnMonster();
    }
    
    public void OnPlayerEnter()
    {
        if (!isSpawning)
        {
            SpawnMonster();
            isSpawning = true;
        }
    }

    public void OnPlayerExit()
    {
        if (isSpawning)
        {
            DespawnMonsterTotal();
            isSpawning = false;
        }
    }

    private void OnEnable()
    {
        isSpawning = false;
        MonsterManager.instance.AddMonsterSpawnerSC(sectorVec, this);
    }

    private void OnDisable()
    {
        isSpawning = false;
        MonsterManager.instance.RemoveMonsterSpawnerSC(sectorVec);
    }

    public void SpawnMonster()
    {
        GameObject monster1 = ObjectPooling.GetObject("Monster-BlackBall", spawnPosition[0].position, Quaternion.identity);
        GameObject monster2 = ObjectPooling.GetObject("Monster-Mushroom", spawnPosition[1].position, Quaternion.identity);
        GameObject monster3 = ObjectPooling.GetObject("Monster-Golem", spawnPosition[2].position, Quaternion.identity);
    }

    public void DespawnMonsterTotal()
    {
        foreach (GameObject monster in spawnedMonsters)
        {
            ObjectPooling.ReturnObject(monster);
        }
        spawnedMonsters.Clear();
    }
}