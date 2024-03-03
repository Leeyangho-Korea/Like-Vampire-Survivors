using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 게임의 전반적인 것을 관리하는 스크립트.
/// 게임의 실행 여부, 경험치, 게임시간 등등
/// </summary>

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("# Game Control")]
    public bool isLive;
    public float gameTime;
    public float maxGameTime = 2 * 10f;
    [Header("# Player Info")]
    public int playerId;
    public float health;
    public float maxHealth = 100;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600 };
    [Header("# Game Object")]
    public PoolManager pool;
    public Player player;
    public LevelUp uiLevelUp;
    public Result uiResult;
    public Transform uiJoy;
    // enemyCleaner = 맵과 같은 크기의 데미지가 쎈 콜라이더
    public GameObject enemyCleaner;

    void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
    }

    // 게임 시작 했을 때
    public void GameStart(int id)
    {
        // 캐릭터 id를 담는 변수
        playerId = id;
        // 캐릭터의 체력 초기화
        health = maxHealth;

        // 플레이어 활성화
        player.gameObject.SetActive(true);

        // 플레이어별로 처음에 가지고 있는 아이템 초기화
        uiLevelUp.Select(playerId % 2);

        Resume();

        // BGM 실행
        AudioManager.instance.PlayBgm(true);
        // 선택하는 효과음 실행
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
    }

    // 게임 오버 됐을 때
    public void GameOver()
    {
        // 게임오버 코루틴 실행.
        StartCoroutine(GameOverRoutine());
    }

    // 게임오버(죽었을 때) 코루틴
    IEnumerator GameOverRoutine()
    {
        isLive = false;

        // 0.5초 대기
        yield return new WaitForSeconds(0.5f);

        // 결과 화면창 활성화
        uiResult.gameObject.SetActive(true);

        // Reslult의 Lose 함수 실행
        uiResult.Lose();
        // Stop 함수 실행
        Stop();

        // BGM 끄기
        AudioManager.instance.PlayBgm(false);
        // Lose 효과음 실행.
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);
    }

    // 게임 이겼을 때
    public void GameVictroy()
    {
        // 빅토리 코루틴 실행
        StartCoroutine(GameVictroyRoutine());
    }

    // 빅토리 코루틴
    IEnumerator GameVictroyRoutine()
    {
        isLive = false;
        // enemycleaner 실행 
        enemyCleaner.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        // 결과 창 활성화
        uiResult.gameObject.SetActive(true);
        // Result의 Win 함수 실행
        uiResult.Win();
        Stop();

        // BGM 끄기
        AudioManager.instance.PlayBgm(false);
        // Win 효과음 실행
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);
    }

    // 재실행
    public void GameRetry()
    {
        // 현재 씬 재로드
        SceneManager.LoadScene(0);
    }

    // 게임 종료
    public void GameQuit()
    {
        Application.Quit();
    }

    void Update()
    {
        // isLive가 false라면 아래 코드 실행 x
        if (!isLive)
            return;

        // 게임시간 증가
        gameTime += Time.deltaTime;

        // 최대 게임시간보다 게임시간이 더 커졌을 경우 ( 게임 승리 조건 )
        if (gameTime > maxGameTime)
        {
            // gameTime은 최대 게임 시간으로 고정.
            gameTime = maxGameTime;
            // Victory 함수 실행.
            GameVictroy();
        }
    }

    // 경험치 증가 함수.
    public void GetExp()
    {
        if (!isLive)
            return;

        exp++;

        // 레벨업 가능한 경험치와 현재 경험치가 같을 경우 (최대 경험치를 인스펙터에서 정해 둠.)
        if (exp == nextExp[Mathf.Min(level, nextExp.Length - 1)])
        {
            // 레벨 증가
            level++;
            // 경험치 0 초기화
            exp = 0;
            // LevelUp의 Show 함수 실행.
            uiLevelUp.Show();
        }
    }

    // 게임 멈췄을 때
    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;
        uiJoy.localScale = Vector3.zero;
    }

    // 게임 재개할 때
    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
        uiJoy.localScale = Vector3.one;
    }
}

