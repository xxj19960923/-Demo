using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxOpen : MonoBehaviour {

    private Animation anim;
    private Transform trans;
    public GameObject[] Tool;
    private int count = 2;//生成道具个数


    void Awake() {
        anim = GetComponent<Animation>();
        trans = GetComponent<Transform>();
    }
    //播放打开动画的同时使碰撞器消失
    void BoxOpenAnim() {
        anim.Play();
        Collider[] cols = GetComponentsInChildren<Collider>();
        foreach(var col in cols) {
            col.enabled = false;
        }
        
    }
    //生成道具
    void GetTool() {
        for(int i = 0; i < count; i++) {
            int index = Random.Range(0, Tool.Length);
            float rangeX = Random.Range(-2.0f, 2.0f);
            Instantiate(Tool[index],trans.position+new Vector3(rangeX, 0.7f,1.0f), Tool[index].transform.rotation).name = Tool[index].name;
        }
        Destroy(gameObject, 5);
    }



}
