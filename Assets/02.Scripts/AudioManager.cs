using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    /// <summary>
    ///  BGM�� ȿ���� ��� ó���ϴ� ��ũ��Ʈ
    /// </summary>
    public static AudioManager instance;

    [Header("#BGM")]
    // BGM�� ��������� AudioClip
    public AudioClip bgmClip;
    // BGM ����
    public float bgmVolume;
    // BGM (source)
    AudioSource bgmPlayer;
    // BGM ���ļ��� ���� ������  ���� ( ȿ���� ���� �� ������� ȿ�� �ֱ� ���� ���� )
    AudioHighPassFilter bgmEffect;

    [Header("#SFX")]
    // ȿ���� �迭
    public AudioClip[] sfxClips;
    // ȿ���� ����
    public float sfxVolume;
    // ȿ���� AudioSource ���� ( �������� ���� ǳ�� _
    // �ʹ� ������ �ò����� �� �ִ� ������ �־ ���ÿ� �︮�� ȿ������ ���� �ϴ� �����̱⵵ ��.)
    public int channels;
    // AudioSource ���� �迭
    AudioSource[] sfxPlayers;
    // ���� �ֱ� �÷��� �� ������� ã�� ���� ����
    int channelIndex;

    public enum Sfx { Dead, Hit, LevelUp = 3, Lose, Melee, Range = 7, Select, Win }

    void Awake()
    {
        instance = this;
        Init();
    }

    void Init()
    {
        // ����� �÷��̾� �ʱ�ȭ
        // "BgmPlayer"��� �̸��� BGM������ ������Ʈ ����.
        GameObject bgmObject = new GameObject("BgmPlayer");
        // BGM������Ʈ�� �� ��ũ��Ʈ�� �ִ� ������Ʈ�� �ڽ����� ����.
        bgmObject.transform.parent = transform;
        // AudiorSourc ������Ʈ �߰�
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        // �ٷ� ���� = false
        bgmPlayer.playOnAwake = false;
        // �ݺ� = true
        bgmPlayer.loop = true;
        // ���� �Է��� ������ ũ��� BGM ���� ����.
        bgmPlayer.volume = bgmVolume;
        // BGM clip ����
        bgmPlayer.clip = bgmClip;
        // BGM �� ȿ�� �� �����н����� ���� �ʱ�ȭ
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();

        // ȿ���� �÷��̾� �ʱ�ȭ
        // "SfxPlayer"��� �̸��� ȿ���� ������ ������Ʈ ����.
        GameObject sfxObject = new GameObject("SfxPlayer");
        // ȿ���� ������Ʈ�� �� ��ũ��Ʈ�� �ִ� ������Ʈ�� �ڽ����� ����.
        sfxObject.transform.parent = transform;
        // ȿ���� ����� �ҽ� �迭�� ���� �Է��� ������ ũ��� �ʱ�ȭ.
        sfxPlayers = new AudioSource[channels];

        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            // �Է��� ����ŭ AudioSource ���� �� �ʱ�ȭ
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            // ȿ�����̹Ƿ� PlayOnAwake�� false
            sfxPlayers[index].playOnAwake = false;
            //�����н��� ������ ���� �ʵ��� �ϱ� ���ؼ� true
            sfxPlayers[index].bypassListenerEffects = true;
            // ���� ������ ��������
            sfxPlayers[index].volume = sfxVolume;
        }
    }

    public void PlayBgm(bool isPlay)
    {
        if (isPlay)
        {
            bgmPlayer.Play();
        }
        else
        {
            bgmPlayer.Stop();
        }
    }

    public void EffectBgm(bool isPlay)
    {
        // BGM �����н� ȿ����
        bgmEffect.enabled = isPlay;
    }

    public void PlaySfx(Sfx sfx)
    {
        // ȿ���� AudioSource ������ŭ ��ȸ, ȿ���� �ҽ��� ��ȸ�ϸ� �Ҹ��� ���������� �Ǵ��ϰ�,
        // �������� �ƴ� �� �ش� AudioSource���� ȿ���� �����ϴ� ����.
        for (int index = 0; index < sfxPlayers.Length; index++)
        { 
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;

            //�ش� �ε����� AudioSource�� clip�� �������̶�� �ش� �ݺ����� �ǳʶڴ�.
            if (sfxPlayers[loopIndex].isPlaying)
                continue;

            // Hit�� ���������� ���带 2���� �ξ ������ �Ҹ��� ������ �ϴ� ����.
            int ranIndex = 0;
            if (sfx == Sfx.Hit || sfx == Sfx.Melee)
            {
                ranIndex = Random.Range(0, 2);
            }

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx + ranIndex];
            sfxPlayers[loopIndex].Play();
            break;
        }
    }
}

