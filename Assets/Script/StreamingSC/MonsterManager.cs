using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager instance { get; private set; }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private GameObject player;

    private bool isSpawning; // �ߺ� ȣ�� ���� �÷���
    private bool isDespawning;

    private void Start()
    {
        player = GameObject.Find("Player");
        isSpawning = false;
        isDespawning = false;
    }

    private void Update()
    {
        //Ư�� ������ �� ��� ���� Spawn.
        if (!isSpawning)
        {
            StartCoroutine(CallSpawnMonster());
        }

        if (!isDespawning)
        {
            StartCoroutine(CallDespawnMonster());
        }
    }

    //Ȱ��ȭ �Ǿ��ִ� Sector�� ���� ���� ������Ʈ�� �����´�.
    Dictionary<Vector2Int, SectorMonsterSpawner> monsterSpawners = new Dictionary<Vector2Int, SectorMonsterSpawner>();
    int monsterSpawnCallRange = 1;

    IEnumerator CallSpawnMonster()
    {
        isSpawning = true;
        Vector2Int currentSector = MapStreamingManager.Instance.GetSector(player.transform.position);
        for (int x = -monsterSpawnCallRange; x <= monsterSpawnCallRange; x++)
        {
            for (int y = -monsterSpawnCallRange; y <= monsterSpawnCallRange; y++)
            {
                Vector2Int spawnCallSector = currentSector + new Vector2Int(x, y);
                while (!monsterSpawners.ContainsKey(spawnCallSector))
                {
                    yield return null;
                }
                monsterSpawners[spawnCallSector].SpawnMonster();
            }
        }
    }

    IEnumerator CallDespawnMonster()
    {
        //ĳ���Ͱ� ������ ���� ������..
        Vector2Int currentSector = MapStreamingManager.Instance.GetSector(player.transform.position);

        isDespawning = true;

        //���� Dictionary�� �ִ� �����ʵ��� Ű(Vector2Int)�� ���� Sector�� ��
        foreach (var sector in monsterSpawners.Keys)
        {
            if(Mathf.Abs(sector.x - currentSector.x) > monsterSpawnCallRange && Mathf.Abs(sector.y - currentSector.y) > monsterSpawnCallRange)
            {
                if (monsterSpawners.ContainsKey(sector))
                {
                    monsterSpawners[sector].DespawnMonster(); // ���� ó��
                }
            }
        }

        yield return null;
    }

    public void AddMonsterSpawnerSC(Vector2Int sector, SectorMonsterSpawner spawner)
    {
        if (!monsterSpawners.ContainsKey(sector))
        {
            Debug.Log($"{sector} Scene spawner ���� �������� ����!");
            monsterSpawners[sector] = spawner;
        }
    }

    public void RemoveMonsterSpawnerSC(Vector2Int sector)
    {
        if (monsterSpawners.ContainsKey(sector))
        {
            Debug.Log($"{sector} Scene spawner ���� �����ϱ� ����!");
            monsterSpawners.Remove(sector);
        }
    }
}
