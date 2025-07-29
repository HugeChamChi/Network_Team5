using System.Collections;
using UnityEngine;

public class IngameCameraMovement : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] IngameUIManager gameUIManager;
    [SerializeField] RoundOverPanelController roundUIController;    
    
    // ���� ���� ����
    [SerializeField] private bool isRoundOver = false;
    // ���� ��Ʈ ����
    [SerializeField] private bool isRoundSetOver = false;

    [Header("Camera Delay")]
    // ī�޶� ������ ��������
    [SerializeField] private float initialDelay = 2f;
    // ī�޶� ������ �ĵ�����
    [SerializeField] private float postDelay = 0.4f;
    public float Postdelay { get { return postDelay; } }

    [Header("Camera Movement Offset")]
    // ī�޶� ���� �̵� �ð�
    [SerializeField] private float moveLeftDuration = 0.1f;
    // ī�޶� ���� �̵� ������
    [SerializeField] private float moveLeftDistance = 2f;
    // ī�޶� ������ �̵� �ð�
    [SerializeField] private float moveRightDuration = 0.5f;


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

        TestIngameManager.OnRoundOver += IngameCameraMove;
        TestIngameManager.OnGameSetOver += SceneChange;
    }

    private void OnDisable()
    {
        TestIngameManager.OnRoundOver -= IngameCameraMove;
        TestIngameManager.OnGameSetOver -= SceneChange;
    }

    /// <summary>
    /// ī�޶� ������ ����
    /// </summary>
    public void IngameCameraMove()
    {
        float offset = creator.GetTransformOffset();
        targetPosition = startPosition + new Vector2(offset, 0);
        cameraCoroutine = StartCoroutine(MoveCamera());

        TestIngameManager.Instance.RoundStart();
    }

    /// <summary>
    /// ī�޶� ������ �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveCamera()
    {
        WaitForSeconds initialCameraDelay = new WaitForSeconds(initialDelay);
        WaitForSeconds postCameraDelay = new WaitForSeconds(postDelay);

        yield return initialCameraDelay;

        // ī�޶� �������� �̵�
        float elapsedTime = 0f;

        while (elapsedTime < moveLeftDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime/moveLeftDuration;
            
            mainCamera.transform.position = Vector2.Lerp(startPosition, startPosition - new Vector2(moveLeftDistance, 0), Mathf.SmoothStep(0, 1, t));
            yield return null;
        }
        mainCamera.transform.position = startPosition - new Vector2(moveLeftDistance, 0);
        startPosition = Camera.main.transform.position;

        // ī�޶� ���������� �̵�
        float elaspedTime = 0f;

        while(elaspedTime < moveRightDuration)
        {
            elaspedTime += Time.deltaTime;
            float t = elaspedTime/moveRightDuration;

            mainCamera.transform.position = Vector2.Lerp(startPosition, targetPosition, Mathf.SmoothStep(0, 1, t));
            yield return null;
        }
        yield return postCameraDelay;        

        roundUIController.ShrinkImage();
        mainCamera.transform.position = targetPosition;
        startPosition = Camera.main.transform.position;
        TestIngameManager.Instance.GameSetStart();
        if (TestIngameManager.Instance.IsGameOver)
        {
            gameUIManager.RestartPanelShow();
        }
        
        cameraCoroutine = null;
    }

    private void SceneChange()
    {
        gameUIManager.HideRoundOverPanel();
        // �� �ε� - ��ȣ�� �񵿱� �ε� ���� �������?
        Debug.Log("�� ��ȯ");        
    }
}