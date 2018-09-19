using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InTask : MonoBehaviour {

    public static bool isGet = false;//是否接受了任务
    public Text myTaskContent;
    public Text TaskContent;
    private GameObject BtnState;  //获取自己任务状态
    //取消按钮功能
    int i = 0;

    private void Awake() {
        BtnState = GameObject.Find("UIManager/MyTask/State");
    }


    public void BtnCancelClick() {
        Move.isShow = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (Move.isTask) {
            Move.Task.DOPlayBackwards();
            StartCoroutine(UIManager.Wait());
        }
        else {
            Move.MyTask.DOPlayBackwards();
            StartCoroutine(UIManager.Wait());
        }
    }
    //接收按钮功能
    public void BtnAcceptClick() {
        BtnCancelClick();
        //重置标志位而且
        isGet = true; //获得任务
        Move.isTask = false;//不处于接任务状态
        CopyContent();
        BtnState.SetActive(true);
    }
    //完成任务
    public void BtnFinishClick() {
        //重置状态
        isGet = false;
        PlayerState.coin -= 5;
        ResetContent();
        for (int i = 0; i < 2; i++) {
            Bag._Instance.GetID(1001);//获得红药
            Bag._Instance.GetID(1002);//获得蓝药
        }
    }

    void Update() {
        if (PlayerState.coin >= 5) {
            BtnState.GetComponentInChildren<Text>().text = "已完成";
            BtnState.GetComponent<Button>().enabled = true;
        }
    }

    //复制内容
    public void CopyContent() {
        Transform[] t = TaskContent.GetComponentsInChildren<Transform>();
        myTaskContent.text = TaskContent.text;
        myTaskContent.alignment = TextAnchor.UpperLeft;
        //复制子物体
        for (int i = 1; i < t.Length; i++) {
            GameObject gb = GameObject.Instantiate(t[i].gameObject);
            gb.name = t[i].name;
            gb.transform.SetParent(myTaskContent.transform);           
            gb.AddComponent<ImageDestroy>();//动态添加一个自我销毁的代码
            gb.transform.localPosition = t[i].localPosition;
            gb.transform.localScale = t[i].localScale;
        }
    }
    //重置内容
    void ResetContent() {
        BroadcastMessage("DestroySelf");//广播销毁子物体
        myTaskContent.alignment = TextAnchor.MiddleCenter;
        myTaskContent.text = "当前没有任务";
        BtnState.GetComponentInChildren<Text>().text = "未完成";
        BtnState.GetComponent<Button>().enabled = false;
        BtnState.SetActive(false);
    }
}
