using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// UI ������ �����ؼ� �ҷ��� �����ϴ� ��ũ��Ʈ
/// </summary>
public class LoadingUIManager : MonoBehaviour
{
    public static LoadingUIManager Instance;

    [Header("UI ����")]
    [SerializeField] private GameObject loadingPanel;           // ��ü �г�
    [SerializeField] private Slider progressBar;                // ����� �����̴�  
    [SerializeField] private TextMeshProUGUI progressText;      // �ۼ�Ʈ ǥ�� �ؽ�Ʈ

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); //  �� �̵��� ����
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Show()
    {
        if (loadingPanel != null)
        {
            Debug.Log("�ε� �г� Ȱ��ȭ �Ǿ� ��µ�");
            loadingPanel.SetActive(true);
        }

        if (progressBar != null)
        {
            progressBar.value = 0f; // �ʱ�ȭ
        }
        if (progressText != null)
        {
            progressText.text = "Loading...0%"; // �ؽ�Ʈ�� �ʱ�ȭ
        }
        // ��� �ִϸ��̼� �ʱ�ȭ
        CardAnimator animator = GetComponentInChildren<CardAnimator>();
        if (animator != null)
        {
            animator.RestartAnimation(); 
        }
    }

    public void Hide()
    {
        if (loadingPanel != null)
        {
            loadingPanel.SetActive(false);
        }
    }

    public void UpdateProgress(float progress)
    {
        if (progressBar != null)
        {
            progressBar.DOValue(progress, 0.3f).SetEase(Ease.OutQuad);
        }

        

        if (progressText != null)
        {
            progressText.text = $"Loading...{Mathf.RoundToInt(progress * 100)}%";
        }

        // ���α׷����� 100%�� �������� ��� �ڵ����� �г� ����
        if (progress >= 1f && loadingPanel != null && loadingPanel.activeSelf)
        {
            Hide();
        }
    }

    
}
