using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Texture texture;

    public GameObject Player;

    float R; //摄像机与主角之间的距离，半径R
    float angle = 0; //旋转角度
    float speedRotationY = 1.5f; //Y轴旋转速度 
    Vector3 OffSet;   // 摄像机与主角的位置偏移量
    //摄像机垂直角度限制
    float minY = -35;
    float maxY = 9;

    //摄像机缩放限制
    float RMin = -7.13f;
    float RMax = -4.13f;
    float YMin = 2.78f;
    float YMax = 3.28f;

    // Use this for initialization
    void Start() {
        OffSet = transform.position - Player.transform.position;
        if(this.name == "Camera") {
            R = -Mathf.Sqrt(Mathf.Pow(OffSet.z, 2) + Mathf.Pow(OffSet.x, 2));
        }else if(this.name == "RightOpenCamera") {
            R = Mathf.Sqrt(Mathf.Pow(OffSet.z, 2) + Mathf.Pow(OffSet.x, 2));
        }
        
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update() {
        //视野缩放
        if (Input.GetAxis("Mouse ScrollWheel") != 0 && !Move.RightOpen && this.name == "Camera") {
            R += Input.GetAxis("Mouse ScrollWheel") * 3;
            OffSet.y -= Input.GetAxis("Mouse ScrollWheel") * 0.5f;
            R = Mathf.Clamp(R, RMin, RMax);
            OffSet.y = Mathf.Clamp(OffSet.y, YMin, YMax);
        }
    }

    void LateUpdate() {
        TurnRotation();
    }


    void TurnRotation() {
        if (!Move.isShow && !Move.isShake) {
            //根据鼠标旋转角色以及摄像机
            float mouseY = Input.GetAxis("Mouse X");
            float mouseX = Input.GetAxis("Mouse Y");
            Vector3 eular = Player.transform.eulerAngles;
            eular.y += mouseY * speedRotationY;
            Player.transform.eulerAngles = eular;

            Vector3 rotation = transform.eulerAngles;
            rotation.y = eular.y;
            rotation.x = ClampAngle(rotation.x - mouseX, Move.RightOpen ? -65 : minY, Move.RightOpen ? 25 : maxY);//限制垂直角度
            transform.eulerAngles = rotation;
            angle = rotation.y;

            //计算旋转角度 世界坐标x轴与z轴的偏移长度
            float angleNumber = angle * Mathf.PI / 180;
            OffSet.x = Mathf.Sin(angleNumber) * R;
            OffSet.z = Mathf.Cos(angleNumber) * R;
            transform.position = Player.transform.position + OffSet;
        }       
    }

    //使用插值限制垂直角度
    private float ClampAngle(float angle, float min, float max) {
        if (angle > 270) angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }

}
