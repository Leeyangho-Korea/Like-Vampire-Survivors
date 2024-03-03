using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� �����ۿ� ���� ȿ�� ���� ��ũ��Ʈ
/// </summary>
public class Gear : MonoBehaviour
{
    public ItemData.ItemType type;
    public float rate;

    public void Init(ItemData data)
    {
        // ���� ����
        name = "Gear " + data.itemId;
        transform.parent = GameManager.instance.player.transform;
        transform.localPosition = Vector3.zero;

        // ������ ���� ������ �ִ� Ÿ�԰� �������� �ʱ�ȭ.
        type = data.itemType;
        rate = data.damages[0];
        //  ���� ����.
        ApplyGear();
    }

    public void LevelUp(float rate)
    {
        this.rate = rate;
        ApplyGear();
    }

    void ApplyGear()
    {
        switch (type)
        {
            // Glove Ÿ���ϰ��
            case ItemData.ItemType.Glove:
                RateUp();
                break;
            // Sho Ÿ���� ���
            case ItemData.ItemType.Shoe:
                SpeedUp();
                break;
        }
    }

    void RateUp()
    {
        // ���� ������ ���� ������ ��������
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        foreach (Weapon weapon in weapons)
        {
            switch (weapon.id)
            {
                // ���� id �� 0�� ��� (��������)
                case 0:
                    // 1.5�� �ӵ� ����
                    float speed = 150 * Character.WeaponSpeed;
                    weapon.speed = speed + (speed * rate);
                    break;
                    // ��  �� �� ��� (�Ѿ�)
                default:
                    // �Ѿ� ���� �ӵ� ���� ( �Ѿ� �߻� �ֱ⸦ ª�� )
                    speed = 0.5f * Character.WeaponRate;
                    weapon.speed = speed * (1f - rate);
                    break;
            }
        }
    }

    // �ӵ� ����
    void SpeedUp()
    {
        // ���� ĳ���� �ӵ� ����
        float speed = 3 * Character.Speed;
        GameManager.instance.player.speed = speed + speed * rate;
    }
}
