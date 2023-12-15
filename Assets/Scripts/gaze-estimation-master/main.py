import mediapipe as mp
import cv2
import gaze
import socket

def exit_program(cap):
    cap.release()
    cv2.destroyAllWindows()
    print("Exiting program.")
    exit()

mp_face_mesh = mp.solutions.face_mesh

sock = socket.socket(socket.AF_INET,socket.SOCK_DGRAM)
serverAddressPort = ("127.0.0.1",5052)

cap = cv2.VideoCapture(0)
if not cap.isOpened():
    print("Error: Unable to open camera.")
    exit_program(cap)

with mp_face_mesh.FaceMesh(
        max_num_faces=1,
        refine_landmarks=True,
        min_detection_confidence=0.5,
        min_tracking_confidence=0.5) as face_mesh:
    gaze_angle = 0  # Initialize gaze_angle outside the loop
    while cap.isOpened():
        success, image = cap.read()
        if not success:
            print("Ignoring empty camera frame.")
            continue

        # Flip the image horizontally
        image = cv2.flip(image, 1)

        image.flags.writeable = False
        image = cv2.cvtColor(image, cv2.COLOR_BGR2RGB)
        results = face_mesh.process(image)
        image = cv2.cvtColor(image, cv2.COLOR_RGB2BGR)

        if results.multi_face_landmarks:
            gaze_angle = gaze.gaze(image, results.multi_face_landmarks[0])

        font = cv2.FONT_HERSHEY_SIMPLEX
        if gaze_angle is not None:
            cv2.putText(image, f'Gaze Angle: {gaze_angle:.2f} degrees', (10, 30), font, 1, (0, 255, 0), 2, cv2.LINE_AA)

        sock.sendto(str.encode(str(gaze_angle)),serverAddressPort)

        cv2.imshow('output window', image)
        key = cv2.waitKey(2)
        if key == 27:  # ESC key
            break
        elif key == ord('q') or key == ord('Q'):  # Press 'q' or 'Q' to exit
            exit_program(cap)
        elif key == ord(' '):  # Press space bar to exit
            exit_program(cap)

exit_program(cap)
