using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TEST_MonsterSpawner : MonoBehaviour
{
    public List<MonsterData> monsterDatas = new List<MonsterData>();
    public GameObject monsterStateUIPrefab;
    
    private void Start()
    {
        SpawnMonsters();
    }

    public void SpawnMonsters()
    {
        //���� ��ġ�� ��ȯ.
        foreach (var data in monsterDatas)
        {
            GameObject monster = Instantiate(data.monsterPrefab, this.transform);
            TEST_MonsterController monsterController = monster.GetComponent<TEST_MonsterController>();

            if (monsterController != null)
            {
                monsterController.MonsterData = data;
            }
            SetMonsterStateUI(monster, data);
        }
    }

    public void SetMonsterStateUI(GameObject monster, MonsterData data)
    {
        GameObject stateUI = Instantiate(monsterStateUIPrefab);
        stateUI.transform.SetParent(monster.transform);

        stateUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = data.monsterName;

        Renderer monsterRenderer = monster.transform.GetChild(0).GetComponent<Renderer>();
        if (monsterRenderer != null)
        {
            // ������ ���� ���
            float monsterHeight = monsterRenderer.bounds.size.y;
            Debug.Log($"{data.monsterName}: {monsterHeight}");

            // UI ��ġ�� ���� �Ӹ� ���� �̵�
            stateUI.transform.localPosition = new Vector3(0, monsterHeight + 0.5f, 0);
        }
    }
}