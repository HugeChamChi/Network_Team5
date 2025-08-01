using UnityEngine;
using UnityEngine.Splines;

public class CardSceneCharacterRightArm : MonoBehaviour
{
    [SerializeField] Transform shoulderTransform;  // �� ������
    [SerializeField] Transform currentHandTarget;         // ���� ���� ��ǥ ��ġ

    [SerializeField] Transform[] cardsTransforms;

    Vector3 localShoulderPos;
    Vector3 localHandPos;

    private SplineContainer splineContainer;
    private Spline spline;
    private BezierKnot shoulderKnot;
    private BezierKnot handKnot;
    private float pointerBendAmout = -4f;
    int targetCardNum = 3;

    private void Start()
    {
        splineContainer = GetComponent<SplineContainer>();
        spline = splineContainer.Spline;
    }

    void Update()
    {
        if (currentHandTarget == null || shoulderTransform == null) return;        

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            targetCardNum = SelectCard(3);
            Debug.Log(targetCardNum);   
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            targetCardNum = SelectCard(4);
            Debug.Log(targetCardNum);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            targetCardNum = SelectCard(5);
            Debug.Log(targetCardNum);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            targetCardNum = SelectCard(6);
            Debug.Log(targetCardNum);
        }

        shoulderKnot.Position = shoulderTransform.position;
        currentHandTarget.position = Vector3.Lerp(currentHandTarget.position, cardsTransforms[targetCardNum].position, Time.deltaTime * 10f);
        handKnot.Position = splineContainer.transform.InverseTransformPoint(currentHandTarget.position);

        spline.SetKnot(0, shoulderKnot);
        spline.SetKnot(2, handKnot);

        Vector3 mid = Vector3.Lerp(shoulderKnot.Position, handKnot.Position, 0.5f);
        mid += new Vector3(1, -1) * 1f; // �������� ��¦ �־����� ����
        spline.SetKnot(1, new BezierKnot(mid));
    }

    private int SelectCard(int cardNum)
    {
        if (cardNum == 3) return 0;
        else if (cardNum == 4) return 1;
        else if (cardNum == 5) return 2;
        else return 3;
    }
}
