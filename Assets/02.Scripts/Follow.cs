using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  �÷��̾� ü�¹ٸ� �÷��̾� ����ٴϵ��� ���ִ� ��ũ��Ʈ
/// </summary>
    public class Follow : MonoBehaviour
    {
        RectTransform rect;

        void Awake()
        {
            rect = GetComponent<RectTransform>();
        }

        void FixedUpdate()
        {
        // ���� ��ǥ�� ��ũ�� ��ǥ�� �������ִ� �Լ��� ����� World��ǥ�� ĳ���Ϳ�
        // Screen���� ǥ���Ǵ� UI�� ��ǥ�� ��Ʈ�� ��.
            rect.position = Camera.main.WorldToScreenPoint(GameManager.instance.player.transform.position);
        }
    }

