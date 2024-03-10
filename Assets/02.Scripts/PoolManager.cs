using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs;

    List<GameObject>[] pools;

    void Awake()
    {
        // 게임 오브젝트 리스트 pools를 prefabs배열 길이만큼 초기화
        pools = new List<GameObject>[prefabs.Length];

        // pools의 길이만큼 순회하며 새로운 게임 오브젝트 생성
        for (int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        // pools[index]에 있는 오브젝트를 탐색
        foreach (GameObject item in pools[index])
        {
            // 만약 해당 오브젝트가 비활성화 상태라면
            if (!item.activeSelf)
            {
                // 비활성화 된 오브젝트를 select 지역변수에 저장.
                select = item;
                // 해당 오브젝트를 활성화로 변경.
                select.SetActive(true);
                break;
            }
        }

        // 만약 select 변수가 없다면 ( 풀링 관리되는 오브젝트가 모두 활성화 상태라면 )
        if (!select)
        {
            // prefabs를 새로 생성.
            select = Instantiate(prefabs[index], transform);
            // pools 리스트에 생성한 오브젝트 추가.
            pools[index].Add(select);
        }

        return select;
    }
}
