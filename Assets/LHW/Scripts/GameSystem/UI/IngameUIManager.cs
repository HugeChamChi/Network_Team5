using System.Collections;
using UnityEngine;

/// <summary>
/// ���� ���� �гΰ� ���� ����� �г��� ������
/// </summary>
public class IngameUIManager : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] RandomMapPresetCreator creator;

    [Header("Panels")]
    [SerializeField] GameObject cardSelectPanel;
    [SerializeField] GameObject roundOverPanel;
    [SerializeField] GameObject gameRestartPanel;

    [Header("Offset")]
    [Tooltip("���� ���� �г� ���� �ð�")]
    [SerializeField] private float roundOverPanelDuration = 3.5f;
    public float RoundOverPanelDuration { get { return roundOverPanelDuration; } }
    [Tooltip("���� ���� �� ����� �г� Ȱ��ȭ ������")]
    [SerializeField] private float restartPanelShowDelay = 3.5f;

    Coroutine ROPanelCoroutine;
    Coroutine restartPanelCoroutine;

    private void OnEnable()
    {
        TestIngameManager.OnRoundOver += RoundOverPanelShow;
        TestIngameManager.OnGameOver += RestartPanelShow;
    }

    private void OnDisable()
    {
        TestIngameManager.OnRoundOver -= RoundOverPanelShow;
        TestIngameManager.OnGameOver -= RestartPanelShow;
    }

    private void RoundOverPanelShow()
    {
        ROPanelCoroutine = StartCoroutine(RoundOverPanelCoroutine());
    }

    public void HideRoundOverPanel()
    {
        roundOverPanel.SetActive(false);
    }

    public void RestartPanelShow()
    {
        restartPanelCoroutine = StartCoroutine(RestartPanelCoroutine());
    }

    public void HideRestartPanel()
    {
        gameRestartPanel.SetActive(false);
    }

    /// <summary>
    /// ���� ���� �г��� Ȱ��ȭ�ϰ� ���ӽð���ŭ ������ ���� �ٽ� ��Ȱ��ȭ�ϴ� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator RoundOverPanelCoroutine()
    {
        WaitForSeconds delay = new WaitForSeconds(roundOverPanelDuration);
        roundOverPanel.SetActive(true);

        yield return delay;
        HideRoundOverPanel();
        TestIngameManager.Instance.RoundStart();
        if (TestIngameManager.Instance.IsGameSetOver)
        {
            Debug.Log("�� ��Ʈ ����");
            creator.MapUpdate(TestIngameManager.Instance.CurrentGameRound);
            TestIngameManager.Instance.GameSetStart();
        }

        ROPanelCoroutine = null;
    }

    /// <summary>
    /// ���� ����� �г��� ������ �ð� ���Ŀ� Ȱ��ȭ�ϴ� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator RestartPanelCoroutine()
    {
        yield return new WaitForSeconds(restartPanelShowDelay);

        gameRestartPanel.SetActive(true);
        restartPanelCoroutine = null;
    }
}