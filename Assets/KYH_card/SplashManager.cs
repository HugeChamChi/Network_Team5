using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashManager : MonoBehaviour
{
    [SerializeField] private Image logoImage;
    [SerializeField] private GameObject logo;
    [SerializeField] private float fadeInTime = 1f;
    [SerializeField] private float stayTime = 1.5f;
    [SerializeField] private float fadeOutTime = 1f;
    [SerializeField] private string nextSceneName = "LoginScene";

    private void Start()
    {
        logo.SetActive(true);
        // �ʱ�ȭ
        logoImage.color = new Color(1, 1, 1, 0);
        logoImage.rectTransform.localScale = Vector3.one;
        logoImage.rectTransform.localEulerAngles = Vector3.zero;

        Sequence seq = DOTween.Sequence();

        // ���ÿ� ���̵��� + ȸ�� + Ȯ��
        seq.Append(logoImage.DOFade(1, fadeInTime));

        seq.Join(logoImage.rectTransform
            .DORotate(new Vector3(0, 0, 360f), fadeInTime, RotateMode.FastBeyond360)
            .SetEase(Ease.OutCubic));

        seq.Join(logoImage.rectTransform
            .DOScale(4f, fadeInTime)
            .SetEase(Ease.OutBack));

        // �ణ ����
        seq.AppendInterval(stayTime);

        // ���̵� �ƿ�
        seq.Append(logoImage.DOFade(0, fadeOutTime));

        // ������ ���� ��
        seq.OnComplete(() => SceneManager.LoadScene(nextSceneName));
    }
}
