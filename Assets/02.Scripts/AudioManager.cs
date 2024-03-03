using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    /// <summary>
    ///  BGM과 효과음 담당 처리하는 스크립트
    /// </summary>
    public static AudioManager instance;

    [Header("#BGM")]
    // BGM을 실행시켜줄 AudioClip
    public AudioClip bgmClip;
    // BGM 볼륨
    public float bgmVolume;
    // BGM (source)
    AudioSource bgmPlayer;
    // BGM 주파수가 낮은 음역대  차단 ( 효과음 나올 때 배경음에 효과 주기 위한 변수 )
    AudioHighPassFilter bgmEffect;

    [Header("#SFX")]
    // 효과음 배열
    public AudioClip[] sfxClips;
    // 효과음 볼륨
    public float sfxVolume;
    // 효과음 AudioSource 개수 ( 많을수록 사운드 풍부 _
    // 너무 많으면 시끄러울 수 있는 단점이 있어서 동시에 울리는 효과음을 제한 하는 목적이기도 함.)
    public int channels;
    // AudioSource 담을 배열
    AudioSource[] sfxPlayers;
    // 가장 최근 플레이 된 오디오를 찾기 위한 변수
    int channelIndex;

    public enum Sfx { Dead, Hit, LevelUp = 3, Lose, Melee, Range = 7, Select, Win }

    void Awake()
    {
        instance = this;
        Init();
    }

    void Init()
    {
        // 배경음 플레이어 초기화
        // "BgmPlayer"라는 이름의 BGM관리할 오브젝트 생성.
        GameObject bgmObject = new GameObject("BgmPlayer");
        // BGM오브젝트를 이 스크립트가 있는 오브젝트의 자식으로 설정.
        bgmObject.transform.parent = transform;
        // AudiorSourc 컴포넌트 추가
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        // 바로 실행 = false
        bgmPlayer.playOnAwake = false;
        // 반복 = true
        bgmPlayer.loop = true;
        // 내가 입력한 볼륨의 크기로 BGM 볼륨 설정.
        bgmPlayer.volume = bgmVolume;
        // BGM clip 세팅
        bgmPlayer.clip = bgmClip;
        // BGM 에 효과 줄 바이패스필터 변수 초기화
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();

        // 효과음 플레이어 초기화
        // "SfxPlayer"라는 이름의 효과음 관리할 오브젝트 생성.
        GameObject sfxObject = new GameObject("SfxPlayer");
        // 효과음 오브젝트를 이 스크립트가 있는 오브젝트의 자식으로 설정.
        sfxObject.transform.parent = transform;
        // 효과음 오디오 소스 배열을 내가 입력한 개수의 크기로 초기화.
        sfxPlayers = new AudioSource[channels];

        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            // 입력한 수만큼 AudioSource 생성 및 초기화
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            // 효과음이므로 PlayOnAwake는 false
            sfxPlayers[index].playOnAwake = false;
            //바이패스의 영향을 받지 않도록 하기 위해서 true
            sfxPlayers[index].bypassListenerEffects = true;
            // 내가 설정한 볼륨으로
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
        // BGM 바이패스 효과음
        bgmEffect.enabled = isPlay;
    }

    public void PlaySfx(Sfx sfx)
    {
        // 효과음 AudioSource 개수만큼 순회, 효과음 소스를 순회하며 소리를 실행중인지 판단하고,
        // 실행중이 아닐 때 해당 AudioSource에서 효과음 실행하는 로직.
        for (int index = 0; index < sfxPlayers.Length; index++)
        { 
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;

            //해당 인덱스의 AudioSource가 clip을 실행중이라면 해당 반복문을 건너뛴다.
            if (sfxPlayers[loopIndex].isPlaying)
                continue;

            // Hit과 근접공격의 사운드를 2개씩 두어서 랜덤한 소리가 나도록 하는 로직.
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

