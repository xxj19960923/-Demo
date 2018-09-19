using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridOfList : MonoBehaviour {

    public static GridOfList _Instance;

    public TextAsset InfoOfListText;
    //创建字典通过id查找物品信息
    private Dictionary<int, Object_Info> ObjectDict = new Dictionary<int, Object_Info>();
    //初始化
    private void Awake() {
        _Instance = this;
        ReadInfo();        
    }
    //通过id查找物品信息
    public Object_Info GetInfo(int id) {
        Object_Info info = null;
        ObjectDict.TryGetValue(id, out info);
        return info;
    }

    //读取物品数据文本
    void ReadInfo() {
        string text = InfoOfListText.text;
        string[] infoArray = text.Split('\n');
        foreach(string str in infoArray) {
            string[] strArray = str.Split(',');
            if (strArray[3] == "Drug") {
                Drug_Info info = new Drug_Info();
                info.id = int.Parse(strArray[0]);
                info.name = strArray[1];
                info.icon_name = strArray[2];
                info.type = ObjectTypes.Drug;
                info.hp = int.Parse(strArray[4]);
                info.mp = int.Parse(strArray[5]);
                info.price_sell = int.Parse(strArray[6]);
                info.price_buy = int.Parse(strArray[7]);
                ObjectDict.Add(info.id,info);
            }
            else if(strArray[3] == "Mat") {
                Mat_Info info = new Mat_Info();
                info.id = int.Parse(strArray[0]);
                info.name = strArray[1];
                info.icon_name = strArray[2];
                info.type = ObjectTypes.Mat;
                info.description = strArray[4];
                ObjectDict.Add(info.id,info);
            }           
        }
    }


}

public enum ObjectTypes {
    Drug,
    Mat
}

public class Object_Info {
    public int id;
    public string name;
    public string icon_name;
    public ObjectTypes type;
}

public class Drug_Info : Object_Info {
    public int hp;
    public int mp;
    public int price_sell;
    public int price_buy;
}

public class Mat_Info : Object_Info {
    public string description;
}


