using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardEffect
{
    QuickReload,
    QuickShot,
    Huge,
    Defender
}

[CreateAssetMenu(fileName = "CardData", menuName = "CardSystem/Card Data", order = 0)]
public class CardDataSO : ScriptableObject
{
    public string cardName;         // ī�� �̸�
    [TextArea] public string description;  // ���� (�� �ؽ�Ʈ ����)
    public string effectSummary;    // ������ ��ġ ���

    public Sprite frontImage;       // ī�� �ո鿡 ǥ���� �̹���
    public CardEffect effectType;   // ���� ����� ȿ��
}
