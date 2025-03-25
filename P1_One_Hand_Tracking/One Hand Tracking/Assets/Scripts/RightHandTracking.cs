using UnityEngine;

public class RightHandTracking : MonoBehaviour
{
    [SerializeField] private LandmarkReceiver landmarkReceiver;

    // 0번 랜드마크 (루트)
    [SerializeField] private Transform rootLandmark;

    // 나머지 랜드마크 (1~20번)
    [SerializeField] private Transform[] landmarks;

    // 좌표에 적용할 스케일 팩터
    [SerializeField] private float scaleFactor = 0.1f;

    // 이전 프레임의 좌표를 저장할 배열
    private Vector3[] previousPositions = new Vector3[21];

    void Start()
    {
        if (landmarks.Length != 20)
        {
            Debug.LogError("landmarks 배열의 크기가 20이어야 합니다.");
        }
    }

    void Update()
    {
        string data = landmarkReceiver.receivedData;
        if (string.IsNullOrEmpty(data))
        {
            return;
        }

        string[] points = data.Split(',');
        if (points.Length != 21 * 3)
        {
            Debug.LogError("수신된 데이터의 크기가 올바르지 않습니다. 예상 크기: 63");
            return;
        }

        // 0번 랜드마크 (루트 좌표) 설정
        Vector3 rootPosition = GetLandmarkPosition(points, 0);
        if (rootLandmark != null)
        {
            rootLandmark.localPosition = rootPosition;
        }

        // 1~20번 랜드마크 설정 (상대 좌표로)
        for (int i = 1; i <= 20; i++)
        {
            Vector3 currentPosition = GetLandmarkPosition(points, i);

            // 부모의 인덱스를 결정
            int parentIndex = GetParentIndex(i);

            // 부모 랜드마크의 좌표
            Vector3 parentPosition = parentIndex == 0
                ? rootPosition
                : GetLandmarkPosition(points, parentIndex);

            // 상대 좌표 계산
            Vector3 relativePosition = currentPosition - parentPosition;

            // 랜드마크 위치 업데이트
            if (landmarks[i - 1] != null)
            {
                landmarks[i - 1].localPosition = relativePosition;
            }
        }
    }

    private Vector3 GetLandmarkPosition(string[] points, int index)
    {
        try
        {
            float x = float.Parse(points[index * 3]) * scaleFactor;
            float y = float.Parse(points[index * 3 + 1]) * scaleFactor;
            float z = float.Parse(points[index * 3 + 2]) * scaleFactor;
            return new Vector3(x, y, z);
        }
        catch
        {
            // 데이터가 잘못된 경우 이전 좌표 반환
            return previousPositions[index];
        }
    }

    private int GetParentIndex(int index)
    {
        // 부모-자식 관계 정의
        return index switch
        {
            // 손가락 루트 노드들 (1, 5, 9, 13, 17)의 부모는 0
            1 or 5 or 9 or 13 or 17 => 0,

            // 기타 손가락 노드의 부모는 이전 노드
            _ => index - 1,
        };
    }
}
