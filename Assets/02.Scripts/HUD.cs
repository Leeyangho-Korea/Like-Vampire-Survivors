using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 인게임 UI 관리 스크립트
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
            // EXP를 관리하는 UI라면
            case InfoType.Exp:
                // 현재 EXP
                float curExp = GameManager.instance.exp;
                // 다음레벨까지 필요한 EXP
                float maxExp = GameManager.instance.nextExp[Mathf.Min(GameManager.instance.level, GameManager.instance.nextExp.Length - 1)];
               // 현재 EXP와 다음 레벨까지 필요한 EXP를 보여줄 슬라이더
                mySlider.value = curExp / maxExp;
                break;
            // Level을 관리하는 UI라면
            case InfoType.Level:
                //  현재 오브젝트의 텍스트 = Lv.? 의 소수점 없는 형태로 출력.
                myText.text = string.Format("Lv.{0:F0}", GameManager.instance.level);
                break;
                // 몬스터를 죽인 수를 관리하는 UI라면
            case InfoType.Kill:
                //  현재 오브젝트의 텍스트 = 소수점 없는 형태로 출력.
                myText.text = string.Format("{0:F0}", GameManager.instance.kill);
                break;
            // 시간을 관리하는 UI라면
            case InfoType.Time:
                // 총 게임시간에서 현재 진행된 게임 시간을 뺀 시간을 remainTime 변수로 초기화.
                float remainTime = GameManager.instance.maxGameTime - GameManager.instance.gameTime;
                // remainTime을 60으로 나누고 소수점을 버려 분을 표현할 min 지역변수 정의.
                int min = Mathf.FloorToInt(remainTime / 60);
                // remainTime을 60으로 나눈 나머지에서 소수점을 버려 초를 표현할 sec 지역변수 정의.
                int sec = Mathf.FloorToInt(remainTime % 60);
                // 00:00 형태의 문자열로 min : sec 을 표현.
                myText.text = string.Format("{0:D2}:{1:D2}", min, sec);
                break;
                // 체력을 관리하는 UI라면
            case InfoType.Health:
                // 현재 체력 담을 curHealth 지역변수 정의.
                float curHealth = GameManager.instance.health;
                // 최대 체력 담을 maxHealth 지역변수 정의.
                float maxHealth = GameManager.instance.maxHealth;
                // 체력 슬라이드 표현
                mySlider.value = curHealth / maxHealth;
                break;
        }
    }
}

