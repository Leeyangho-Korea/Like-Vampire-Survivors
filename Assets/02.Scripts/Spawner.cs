using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 랜덤위치에 몬스터 스폰 시키는 스크립트
/// </summary>
public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;

    float timer;
    private float zenRate = 0.2f; // 몬스터 소환 주기

    int level;

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>(); // 자기자신의 트랜스폼도 가져옴.
    }
    void Update()
    {
        timer += Time.deltaTime;
        level = Mathf.Min( Mathf.FloorToInt(GameManager.instance.gameTime / 10f), spawnData.Length -1);
        
        // 레벨이 0면 0.5초마다 아니면 0.2초마다 젠
        if(timer > spawnData[level].spawnTime)
        {
            timer = 0f;
            SpawnMonster();
        }
    }

    void SpawnMonster()
    {
        // 랜덤 몬스터 생성
        GameObject enemy =  GameManager.instance.pool.Get(0);
        // 랜덤 위치로
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
    }
}

[System.Serializable]
public class SpawnData
{
    public float spawnTime;
    public int spriteType;
    public int health;
    public float speed;
}