import cv2
import mediapipe as mp

# 손 추적 모델 초기화
mp_hands = mp.solutions.hands
hands = mp_hands.Hands(min_detection_confidence=0.7, min_tracking_confidence=0.7)
mp_drawing = mp.solutions.drawing_utils

# 이미지 파일
image_path = 'hand1.jpg'    # 같은 디렉토리에 이미지 파일 삽입

# 이미지 읽기
image = cv2.imread(image_path)

# 이미지를 RGB로 변환
image_rgb = cv2.cvtColor(image, cv2.COLOR_BGR2RGB)

# 손 추적 결과 
results = hands.process(image_rgb)

# 손의 랜드마크를 찾았을 경우 그려주기
if results.multi_hand_landmarks:
        for landmarks in results.multi_hand_landmarks:
            # 손의 랜드마크를 화면에 그리기
            mp_drawing.draw_landmarks(image, landmarks, mp_hands.HAND_CONNECTIONS)

# 이미지 표시
cv2.imshow('Hand Tracking in Image', image)

# 'q' 키를 누르면 종료
cv2.waitKey(0)

# 창 닫기기
cv2.destroyAllWindows()