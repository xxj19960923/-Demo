using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BagSystem : MonoBehaviour {

    public static BagSystem _Instance;
    private DOTweenAnimation bagAnim;
    public List<ItemGrid> itemGrid;
    public GameObject item;

    bool isBag = false;
    bool timeout = false;

    void Awake() {
        _Instance = this;
        bagAnim = GetComponent<DOTweenAnimation>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.I)) {
            if (!timeout) {
                if (!isBag)
                    bagAnim.DOPlayForward();
                else
                    bagAnim.DOPlayBackwards();
                isBag = Wait(isBag);
            }                            
        }
        if (Input.GetKeyDown(KeyCode.F)) {
            GetID(Random.Range(1001, 1003));
        }
    }

    bool Wait(bool state) {
        state = !state;
        timeout = true;
        StartCoroutine(WaitTime());
        return state;
    }
    IEnumerator WaitTime() {
        yield return new WaitForSeconds(1.5f);
        timeout = false;
    }
    //拾取到ID的物品，添加到背包中
    public void GetID(int id) {
        ItemGrid grid = null;
        foreach(var temp in itemGrid) {
            if(temp.id == id) {
                grid = temp;
                break;
            }
        }
        if (grid != null) {//存在情况
            grid.SetNum();
        }
        else {//不存在情况
            foreach (var temp in itemGrid) {
                if(temp.id == 0) {//查找空格子
                    grid = temp;
                    break;
                }
            }
            if(grid != null) {//查找到空格子,放置
                GameObject itemgo = GameObject.Instantiate(item);
                itemgo.name = "item";
                itemgo.transform.SetParent(grid.transform);
                itemgo.transform.localPosition = Vector3.zero;
                itemgo.transform.localScale = Vector3.one;
                grid.SetId(id);
            }
            else {//找不到空格子
                Debug.Log("没有空格子");
            }
        }
    }
}
