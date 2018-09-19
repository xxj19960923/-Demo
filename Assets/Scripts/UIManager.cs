 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Text Typetxt;  //武器类型
    public Text Moneytxt;//金钱
    public Image RightOpenImg; //右键开镜图片
    public Image GunShotImg; //准星图片
    private GameObject Hit;  //信息提示
    private int CurrentCoin;

    internal static bool timeOut = false;//是否动画执行


    private void Awake() {
        Hit = GameObject.Find("Hit");
        CurrentCoin = PlayerState.coin;
    }


    void TypeTxt(int ShotType) {
        if(ShotType == 1) {
            Typetxt.text = "武器类型：冲锋枪" ;
        }else if(ShotType == 2) {
            Typetxt.text = "武器类型：狙击枪";
        }else if(ShotType == 3) {
            Typetxt.text = "武器类型：手雷";
        }
        
    }

    //void MoneyTxt(int number) {
    //    PlayerState.coin += number;
    //    Moneytxt.text = "金钱：" + PlayerState.coin + "$";
    //}


    void ImageSetActive(bool RightOpen) {
         RightOpenImg.enabled = RightOpen;
         GunShotImg.enabled = !RightOpen;      
    }
    //显示提示效果
    void GameObjectSetActive(object[] message) {
        Hit.GetComponent<Image>().enabled = (bool)message[0];
        Text text = Hit.GetComponentInChildren<Text>();
        text.enabled = (bool)message[0];
        text.text = (string)message[1];
    }

    //等待动画执行
    public static IEnumerator Wait() {
        timeOut = true;
        yield return new WaitForSeconds(1.5f);
        timeOut = false;
    }

    private void Update() {
        if (CurrentCoin != PlayerState.coin) {
            CurrentCoin = PlayerState.coin;
            Moneytxt.text = "金钱：" + CurrentCoin + "$";
        }
    }

}
