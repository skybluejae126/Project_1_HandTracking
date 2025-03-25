using UnityEngine;

public class HandBySphere : MonoBehaviour
{
    [SerializeField] private LandmarkReceiver landmarkReceiver;

    // 손목에 해당하는 지점을 여러 개 설정 가능
    [SerializeField] private Transform[] wristPoints;

    // 1번부터 20번까지의 나머지 랜드마크
    [SerializeField] private Transform[] otherLandmarkPoints;

    // 좌표에 적용할 스케일 팩터
    [SerializeField] private float scaleFactor = 0.1f;

    // 이전 프레임의 좌표를 저장할 배열
    private Vector3[] previousPositions = new Vector3[21];

    // 초기 위치를 저장할 배열
    private Vector3[] initialPositions;

    void Start()
    {
        // 나머지 랜드마크 배열의 크기 확인
        if (otherLandmarkPoints.Length != 20)
        {
            Debug.LogError("otherLandmarkPoints 배열의 크기가 20이어야 합니다.");
        }

        // 초기 위치 저장
        int totalLandmarks = 1 + otherLandmarkPoints.Length; // Wrist + Other
        initialPositions = new Vector3[totalLandmarks];

        // Wrist 초기 위치
        for (int i = 0; i < wristPoints.Length; i++)
        {
            if (wristPoints[i] != null)
                initialPositions[i] = wristPoints[i].localPosition;
        }

        // Other Landmark 초기 위치
        for (int i = 0; i < otherLandmarkPoints.Length; i++)
        {
            if (otherLandmarkPoints[i] != null)
                initialPositions[i + 1] = otherLandmarkPoints[i].localPosition;
        }
    }

    void Update()
    {
        string data = landmarkReceiver.receivedData;
        if (string.IsNullOrEmpty(data))
        {
            // 데이터가 없으면 초기 위치를 유지
            MaintainInitialPositions();
            return;
        }

        string[] points = data.Split(',');
        if (points.Length != 21 * 3)
        {
            Debug.LogError("수신된 데이터의 크기가 올바르지 않습니다. 예상 크기: 63");
            return;
        }

        for (int i = 0; i < 21; i++)
        {
            float x, y, z;

            try
            {
                // 현재 데이터에서 좌표 값 추출
                x = float.Parse(points[i * 3]) * scaleFactor;
                y = float.Parse(points[i * 3 + 1]) * scaleFactor;
                z = float.Parse(points[i * 3 + 2]) * scaleFactor;

                // 이전 값 갱신
                previousPositions[i] = new Vector3(x, y, z);
            }
            catch
            {
                // 데이터가 없거나 잘못된 경우 이전 값 유지
                x = previousPositions[i].x;
                y = previousPositions[i].y;
                z = previousPositions[i].z;
            }

            // 손목 좌표 업데이트 (여러 개 처리)
            if (i == 0 && wristPoints.Length > 0)
            {
                foreach (var wristPoint in wristPoints)
                {
                    if (wristPoint != null)
                        wristPoint.localPosition = new Vector3(x, y, z);
                }
            }
            // 나머지 랜드마크 업데이트
            else if (i > 0 && i <= 20 && otherLandmarkPoints[i - 1] != null)
            {
                otherLandmarkPoints[i - 1].localPosition = new Vector3(x, y, z);
            }
        }
    }

    private void MaintainInitialPositions()
    {
        // Wrist 초기 위치 유지
        for (int i = 0; i < wristPoints.Length; i++)
        {
            if (wristPoints[i] != null)
                wristPoints[i].localPosition = initialPositions[i];
        }

        // Other Landmark 초기 위치 유지
        for (int i = 0; i < otherLandmarkPoints.Length; i++)
        {
            if (otherLandmarkPoints[i] != null)
                otherLandmarkPoints[i].localPosition = initialPositions[i + 1];
        }
    }
}
