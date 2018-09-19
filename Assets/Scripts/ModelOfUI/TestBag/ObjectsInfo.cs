using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsInfo : MonoBehaviour {

    public static ObjectsInfo _Instance;
    public TextAsset ObjectOfListText;
    //创建字典通过id查找物品信息
    private Dictionary<int, ObjectInfo> ObjectInfoDict = new Dictionary<int, ObjectInfo>();
    private void Awake() {
        _Instance = this;
        ReadInfo();
    }

    //通过id返回一个物品信息
    public ObjectInfo GetObjectInfo(int id) {
        ObjectInfo info = null;
        ObjectInfoDict.TryGetValue(id, out info);
        return info;
    }


    //读取文本信息
    void ReadInfo() {
        string text = ObjectOfListText.text;
        string[] strArray = text.Split('\n');
        foreach(string str in strArray) {
            string[] proArray = str.Split(',');
            ObjectInfo info = new ObjectInfo();
            info.id = int.Parse(proArray[0]);
            info.name = proArray[1];
            info.icon_name = proArray[2];
            string type = proArray[3];
            if(type == "Drup") {
                info.type = ObjectType.Drug;
                info.hp = int.Parse(proArray[4]);
                info.mp = int.Parse(proArray[5]);
                info.price_sell = int.Parse(proArray[6]);
                info.price_buy = int.Parse(proArray[7]);
            }
            //else if (type == "Mat") {
            //    info.type = ObjectType.Mat;
            //}
            ObjectInfoDict.Add(info.id, info);
        }
    }
}

public enum ObjectType {
    Drug, //药品
    Mat   //材料
}

public class ObjectInfo {
    public int id;
    public string name;
    public string icon_name;
    public ObjectType type;
    public int hp;
    public int mp;
    public int price_sell;
    public int price_buy;
}