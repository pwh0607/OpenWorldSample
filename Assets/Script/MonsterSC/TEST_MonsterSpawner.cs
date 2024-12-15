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
        float transX = -10f;

        //���� ��ġ�� ��ȯ.
        foreach (var data in monsterDatas)
        {
            GameObject monster = Instantiate(data.monsterPrefab);
            TEST_MonsterController monsterController = monster.GetComponent<TEST_MonsterController>();

            monster.transform.position = new Vector3(transX, gameObject.transform.position.y, gameObject.transform.position.z);
            
            transX += 10f;

            
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
        MonsterStateUIController monsterStateUIController = stateUI.GetComponent<MonsterStateUIController>();
        monsterStateUIController.InitMonsterUI(data);
        stateUI.transform.SetParent(monster.transform);
        stateUI.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        Renderer monsterRenderer = monster.transform.GetChild(0).GetComponent<Renderer>();

        if (monsterRenderer != null)
        {
            float monsterHeight = monsterRenderer.bounds.size.y;

            Debug.Log($"���� ��� {monsterHeight}");
            stateUI.GetComponent<RectTransform>().anchoredPosition.Set(0, monsterHeight + 0.5f);
        }

        //controller�� ui���� ����
        TEST_MonsterController monsterController = monster.GetComponent<TEST_MonsterController>();
        monsterController.SetMonsterUI(monsterStateUIController);
    }
}