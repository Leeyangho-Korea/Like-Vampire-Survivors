using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;
    public float levelTime;

    int level;
    float timer;

    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
        levelTime = GameManager.instance.maxGameTime / spawnData.Length;
    }

    void Update()
    {    
        // 게임 진행중이 아니라면 실행 안함.
        if (!GameManager.instance.isLive)
            return;

        // 몬스터 젠 시간.
        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / levelTime), spawnData.Length - 1);

        // 몬스터 젠 시간이 현재 스폰할 몬스터 주기보다 클 때
        if (timer > spawnData[level].spawnTime)
        {
            // 몬스터 젠 시간 초기화
            timer = 0;
            // 몬스터 스폰
            Spawn();
        }
    }

    void Spawn()
    {
        // pool 함수 실행 0이 몬스터
        GameObject enemy = GameManager.instance.pool.Get(0);
        // 생성한 몬스터의 위치는 스폰되는 여러 스팟 중 한 곳 랜덤으로 초기화.
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        // 생성한 몬스터를 종류에 따라 초기화
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
