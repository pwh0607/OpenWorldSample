using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStates : MonoBehaviour
{
    //���Ŀ� �̱������� �ٲ��� ���...
    public GameObject HP_Bar;
    public GameObject MP_Bar;

    private Image HP_Image;
    private Image MP_Image;

    private State myState;

    //TEST �� �ڵ�
    int MaxHP = 100;
    int currentHP = 100;
    int testDamage = 10;
    int testHeal = 20;

    private void Start()
    {
        myState = PlayerController.player.GetMyState();
        HP_Image = HP_Bar.GetComponent<Image>();
        MP_Image = MP_Bar.GetComponent<Image>();

        if(HP_Image == null)
        {
            Debug.Log("�̹��� ���� ����...");
        }

        UpdateHP_Bar();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1)) TakeDamage();

        if (Input.GetKeyDown(KeyCode.F2)) TakeHeal();
    }

    public void TakeDamage()
    {
        Debug.Log("Take Damage...");
        currentHP -= testDamage;
        UpdateHP_Bar();
    }

    public void TakeHeal()
    {
        Debug.Log("Take Heal...");
        currentHP += testHeal;
        UpdateHP_Bar();
    }

    public void UpdateHP_Bar()
    {
        HP_Image.fillAmount = (float)currentHP / MaxHP;
    }
}
