# 📖 Project_1_HandTracking 📖

## ✎ About ✎
  - Unity, python, opencv, mediapipe

## ✎ Purpose of this project ✎
  - Most modern AR tracking solutions rely on Apple's ARKit. This project aims to use a webcam or a regular smartphone along with Python's MediaPipe to achieve tracking.
The goal is to track body movements in Python and replicate them with a 3D model in Unity.

## ✎ Goals ✎
<span style="color:red;">Currently, this project supports tracking only one hand.</span> However, future updates will enable dual-hand and full-body tracking.

- **Real-time Tracking**: Track hands or body movements in real time and apply them to a 3D model.
  - *Hand Mode*: Tracks only the hands.
  - *Body Mode*: Tracks the entire body.
  - *Sign Language Education*: The 3D hand model can be used for sign language education.
- **Partial Tracking & Data Merging**:
  - Record and merge tracking data for specific body parts.
  - Track different body parts (e.g., hands, torso) separately and combine them later into a single sequence.
  - This feature could be useful for 3D motion capture applications, such as dance cover videos.

## ✎ File Structure ✎
```
/Project_1_HandTracking
│── /Example_1                    # MediaPipe sample scripts
│── /P1_One_Hand_Tracking         # Python script for tracking & Unity project for rendering 3D model
│── LICENSE                       # LICENSE
│── README.md                     # Project documentation
```
1. **Example_1**:
   - Contains sample scripts utilizing MediaPipe.
   - If image or video files are placed in the same directory, the script can visualize how MediaPipe tracks hand movements.
2. **P1_One_Hand_Tracking**:
   - Includes the Python script for tracking and the Unity project for rendering the 3D model.

## ✎ How It Works ✎
1. Connect a webcam or an Android smartphone to a PC.
   - On Windows 11 or later, the built-in smartphone linking feature can be used to treat the phone as a webcam.
2. Use Python's MediaPipe to track the hand.
   - MediaPipe detects 21 landmark points that define hand movements.
3. Send landmark data from Python to Unity via socket communication in JSON format.
4. Use this data to animate a 3D hand model in Unity.

## ✎ How to Use ✎
```sh
# Clone the repository
git clone https://github.com/skybluejae126/Project_1_HandTracking.git
cd P1_One_Hand_Tracking
```
```sh
# Make venv Python version 3.8.x
py -3.8 -m venv venv
```
```sh
# Install dependencies
pip install -r requirements.txt
```

Check your webcam working

Run the Python tracking script
Websocket_1Hand.py

1. Download the One Hand Tracking project.
2. Run the Python script.
3. Launch Unity.
4. Use the webcam to track and visualize hand movements.

## ✎ Dependencies ✎
- Python 3.8.x
- OpenCV
- MediaPipe
- Unity 2021 or later

## ✎ License ✎
This project is licensed under the MIT License. See the `LICENSE` file for details.
