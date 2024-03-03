using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 몬스터와 무기의 풀링을 관리하는 스크립트
/// </summary>
public class PoolManager : MonoBehaviour
{
    // 프리팹 보관할 변수
    public GameObject[] enemyPrefabs;

    // 풀 담당 하는 리스트들
    List<GameObject>[] pools;

    private void Awake()
    {
        pools = new List<GameObject>[enemyPrefabs.Length];
        for (int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>();
        }
        Debug.Log(pools.Length);
    }

    public GameObject Get(int index)
    {
        GameObject select = null;
        // 선택한 풀의 놀고 있는 게임오브젝트 접근
        // 발견하면 select 변수에 할당.

        foreach (GameObject item in pools[index])
        {
            // 만약 비활성화 된 오브젝트 있으면 다시 활성화
            if (!item.activeSelf)
            {
                // 발견하면 select 변수에 할당
                select = item;
                select.SetActive(true);
                break;
            }
        }
        // 비활성화 된 오브젝트가 없다면 ? (== 모든 오브젝트들이 활성화돼서 활동중이라면)
        // 새로 몬스터 생성
        if (!select)
        {
            select = Instantiate(enemyPrefabs[index], transform);
            pools[index].Add((select));
        }
        return select;
    }
}