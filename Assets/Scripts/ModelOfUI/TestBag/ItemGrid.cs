using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemGrid : MonoBehaviour {

    public int id = 0;
    public int num = 0;
    private ObjectInfo info = null;
    private Text countTxt;

    public void GetText() {
        countTxt = GetComponentInChildren<Text>();
    }

    public void SetId(int id,int num = 1) {
        GetText();
        info = ObjectsInfo._Instance.GetObjectInfo(id);
        Item item = GetComponentInChildren<Item>();
        item.SetIconName(info.icon_name);
        this.num = num;
        if (this.num > 1) {
            countTxt.enabled = true;
        }
        this.id = id;
        countTxt.text = this.num.ToString();
    }

    public void ClearInfo() {
        id = 0;
        num = 0;
        info = null;
        countTxt.enabled = false;
    }

    public void SetNum(int num = 1) {
        this.num += num;
        if (this.num > 1 && !countTxt.enabled) {
            countTxt.enabled = true;
        }
        countTxt.text = this.num.ToString();
    }
}
