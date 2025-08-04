using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defender : CardEffect
{
    private void Awake()
    {
        cardName = "Defender";
        description = "��� ��Ÿ�� 30% ����, ü�� 30% ����";
    }

    public override void ApplyShotEffect(PlayerStats playerStats)
    {
        
    }

    public override void ApplyStatusEffect(PlayerStats playerStats)
    {
        playerStats.blockCooldown *= 0.7f;
        playerStats.IncreaseMaxHealth(0.3f);
    }
}
