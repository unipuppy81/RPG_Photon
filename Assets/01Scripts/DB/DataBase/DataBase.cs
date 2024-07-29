using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class DataBase : ScriptableObject
{
	public List<CharacterStatsDBEntity> Character; // ���� ��Ʈ �̸�
    public List<EnemyStatsDBEntity> Enemy;

    // Ư�� Ÿ���� ĳ���� ������ ������Ʈ�ϴ� �޼���
    public void UpdateCharacterStats(string type, float maxHp, float atk, float def, float wspeed, float rspeed)
    {
        // ����Ʈ�� ��ȸ�ϸ鼭 Ÿ�Կ� �´� ĳ���� ������ ã�� ������Ʈ�մϴ�.
        for (int i = 0; i < Character.Count; ++i)
        {
            if (Character[i].Type == type)
            {
                Character[i].Maxhp = maxHp;
                Character[i].Atk = atk;
                Character[i].Def = def;
                Character[i].Wspeed = wspeed;
                Character[i].Rspeed = rspeed;

                return;
            }
        }
    }

    // Ư�� Ÿ���� �� ������ ������Ʈ�ϴ� �޼���
    public void UpdateEnemyStats(float maxHp, float spd, float atk, float def, float healAmount, float Radius)
    {
        for (int i = 0; i < Enemy.Count; ++i)
        {
            Enemy[i].Maxhp = maxHp;
            Enemy[i].Speed = spd;
            Enemy[i].Atk = atk;
            Enemy[i].Def = def;
            Enemy[i].Healamount = healAmount;
            Enemy[i].Radius = Radius;

            return;

        }
    }
}
