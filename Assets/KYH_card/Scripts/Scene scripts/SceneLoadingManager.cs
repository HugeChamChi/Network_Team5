using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


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

    public void LoadSceneAsync(string sceneName)
    {
        StartCoroutine(LoadAsyncRoutine(sceneName));
    }

    /// <summary>
    /// �񵿱� �ε� �ڷ�ƾ
    /// </summary> 

    private IEnumerator LoadAsyncRoutine(string sceneName)
    {
        float minDisplayTime = 1.5f; // �ε�ȭ�� �ּ� ���� �ð�
        float timer = 0f;

        // 1. �ε� UI ����
        if (LoadingUIManager.Instance != null)
        {
            LoadingUIManager.Instance.Show();
        }
        // 2. �� �񵿱� �ε�
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        while (op.progress < 0.9f)
        {
            if (LoadingUIManager.Instance != null)
                LoadingUIManager.Instance.UpdateProgress(op.progress / 0.9f);
            yield return null;
        }

        // 3. ���� �����
        if (LoadingUIManager.Instance != null)
        {
            LoadingUIManager.Instance.UpdateProgress(1f);
        }

        while (timer < minDisplayTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        // 4. �ε� UI �����
        if (LoadingUIManager.Instance != null)
        {
            LoadingUIManager.Instance.Hide(); // 
        }
        // 5. �� ��ȯ ���
        op.allowSceneActivation = true;
        Debug.Log($"���� ������ �Ѿ. �̵� �� �� �̸� {sceneName}");
    }
}
