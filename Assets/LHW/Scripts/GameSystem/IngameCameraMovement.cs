using ExitGames.Client.Photon.StructWrapping;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class IngameCameraMovement : MonoBehaviour
{
    [SerializeField] private bool isRoundOver = false;
    [SerializeField] private bool isRoundSetOver = false;

    [SerializeField] private float moveDuration = 1f;

    // ���ӸŴ����� ��� �ϴ� Update�� ó�� �� �׽�Ʈ
    // �Ŀ� �̺�Ʈ�� ���� �� ����� ���� ���θ� �޾ƿ��� ��� �����

    private RandomMapPresetCreator creator;
    private Camera mainCamera;
    private Vector2 startPosition;
    private Vector2 targetPosition;

    private Coroutine cameraCoroutine;

    private void OnEnable()
    {
        creator = GetComponent<RandomMapPresetCreator>();
        mainCamera = Camera.main;
        startPosition = Camera.main.transform.position;
    }

    private void Update()
    {
        if(isRoundSetOver)
        {
            SceneChange();
        }
        else if (isRoundOver)
        {
            IngameCameraMove();
        }
    }

    private void IngameCameraMove()
    {
        float offset = creator.GetTransformOffset();
        targetPosition = startPosition + new Vector2(offset, 0);
        cameraCoroutine = StartCoroutine(MoveCamera());

        isRoundOver = false;
    }

    IEnumerator MoveCamera()
    {
        float elaspedTime = 0f;

        while(elaspedTime < moveDuration)
        {
            elaspedTime += Time.deltaTime;
            float t = elaspedTime/moveDuration;

            mainCamera.transform.position = Vector2.Lerp(startPosition, targetPosition, Mathf.SmoothStep(0, 1, t));
            yield return null;
        }
        
        mainCamera.transform.position = targetPosition;
        startPosition = Camera.main.transform.position;
        cameraCoroutine = null;
    }

    private void SceneChange()
    {
        // �� �ε� - ��ȣ�� �񵿱� �ε� ���� �������?
    }
}
