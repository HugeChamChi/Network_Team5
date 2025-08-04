using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuickShot : CardEffect
{
    private void Awake()
    {
        cardName = "Quick Shot";
        description = "����ü �ӵ� 1.5��, ������ �ӵ� 25% ����";
    }

    public override void ApplyShotEffect(PlayerStats playerStats)
    {
        playerStats.projectileSpeed *= 1.5f;
        playerStats.reloadSpeed *= 1.25f;
    }

    public override void ApplyStatusEffect(PlayerStats playerStats)
    {
        
    }
}
