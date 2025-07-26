using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingBootstrapper : MonoBehaviour
{
    [SerializeField] private GameObject loadingCanvasPrefab;

    private void Awake()
    {
        // Instance�� ���� ���� ����
        if (LoadingUIManager.Instance == null)
        {
            GameObject obj = Instantiate(loadingCanvasPrefab);
            DontDestroyOnLoad(obj);
        }
    }
}
