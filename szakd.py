import cv2
import mediapipe as mp
from pynput.keyboard import Controller


KEYBOARD = Controller()


def get_finger_status(hands_module, hand_landmarks, finger_name):
    finger_id_map = {"INDEX": 8, "MIDDLE": 12, "RING": 16, "PINKY": 20}

    finger_tip_y = hand_landmarks.landmark[finger_id_map[finger_name]].y
    finger_dip_y = hand_landmarks.landmark[finger_id_map[finger_name] - 1].y
    finger_mcp_y = hand_landmarks.landmark[finger_id_map[finger_name] - 2].y

    return finger_tip_y < finger_mcp_y


def get_thumb_status(hands_module, hand_landmarks):
    thumb_tip_x = hand_landmarks.landmark[hands_module.HandLandmark.THUMB_TIP].x
    thumb_mcp_x = hand_landmarks.landmark[hands_module.HandLandmark.THUMB_TIP - 2].x
    thumb_ip_x = hand_landmarks.landmark[hands_module.HandLandmark.THUMB_TIP - 1].x

    return thumb_tip_x > thumb_ip_x > thumb_mcp_x


def start_video():
    drawing_module = mp.solutions.drawing_utils
    hands_module = mp.solutions.hands

    capture = cv2.VideoCapture(0)
    capture.set(3, 640)
    capture.set(4, 480)

    with hands_module.Hands(static_image_mode=False, min_detection_confidence=0.6,
                            min_tracking_confidence=0.5, max_num_hands=1) as hands:
        while capture.isOpened():

            ret, frame = capture.read()
            results = hands.process(cv2.cvtColor(frame, cv2.COLOR_BGR2RGB))

            player_status = "DEFAULT"
            if results.multi_hand_landmarks:
                for hand_landmarks in results.multi_hand_landmarks:
                    drawing_module.draw_landmarks(frame, hand_landmarks, hands_module.HAND_CONNECTIONS)

                    current_state = ""
                    thumb_status = get_thumb_status(hands_module, hand_landmarks)
                    current_state += "1" if thumb_status else "0"

                    index_status = get_finger_status(hands_module, hand_landmarks, "INDEX")
                    current_state += "1" if index_status else "0"

                    middle_status = get_finger_status(hands_module, hand_landmarks, "MIDDLE")
                    current_state += "1" if middle_status else "0"

                    ring_status = get_finger_status(hands_module, hand_landmarks, "RING")
                    current_state += "1" if ring_status else "0"

                    pinky_status = get_finger_status(hands_module, hand_landmarks, "PINKY")
                    current_state += "1" if pinky_status else "0"

                    if current_state == "00000":
                        player_status = "BLIND"
                        KEYBOARD.press("b")
                        KEYBOARD.release("b")
                    elif current_state == "11111":
                        player_status = "DEAF"
                        KEYBOARD.press("n")
                        KEYBOARD.release("n")
                    elif current_state == "01100":
                        player_status = "MUTE"
                        KEYBOARD.press("m")
                        KEYBOARD.release("m")
                    else:
                        player_status = "DEFAULT"

                if player_status != "DEFAULT":
                    print("Player in " + player_status + " status.")

            cv2.imshow("LIVE | Hand Gesture Game Controller", frame)

            if cv2.waitKey(1) == 27:
                break

        cv2.destroyAllWindows()
        capture.release()


if __name__ == "__main__":
    start_video()

################
# so for clarification
# currently, this works differently for the two hands (for some unknown reason)
# but it at least works, so I shall leave it at that
################
# right hand mouse, left hand gesture: stop/blind = only thumb, default/mute = 3 (aka thumb, index, middle), run/deaf = 4 (all but thumb)
# left hand mouse, right hand gesture: works as intended: rock = stop/blind, scissors = default/mute, paper = run/deaf
################
# I can change which should be the default movement (cuz just now I changed it up to opposite hands)
# but for now I think I'll leave it normal for right-handed people (I'm sorry, lefties)
