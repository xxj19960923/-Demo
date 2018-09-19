using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bag_Grid : MonoBehaviour {

    public int id;
    public int num;
    private Text countText;
    private Object_Info info = null;

    public void GetText() {
        countText = GetComponentInChildren<Text>();
    }

    public void SetId(int id,int num = 1) {
        GetText();//获得数量文本
        info = GridOfList._Instance.GetInfo(id);//通过id查找物品
        Bag_Item item = GetComponentInChildren<Bag_Item>();//获得子物体下的Bag_Item
        item.SetImage(info.id,info.icon_name);//设置图片
        this.id = id;
        this.num = num;
        //if (this.num > 1) {//数量大于1时显示数字
        //    countText.enabled = true;
        //}
        countText.text = this.num.ToString();
    }

    public void ClearInfo() {
        id = 0;
        num = 0;
        info = null;
    }

    public void SetNum(int num=1) {
        this.num += num;
        if(this.num>1 && !countText.enabled) {//数量大于1时显示数量面板
            countText.enabled = true;
        }
        else if((this.num ==0 ||this.num==1) && countText.enabled){//数量等于0或者1时隐藏数量面板
            countText.enabled = false;
        }
        countText.text = this.num.ToString();
    }
}
