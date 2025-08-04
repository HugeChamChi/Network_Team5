using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Huge : CardEffect
{
    private void Awake()
    {
        cardName = "Huge";
        description = "�ִ� ü�� 80% ����, ĳ���� ũ�� 30% ����";
    }

    public override void ApplyStatusEffect(PlayerStats playerStats)
    {
        playerStats.IncreaseMaxHealth(0.8f);
        playerStats.ChangeScale(1.3f);
    }

    public override void ApplyShotEffect(PlayerStats playerStats)
    {
        
    }
}
