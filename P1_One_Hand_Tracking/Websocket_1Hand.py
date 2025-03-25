import cv2
import mediapipe as mp
import socket

# 소켓 설정
UDP_IP = "127.0.0.1"  # Unity에서 사용할 IP 주소
UDP_PORT = 5052       # Unity에서 사용할 포트
sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

# Mediapipe 설정
mp_hands = mp.solutions.hands
hands = mp_hands.Hands(static_image_mode=False, max_num_hands=1, min_detection_confidence=0.7, min_tracking_confidence=0.7)

# 웹캠 설정
cap = cv2.VideoCapture(0)  # 0번 카메라 사용
cap.set(3, 640)  # 너비 설정
cap.set(4, 480)  # 높이 설정

while cap.isOpened():
    success, frame = cap.read()
    if not success:
        print("웹캠에서 영상을 읽을 수 없습니다.")
        break

    # 프레임을 RGB로 변환(mediapipe는 RGB 이미지를 처리)
    frame_rgb = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
    result = hands.process(frame_rgb)

    if result.multi_hand_landmarks:
        for hand_landmarks in result.multi_hand_landmarks:
            height, width, _ = frame.shape
            data = []

            # 각 랜드마크의 x, y, z 좌표 추출 및 변환
            for lm in hand_landmarks.landmark:
                x = lm.x * width
                y = height - (lm.y * height)    # OpenCV 좌표계 변환
                z = lm.z * width * (-1)         # z 값을 x와 동일한 비율로 스케일링
                data.extend([x, y, z])

            # 데이터를 쉼표로 구분한 문자열로 변환
            message = ",".join(map(str, data))

            # 소켓으로 데이터 전송
            sock.sendto(message.encode(), (UDP_IP, UDP_PORT))

    # 웹캠 영상 표시
    cv2.imshow("WebCam", frame)

    # 'q' 키를 누르면 종료
    if cv2.waitKey(1) & 0xFF == ord("q"):
        break

# 정리
cap.release()
sock.close()
hands.close()
cv2.destroyAllWindows()
