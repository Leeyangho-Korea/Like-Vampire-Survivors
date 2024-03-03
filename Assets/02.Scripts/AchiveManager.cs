using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ĳ���� �ر��� ���� ��ũ��Ʈ
/// ���� ������ �޼��ϸ� PlayerPrefs�� ����Ͽ� 
/// ��⿡ ������ ���� �� �ش� �����͸� �ҷ�����
/// ��ǥ �޼� ���θ� �Ǵ��Ͽ� ����.
/// </summary>
public class AchiveManager : MonoBehaviour
{
    // �رݵ��� ���� ĳ����
    public GameObject[] lockCharacter;
    // �رݵ� ĳ����
    public GameObject[] unlockCharacter;
    // �˸�â
    public GameObject uiNotice;

    enum Achive { UnlockPotato, UnlockBean }
    Achive[] achives;

    // ġȯ
    WaitForSecondsRealtime wait;

    void Awake()
    {
        // �ʱ�ȭ
        achives = (Achive[])Enum.GetValues(typeof(Achive));
        wait = new WaitForSecondsRealtime(5);

        // MyData �����Ͱ� ������ ( ���� ���� )
        if (!PlayerPrefs.HasKey("MyData"))
        {
            Init();
        }
    }

    void Init()
    {
        // MyData�� 1�� ������ �ʱ�ȭ
        PlayerPrefs.SetInt("MyData", 1);

        // �ر� ���� ���� ĳ����( ��ǥ�޼��ؾ� ������ ĳ���� )�� �̸��� Key�� �� Value�� 0���� �ʱ�ȭ
        foreach (Achive achive in achives)
        {
            PlayerPrefs.SetInt(achive.ToString(), 0);
        }
    }

    void Start()
    {
        UnlockCharacter();
    }

    // �رݵ� ĳ���� �ִ��� �Ǵ�.
    void UnlockCharacter()
    {
        // �⺻���� ���ɸ� ĳ���͵��� ���� ��ŭ ��ȸ
        for (int index = 0; index < lockCharacter.Length; index++)
        {
            // ���ɸ� ĳ���͵��� �̸��� ���� ��������
            string achiveName = achives[index].ToString();
            // ���� ��⿡ �ش� �̸��� Key���� Value�� 1���� �Ǵ� (1�̸� �رݵ�.)
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;
            // �ر� ���ο� ���� ĳ���� ���� ������Ʈ�� Ȱ��ȭ �Ǵ� ��Ȱ��ȭ
            lockCharacter[index].SetActive(!isUnlock);
            unlockCharacter[index].SetActive(isUnlock);
        }
    }

    void LateUpdate()
    {
        // ĳ���� �ر� ���� �޼��ߴ��� ���������� �Ǵ�.
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
            //UnlockPotato�� �޼� ����
            case Achive.UnlockPotato:
                // ����ִ� ����
                if (GameManager.instance.isLive)
                    // 20���� �̻��� ���͸� ������ ����.
                    isAchive = GameManager.instance.kill >= 20;
                break;
                // UnlockBean �޼� ����
            case Achive.UnlockBean:
                // ���ӽð� ������ ��Ƴ����� ����.
                isAchive = GameManager.instance.gameTime == GameManager.instance.maxGameTime;
                break;
        }

        // �޼��ߴµ� �޼��� ������ Value�� 0�̿��ٸ� (���� �رݵ��� �ʾҴµ� �޼����� ��)
        if (isAchive && PlayerPrefs.GetInt(achive.ToString()) == 0)
        {
            // �ر��� ǥ�÷� �޼��� ������ Key���� Value�� 1�� ����.
            PlayerPrefs.SetInt(achive.ToString(), 1);

            // �˸� UI����
            for (int index = 0; index < uiNotice.transform.childCount; index++)
            {
                // ������ ���� index�� ���� index�� ������ �ڽ� ������Ʈ �� �ش� �ε����� ������Ʈ Ȱ��ȭ.
                bool isActive = index == (int)achive;
                uiNotice.transform.GetChild(index).gameObject.SetActive(isActive);
            }

            StartCoroutine(NoticeRoutine());
        }
    }

    // �˸�â �ڷ�ƾ
    IEnumerator NoticeRoutine()
    {   
        // �˸�â Ȱ��ȭ
        uiNotice.SetActive(true);
        // �˸�â�� �´� ����  ���
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);
        // ������ �ʱ�ȭ�� 5�� ���
        yield return wait;
        // �˸�â ��Ȱ��ȭ
        uiNotice.SetActive(false);
    }
}
