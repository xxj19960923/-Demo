using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour {

    private GameObject MainCamera;      // 主摄像机物体
    private GameObject RightOpenCamera;  //  右键放大摄像机物体

    private void Awake() {
        MainCamera = GameObject.Find("ARTHUR/Camera");
        RightOpenCamera = GameObject.Find("ARTHUR/RightOpenCamera");
    }

    void CameraChange() {
        if (Move.RightOpen && !Move.OpenAgain) {
            RightOpenCamera.transform.rotation = MainCamera.transform.rotation;
            MainCamera.GetComponent<Camera>().enabled = false;
            RightOpenCamera.GetComponent<Camera>().enabled = true;

            Move.OpenAgain = true;
        }
        else if (Move.RightOpen && Move.OpenAgain) {
            RightOpenCamera.GetComponent<Camera>().fieldOfView = 10;
            Move.OpenAgain = false;
        }
        else {
            RightOpenCamera.GetComponent<Camera>().fieldOfView = 35;
            MainCamera.GetComponent<Camera>().enabled = true;
            RightOpenCamera.GetComponent<Camera>().enabled = false;
        }
    }

    void CameraShake() {
        Move.isShake = true;
        RightOpenCamera.transform.DOShakeRotation(1.5f, new Vector3(15, 0, 0));
    }
}
