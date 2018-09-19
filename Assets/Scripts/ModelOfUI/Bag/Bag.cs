using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bag : MonoBehaviour {

    public static Bag _Instance;
    public List<Bag_Grid> Grids = new List<Bag_Grid>();
    public GameObject item;

    private void Awake() {
        _Instance = this;
    }


    //背包取消按钮
    public void BtnCancelClick() {
        Move.isShow = false;
        Cursor.lockState = CursorLockMode.Locked;
        Move.BagAnim.DOPlayBackwards();
        StartCoroutine(UIManager.Wait());
    }
    //拾取到ID的物品
    public void GetID(int id) {
        //第一种情况：找到存在相同物品的格子，叠加
        Bag_Grid Grid = null;
        foreach(Bag_Grid temp in Grids) {
            if(temp.id == id) {
                Grid = temp;
                break;
            }
        }
        if (Grid != null) {//找到存在相同物品的格子
            Grid.SetNum();
        }
        else {
            foreach(Bag_Grid temp in Grids) {//查找空格子
                if(temp.id == 0) {
                    Grid = temp;
                    break;
                }
            }
            if(Grid != null) {//找到空格子
                GameObject itemGo = GameObject.Instantiate(item);
                itemGo.name = "Item";
                itemGo.transform.SetParent(Grid.transform);
                itemGo.transform.localPosition = Vector3.zero;
                itemGo.transform.localScale = Vector3.one;
                Grid.SetId(id);
            }
            else {//找不到空格子
                Debug.Log("背包已经满了");
            }
        }
    }
}
