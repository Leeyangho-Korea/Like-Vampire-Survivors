using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs;

    List<GameObject>[] pools;

    void Awake()
    {
        // ���� ������Ʈ ����Ʈ pools�� prefabs�迭 ���̸�ŭ �ʱ�ȭ
        pools = new List<GameObject>[prefabs.Length];

        // pools�� ���̸�ŭ ��ȸ�ϸ� ���ο� ���� ������Ʈ ����
        for (int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        // pools[index]�� �ִ� ������Ʈ�� Ž��
        foreach (GameObject item in pools[index])
        {
            // ���� �ش� ������Ʈ�� ��Ȱ��ȭ ���¶��
            if (!item.activeSelf)
            {
                // ��Ȱ��ȭ �� ������Ʈ�� select ���������� ����.
                select = item;
                // �ش� ������Ʈ�� Ȱ��ȭ�� ����.
                select.SetActive(true);
                break;
            }
        }

        // ���� select ������ ���ٸ� ( Ǯ�� �����Ǵ� ������Ʈ�� ��� Ȱ��ȭ ���¶�� )
        if (!select)
        {
            // prefabs�� ���� ����.
            select = Instantiate(prefabs[index], transform);
            // pools ����Ʈ�� ������ ������Ʈ �߰�.
            pools[index].Add(select);
        }

        return select;
    }
}
