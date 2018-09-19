using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Bag_Item : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,ICanvasRaycastFilter, IBeginDragHandler, IDragHandler, IEndDragHandler {

    private Transform oldParent;//用于保存在拖动前的位置
    private Transform BagTra; // 用于获得放置的父物体
    private Image image;
    private bool isRayCastLocal = true;//默认射线不能穿透物品

    private Bag_Grid old_Grid;
    private int id;

    void Awake() {
        image = GetComponent<Image>();
        BagTra = GameObject.Find("UIManager/BagSystem/Bag").GetComponent<Transform>();
    }


    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera) {
        return isRayCastLocal;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        oldParent = transform.parent;
        old_Grid = oldParent.GetComponent<Bag_Grid>();
        transform.SetParent(BagTra);
        isRayCastLocal = false;
    }

    public void OnDrag(PointerEventData eventData) {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData) {
        GameObject target = eventData.pointerCurrentRaycast.gameObject;
        if(target != null) {
            int id = old_Grid.id;
            int num = old_Grid.num;
            if (target.tag == "Grid") {
                Bag_Grid new_Grid = target.GetComponent<Bag_Grid>();
                SetPositionAndParent(transform, target.transform);
                old_Grid.ClearInfo();
                new_Grid.SetId(id, num);
            }
            else if(target.tag == "Item") {
                Bag_Grid new_Grid = target.transform.parent.GetComponent<Bag_Grid>();
                SetPositionAndParent(transform, target.transform.parent);
                SetPositionAndParent(target.transform, oldParent);
                old_Grid.SetId(new_Grid.id, new_Grid.num);
                new_Grid.SetId(id, num);
            }
            else {
                SetPositionAndParent(transform, oldParent);
            }
        }
        else {
            SetPositionAndParent(transform, oldParent);
        }
        
        isRayCastLocal = true;
    }

    //设置位置和父物体
    private void SetPositionAndParent(Transform child,Transform parent) {
        child.SetParent(parent);
        child.position = parent.position;
    }

    //设置图片
    public void SetImage(int id,string icon_name) {
        Sprite sprite = Resources.Load(icon_name, typeof(Sprite)) as Sprite;
        image.sprite = sprite;
        this.id = id;//用于保存id
    }

    public void OnPointerEnter(PointerEventData eventData) {
        Object_Info info = GridOfList._Instance.GetInfo(id);
        ItemHit._Instance.Show(info.type, info.id,eventData.pointerCurrentRaycast.gameObject.transform);
    }

    public void OnPointerExit(PointerEventData eventData) {
        ItemHit._Instance.Hide();
    }
}
