using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ������ �������� ���� �����ϴ� ��ũ��Ʈ.
/// ������ ���� ����, ����ġ, ���ӽð� ���
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
    // enemyCleaner = �ʰ� ���� ũ���� �������� �� �ݶ��̴�
    public GameObject enemyCleaner;

    void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
    }

    // ���� ���� ���� ��
    public void GameStart(int id)
    {
        // ĳ���� id�� ��� ����
        playerId = id;
        // ĳ������ ü�� �ʱ�ȭ
        health = maxHealth;

        // �÷��̾� Ȱ��ȭ
        player.gameObject.SetActive(true);

        // �÷��̾�� ó���� ������ �ִ� ������ �ʱ�ȭ
        uiLevelUp.Select(playerId % 2);

        Resume();

        // BGM ����
        AudioManager.instance.PlayBgm(true);
        // �����ϴ� ȿ���� ����
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
    }

    // ���� ���� ���� ��
    public void GameOver()
    {
        // ���ӿ��� �ڷ�ƾ ����.
        StartCoroutine(GameOverRoutine());
    }

    // ���ӿ���(�׾��� ��) �ڷ�ƾ
    IEnumerator GameOverRoutine()
    {
        isLive = false;

        // 0.5�� ���
        yield return new WaitForSeconds(0.5f);

        // ��� ȭ��â Ȱ��ȭ
        uiResult.gameObject.SetActive(true);

        // Reslult�� Lose �Լ� ����
        uiResult.Lose();
        // Stop �Լ� ����
        Stop();

        // BGM ����
        AudioManager.instance.PlayBgm(false);
        // Lose ȿ���� ����.
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);
    }

    // ���� �̰��� ��
    public void GameVictroy()
    {
        // ���丮 �ڷ�ƾ ����
        StartCoroutine(GameVictroyRoutine());
    }

    // ���丮 �ڷ�ƾ
    IEnumerator GameVictroyRoutine()
    {
        isLive = false;
        // enemycleaner ���� 
        enemyCleaner.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        // ��� â Ȱ��ȭ
        uiResult.gameObject.SetActive(true);
        // Result�� Win �Լ� ����
        uiResult.Win();
        Stop();

        // BGM ����
        AudioManager.instance.PlayBgm(false);
        // Win ȿ���� ����
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);
    }

    // �����
    public void GameRetry()
    {
        // ���� �� ��ε�
        SceneManager.LoadScene(0);
    }

    // ���� ����
    public void GameQuit()
    {
        Application.Quit();
    }

    void Update()
    {
        // isLive�� false��� �Ʒ� �ڵ� ���� x
        if (!isLive)
            return;

        // ���ӽð� ����
        gameTime += Time.deltaTime;

        // �ִ� ���ӽð����� ���ӽð��� �� Ŀ���� ��� ( ���� �¸� ���� )
        if (gameTime > maxGameTime)
        {
            // gameTime�� �ִ� ���� �ð����� ����.
            gameTime = maxGameTime;
            // Victory �Լ� ����.
            GameVictroy();
        }
    }

    // ����ġ ���� �Լ�.
    public void GetExp()
    {
        if (!isLive)
            return;

        exp++;

        // ������ ������ ����ġ�� ���� ����ġ�� ���� ��� (�ִ� ����ġ�� �ν����Ϳ��� ���� ��.)
        if (exp == nextExp[Mathf.Min(level, nextExp.Length - 1)])
        {
            // ���� ����
            level++;
            // ����ġ 0 �ʱ�ȭ
            exp = 0;
            // LevelUp�� Show �Լ� ����.
            uiLevelUp.Show();
        }
    }

    // ���� ������ ��
    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;
        uiJoy.localScale = Vector3.zero;
    }

    // ���� �簳�� ��
    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
        uiJoy.localScale = Vector3.one;
    }
}

