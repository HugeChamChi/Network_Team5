using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickReload : CardEffect
{
    private void Awake()
    {
        cardName = "Quick Reload";
        description = "������ �ӵ� 70% ����";
    }

    public override void ApplyShotEffect(PlayerStats playerStats)
    {
        playerStats.reloadSpeed *= 0.3f;
    }

    public override void ApplyStatusEffect(PlayerStats playerStats)
    {
        
    }


}
