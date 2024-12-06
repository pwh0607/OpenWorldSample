using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;

public class PlayerUIController : MonoBehaviour
{
    public GameObject inventory;
    public GameObject equipment;

    Stack<GameObject> activeWindows;        //Ȱ��ȭ �Ǿ��ִ� â��..

    private void Start()
    {
        inventory.SetActive(false);
        equipment.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            SetSingleWindowActive(inventory);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            SetSingleWindowActive(equipment);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetWindowsActiveEvent();
        }
    }

    private void SetSingleWindowActive(GameObject window)
    {   
        window.SetActive(!window.activeSelf);
        activeWindows.Push(window);
    }

    private void SetWindowsActiveEvent()
    {
        if(activeWindows.Count == 0)
        {
            Debug.Log("���� ���� �ִ� �����찡 �������� �ʽ��ϴ�.");
            return;
        }

        while (activeWindows.Count > 0)
        {
            GameObject windowRef = activeWindows.Pop();

            if (windowRef.activeSelf)
            {
                //�����찡 Ȱ��ȭ�� ���¶�� �ݱ�
                windowRef.SetActive(false);
                break;
            }
        }
    }
}
