using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorMonsterSpawner : MonoBehaviour
{
    // Scene ���ο� �����ϸ� Scene�� Ȱ��ȭ �Ǿ����� ����ϰ��ִ�.
    // ĳ���Ϳ��� �Ÿ��� 1Sector ���� ���� ��, ���͸� Ȱ��ȭ �Ѵ�.
    public static SectorMonsterSpawner localInstance;

    private void Awake()
    {
        localInstance = this;
    }

    public GameObject monsterPrefab;
    //public Transform[] spawnPrefab;
    public int spawnCount;          //���ͺ��� ������ ����.

    private Queue<GameObject> monsterPool;
    public Vector2Int sectorVec;

    void Start()
    {
        //�ʱ�ȭ Ƚ�� �ʿ�.    
        monsterPool = new Queue<GameObject>();
        SpawnMonster();
    }

    private void OnEnable()
    {
        //Ȱ��ȭ �� ���... �߾� ó�� ������Ʈ�� SC ������ �ѱ��.
        MonsterManager.instance.AddMonsterSpawnerSC(sectorVec, localInstance);
    }

    private void OnDisable()
    {
        MonsterManager.instance.RemoveMonsterSpawnerSC(sectorVec);       
    }

    public void SpawnMonster()
    {
        //test������ ���� �������� ��ġ�� ��ȯ�Ѵ� [�������� �߾�]
        GameObject monster = Instantiate(monsterPrefab, transform.position, transform.rotation);
    }
}