using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �ΰ��� UI ���� ��ũ��Ʈ
/// </summary>

public class HUD : MonoBehaviour
{
    public enum InfoType { Exp, Level, Kill, Time, Health }
    public InfoType type;

    Text myText;
    Slider mySlider;

    void Awake()
    {
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();
    }

    void LateUpdate()
    {
        switch (type)
        {
            // EXP�� �����ϴ� UI���
            case InfoType.Exp:
                // ���� EXP
                float curExp = GameManager.instance.exp;
                // ������������ �ʿ��� EXP
                float maxExp = GameManager.instance.nextExp[Mathf.Min(GameManager.instance.level, GameManager.instance.nextExp.Length - 1)];
               // ���� EXP�� ���� �������� �ʿ��� EXP�� ������ �����̴�
                mySlider.value = curExp / maxExp;
                break;
            // Level�� �����ϴ� UI���
            case InfoType.Level:
                //  ���� ������Ʈ�� �ؽ�Ʈ = Lv.? �� �Ҽ��� ���� ���·� ���.
                myText.text = string.Format("Lv.{0:F0}", GameManager.instance.level);
                break;
                // ���͸� ���� ���� �����ϴ� UI���
            case InfoType.Kill:
                //  ���� ������Ʈ�� �ؽ�Ʈ = �Ҽ��� ���� ���·� ���.
                myText.text = string.Format("{0:F0}", GameManager.instance.kill);
                break;
            // �ð��� �����ϴ� UI���
            case InfoType.Time:
                // �� ���ӽð����� ���� ����� ���� �ð��� �� �ð��� remainTime ������ �ʱ�ȭ.
                float remainTime = GameManager.instance.maxGameTime - GameManager.instance.gameTime;
                // remainTime�� 60���� ������ �Ҽ����� ���� ���� ǥ���� min �������� ����.
                int min = Mathf.FloorToInt(remainTime / 60);
                // remainTime�� 60���� ���� ���������� �Ҽ����� ���� �ʸ� ǥ���� sec �������� ����.
                int sec = Mathf.FloorToInt(remainTime % 60);
                // 00:00 ������ ���ڿ��� min : sec �� ǥ��.
                myText.text = string.Format("{0:D2}:{1:D2}", min, sec);
                break;
                // ü���� �����ϴ� UI���
            case InfoType.Health:
                // ���� ü�� ���� curHealth �������� ����.
                float curHealth = GameManager.instance.health;
                // �ִ� ü�� ���� maxHealth �������� ����.
                float maxHealth = GameManager.instance.maxHealth;
                // ü�� �����̵� ǥ��
                mySlider.value = curHealth / maxHealth;
                break;
        }
    }
}

