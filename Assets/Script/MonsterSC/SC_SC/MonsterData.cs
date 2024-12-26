using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterType
{
    MushRoom,
    Golem,
    Bat
}

[System.Serializable]
public class LootEntry
{
    //������ Ȯ�� ���̺�.
    public GameObject item;

    public float dropRate;

    public LootEntry(GameObject item, float dropRate)
    {
        this.item = item;
        this.dropRate = dropRate;
    }
}

[CreateAssetMenu(fileName = "Monster Data", menuName = "Scriptable Object/Monster Data")]
public class MonsterData : ScriptableObject
{
    [Header("Basic Attributes")]
    public string monsterName;
    public MonsterType monsterType;
    public int HP;
    public int attackPower;
    public float moveSpeed;

    [Header("Visuals")]
    public GameObject monsterPrefab;        // ���� 3D �� ������

    [Header("Props")]
    public List<GameObject> basicLoot;      //�⺻������ ������ �ִ� ����ǰ.
    public List<LootEntry> randomLoot;           //����ǰ

    //��ȿ�� �˻� �ڵ�.
}