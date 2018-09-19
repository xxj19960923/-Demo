using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemHit : MonoBehaviour {

    public static ItemHit _Instance;
    private Text ItemHitText;

    void Awake() {
        _Instance = this;
        ItemHitText = GetComponentInChildren<Text>();
    }

    public void Show(ObjectTypes type,int id,Transform transform) {
        this.gameObject.SetActive(true);
        string str = "";
        switch (type) {
            case ObjectTypes.Drug:
                Drug_Info Dinfo = GridOfList._Instance.GetInfo(id) as Drug_Info;
                str = ShowDrug(str,Dinfo);
                break;
            case ObjectTypes.Mat:
                Mat_Info Minfo = GridOfList._Instance.GetInfo(id) as Mat_Info;
                str = ShowMat(str,Minfo);
                break;
        }
        ItemHitText.text = str;
        this.transform.position = transform.position - new Vector3(80,0,0);
    }


    public void Hide() {
        this.gameObject.SetActive(false);
    }

    string ShowDrug(string str,Drug_Info info) {
        str += "物品类型： " + info.type + "\n";
        str += "物品名称： " + info.name + "\n";
        str += "回血： " + info.hp + "\n";
        str += "回蓝： " + info.mp + "\n";
        str += "出售价： " + info.price_sell + "\n";
        str += "购买价： " + info.price_buy;
        return str;
    }

    string ShowMat(string str, Mat_Info info) {
        str += "物品类型： " + info.type + "\n";
        str += "物品名称： " + info.name + "\n";
        str += "物品描述： " + info.description;
        return str;
    }
}
