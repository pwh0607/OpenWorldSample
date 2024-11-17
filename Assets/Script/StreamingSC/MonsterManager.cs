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

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        //Ư�� ������ �� ��� ���� Spawn.
        CallMonsterSpawn();
    }

    //Ȱ��ȭ �Ǿ��ִ� Sector�� ���� ���� ������Ʈ�� �����´�.
    Dictionary<Vector2Int, SectorMonsterSpawner> monsterSpawners = new Dictionary<Vector2Int, SectorMonsterSpawner>();
    int monsterSpawnCallRange = 1;

    void CallMonsterSpawn()
    {
        Vector2Int currentSector = MapStreamingManager.Instance.GetSector(player.transform.position);
        for (int x = -monsterSpawnCallRange; x <= monsterSpawnCallRange; x++)
        {
            for (int y = -monsterSpawnCallRange; y <= monsterSpawnCallRange; y++)
            {
                Vector2Int spawnCallSector = currentSector + new Vector2Int(x, y);
                
            }
        }
    }

    //�ܺ� ������ Load �Ǿ����� �� �߾� ó����ũ��Ʈ�� ��������.
    public void AddMonsterSpawnerSC(Vector2Int sector, SectorMonsterSpawner spawner)
    {
        if (!monsterSpawners.ContainsKey(sector))
        {
            Debug.Log($"{sector} Scene spawner ���� �������� ����!");
            monsterSpawners[sector] = spawner;
        }
    }

    //UnLoad��..
    public void RemoveMonsterSpawnerSC(Vector2Int sector)
    {
        if (monsterSpawners.ContainsKey(sector))
        {
            Debug.Log($"{sector} Scene spawner ���� �����ϱ� ����!");
            monsterSpawners.Remove(sector);
        }
    }
}
