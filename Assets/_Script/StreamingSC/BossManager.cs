using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class BossInform{
    public string bossId;
    public int MaxHP;
    public float shortRangeAttackPower;
    public float longRangeAttackPower;
    public float deathTime;
    public GameObject bossModel;
}

public class BossManager : MonoBehaviour
{
    public List<BossInform> bossList = new List<BossInform>();
    public static BossManager bossManager { get; private set; }
    //전체적인 보스전 관리자.
    private void Awake()
    {
        if (bossManager == null)
        {
            bossManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start(){

    }

    public void UpdateBossInform(string bossId, BossInform bossData){
        Debug.Log("BossData Update!");
        var updateData = bossList.Find(boss => boss.bossId == bossId);
        updateData = bossData;
    }

    public BossInform StartBossStage(string bossId){ return bossList.Find(boss=>boss.bossId == bossId); }


}