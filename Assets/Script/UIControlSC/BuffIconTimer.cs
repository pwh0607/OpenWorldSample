using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuffIconTimer : MonoBehaviour
{
    private float curTime;
    public float duration;
    private bool timerRunning;

    private bool blinking;
    public GameObject buffTimerImg;
    private Image buffStateBar;
    BuffManager buffManager;


    private void Start()
    {
        buffStateBar = buffTimerImg.GetComponent<Image>();
        StartTimer();
    }

    public void StartTimer()
    {
        timerRunning = true;
        duration = 5f;
        curTime = 0;
    }

    private void Update()
    {
        curTime += Time.deltaTime; // �� ������ ��� �ð� �߰�
        if (curTime >= duration)
        {
            timerRunning = false;
            Debug.Log("Timer finished!");

            //Ÿ�̸� ����� ��Ȱ��ȭ�ϱ�
            Destroy(this);
        }

        UpdateBuffState();
    }

    private void UpdateBuffState()
    {
        buffStateBar.fillAmount = curTime / duration;

        //���� ������ �� ���ڰŸ��� �����ϱ�.
        if (curTime / duration <= 0.1f)
        {

        }
    }

    private void OnDestroy()
    {
        //���� �Ŵ������� ������ ���� �Ǿ����� �˸�...
    }
}
