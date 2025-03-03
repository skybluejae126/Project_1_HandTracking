import cv2
import mediapipe as mp

# 손 추적 모델 초기화
mp_hands = mp.solutions.hands
hands = mp_hands.Hands(min_detection_confidence=0.7, min_tracking_confidence=0.7)
mp_drawing = mp.solutions.drawing_utils

# 비디오 파일 프레임당 캡쳐쳐
cap = cv2.VideoCapture('video1.mp4')    # 같은 디렉토리에 비디오오 파일 삽입

while cap.isOpened():
    # 프레임 읽기
    success, frame = cap.read()
    if not success:
        break

    # 프레임을 RGB로 변환(mediapipe는 RGB 이미지를 처리)
    image_rgb = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)

    # 손 추적 결과 얻기
    results = hands.process(image_rgb)

    # 손의 랜드마크를 찾았을 경우 그려주기
    if results.multi_hand_landmarks:
        for landmarks in results.multi_hand_landmarks:
            # 랜드마크를 화면에 그리기
            mp_drawing.draw_landmarks(frame, landmarks, mp_hands.HAND_CONNECTIONS)

    # 이미지 표시
    cv2.imshow('Hand Tracking in Video', frame)

    # 'q' 키를 누르면 종료
    if cv2.waitKey(1) & 0xFF == ord('q'):
        break

# 창 닫기기
cap.release()
cv2.destroyAllWindows()