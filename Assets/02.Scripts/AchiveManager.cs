using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 캐릭터 해금을 위한 스크립트
/// 일정 조건을 달성하면 PlayerPrefs를 사용하여 
/// 기기에 데이터 저장 후 해당 데이터를 불러오며
/// 목표 달성 여부를 판단하여 진행.
/// </summary>
public class AchiveManager : MonoBehaviour
{
    // 해금되지 않은 캐릭터
    public GameObject[] lockCharacter;
    // 해금된 캐릭터
    public GameObject[] unlockCharacter;
    // 알림창
    public GameObject uiNotice;

    enum Achive { UnlockPotato, UnlockBean }
    Achive[] achives;

    // 치환
    WaitForSecondsRealtime wait;

    void Awake()
    {
        // 초기화
        achives = (Achive[])Enum.GetValues(typeof(Achive));
        wait = new WaitForSecondsRealtime(5);

        // MyData 데이터가 없으면 ( 최초 실행 )
        if (!PlayerPrefs.HasKey("MyData"))
        {
            Init();
        }
    }

    void Init()
    {
        // MyData에 1의 값으로 초기화
        PlayerPrefs.SetInt("MyData", 1);

        // 해금 되지 않은 캐릭터( 목표달성해야 열리는 캐릭터 )의 이름을 Key로 한 Value를 0으로 초기화
        foreach (Achive achive in achives)
        {
            PlayerPrefs.SetInt(achive.ToString(), 0);
        }
    }

    void Start()
    {
        UnlockCharacter();
    }

    // 해금된 캐릭터 있는지 판단.
    void UnlockCharacter()
    {
        // 기본으로 락걸린 캐릭터들의 개수 만큼 순회
        for (int index = 0; index < lockCharacter.Length; index++)
        {
            // 락걸린 캐릭터들의 이름을 담을 지역변수
            string achiveName = achives[index].ToString();
            // 로컬 기기에 해당 이름의 Key값의 Value가 1인지 판단 (1이면 해금됨.)
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;
            // 해금 여부에 따라서 캐릭터 선택 오브젝트를 활성화 또는 비활성화
            lockCharacter[index].SetActive(!isUnlock);
            unlockCharacter[index].SetActive(isUnlock);
        }
    }

    void LateUpdate()
    {
        // 캐릭터 해금 조건 달성했는지 지속적으로 판단.
        foreach (Achive achive in achives)
        {
            CheckAchive(achive);
        }
    }

    void CheckAchive(Achive achive)
    {
        bool isAchive = false;

        switch (achive)
        {
            //UnlockPotato의 달성 조건
            case Achive.UnlockPotato:
                // 살아있는 동안
                if (GameManager.instance.isLive)
                    // 20마리 이상의 몬스터를 잡으면 성공.
                    isAchive = GameManager.instance.kill >= 20;
                break;
                // UnlockBean 달성 조건
            case Achive.UnlockBean:
                // 게임시간 끝까지 살아남으면 성공.
                isAchive = GameManager.instance.gameTime == GameManager.instance.maxGameTime;
                break;
        }

        // 달성했는데 달성한 조건의 Value가 0이였다면 (아직 해금되지 않았는데 달성했을 때)
        if (isAchive && PlayerPrefs.GetInt(achive.ToString()) == 0)
        {
            // 해금의 표시로 달성한 조건의 Key값의 Value를 1로 세팅.
            PlayerPrefs.SetInt(achive.ToString(), 1);

            // 알림 UI에서
            for (int index = 0; index < uiNotice.transform.childCount; index++)
            {
                // 성공한 조건 index와 현재 index가 맞으면 자식 오브젝트 중 해당 인덱스의 오브젝트 활성화.
                bool isActive = index == (int)achive;
                uiNotice.transform.GetChild(index).gameObject.SetActive(isActive);
            }

            StartCoroutine(NoticeRoutine());
        }
    }

    // 알림창 코루틴
    IEnumerator NoticeRoutine()
    {   
        // 알림창 활성화
        uiNotice.SetActive(true);
        // 알림창에 맞는 사운드  출력
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);
        // 위에서 초기화한 5초 대기
        yield return wait;
        // 알림창 비활성화
        uiNotice.SetActive(false);
    }
}
