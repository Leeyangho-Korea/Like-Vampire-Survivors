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
        // ���� �������� �ƴ϶�� ���� ����.
        if (!GameManager.instance.isLive)
            return;

        // ���� �� �ð�.
        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / levelTime), spawnData.Length - 1);

        // ���� �� �ð��� ���� ������ ���� �ֱ⺸�� Ŭ ��
        if (timer > spawnData[level].spawnTime)
        {
            // ���� �� �ð� �ʱ�ȭ
            timer = 0;
            // ���� ����
            Spawn();
        }
    }

    void Spawn()
    {
        // pool �Լ� ���� 0�� ����
        GameObject enemy = GameManager.instance.pool.Get(0);
        // ������ ������ ��ġ�� �����Ǵ� ���� ���� �� �� �� �������� �ʱ�ȭ.
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        // ������ ���͸� ������ ���� �ʱ�ȭ
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
