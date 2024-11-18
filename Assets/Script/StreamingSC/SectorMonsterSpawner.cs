using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorMonsterSpawner : MonoBehaviour
{
    // Scene ���ο� �����ϸ� Scene�� Ȱ��ȭ �Ǿ����� ����ϰ��ִ�.
    // ĳ���Ϳ��� �Ÿ��� 1Sector ���� ���� ��, ���͸� Ȱ��ȭ �Ѵ�.
    public static SectorMonsterSpawner localInstance;
    
    public static ObjectPooling ObjectPooling;
    public List<GameObject> spawnedMonster;


    private void Awake()
    {
        localInstance = this;
        spawnedMonster = new List<GameObject>();
    }

    public int spawnCount;          //���ͺ��� ������ ����.

    public Vector2Int sectorVec;
    
    void Start()
    {
        ObjectPooling = GetComponent<ObjectPooling>();
    }

    private void OnEnable()
    {
        MonsterManager.instance.AddMonsterSpawnerSC(sectorVec, localInstance);
    }

    private void OnDisable()
    {
        MonsterManager.instance.RemoveMonsterSpawnerSC(sectorVec);       
    }

    public void SpawnMonster()
    {
        GameObject monster = ObjectPooling.GetObject(transform.position, transform.rotation);   
        spawnedMonster.Add(monster);
    }

    public void DespawnMonster()
    {
        if (spawnedMonster.Count > 0)
        {
            foreach (GameObject monster in spawnedMonster)
            {
                ObjectPooling.ReturnObject(monster);
            }
        }
    }
}