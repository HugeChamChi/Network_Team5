using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �񵿱� �ε� �����ϴ� �Ŵ��� ��ũ��Ʈ
/// </summary>
public class SceneLoadingManager : MonoBehaviour
{
    public static SceneLoadingManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �ÿ��� ����
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// �ܺο��� �� �ε� ��û
    /// </summary>
    private bool isSceneReadyToActivate;
    private AsyncOperation currentOperation;
    private bool allowSceneActivationExternally = false;

    public void LoadSceneAsync(string sceneName)
    {
        isSceneReadyToActivate = false;
        StartCoroutine(LoadAsyncRoutine(sceneName));
    }

    public void AllowSceneActivation()
    {
        isSceneReadyToActivate = true;
    }

    /// <summary>
    /// �񵿱� �ε� �ڷ�ƾ
    /// </summary> 

    private IEnumerator LoadAsyncRoutine(string sceneName)
    {
        float minDisplayTime = 1.5f;
        float timer = 0f;

        LoadingUIManager.Instance?.Show();

        currentOperation = SceneManager.LoadSceneAsync(sceneName);
        currentOperation.allowSceneActivation = false;

        while (currentOperation.progress < 0.9f)
        {
            LoadingUIManager.Instance?.UpdateProgress(currentOperation.progress / 0.9f);
            timer += Time.deltaTime;
            yield return null;
        }

        // 90% ���� ��, 1.5�� ���� �� ���
        LoadingUIManager.Instance?.UpdateProgress(1f);

        while (timer < minDisplayTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        // ���⼭ ī�� ���ÿ��� �Ѿ�� ������ ���
        yield return new WaitUntil(() => isSceneReadyToActivate);

        LoadingUIManager.Instance?.Hide();
        currentOperation.allowSceneActivation = true;

        Debug.Log($"�� ��ȯ �Ϸ�: {sceneName}");
    }
}
