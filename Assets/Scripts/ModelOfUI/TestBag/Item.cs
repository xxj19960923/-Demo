using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, ICanvasRaycastFilter{

    private Transform canvasTra;
    private Transform oldParent;
    private Image image;
    private ItemGrid oldGrid;
    private bool isRaycastLocationValid = true;//默认射线不能穿透物品

    void Awake() {
        image = GetComponent<Image>();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        if (canvasTra == null) canvasTra = GameObject.Find("Canvas").transform;
        oldParent = transform.parent;
        oldGrid = oldParent.GetComponent<ItemGrid>();
        transform.SetParent(canvasTra);
        isRaycastLocationValid = false;
    }

    public void OnDrag(PointerEventData eventData) {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData) {
        GameObject go = eventData.pointerCurrentRaycast.gameObject;
        
        if (go != null) {
            int id = oldGrid.id;
            int num = oldGrid.num;
            if (go.tag.Equals("Grid")) {
                ItemGrid newGrid = go.GetComponent<ItemGrid>();
                SetParentAndPosition(transform, go.transform);                                               
                oldGrid.ClearInfo();
                newGrid.SetId(id,num);                
            }
            else if (go.tag.Equals("Item")) {
                ItemGrid newGrid = go.transform.parent.GetComponent<ItemGrid>();
                SetParentAndPosition(transform, go.transform.parent);
                SetParentAndPosition(go.transform, oldParent);
                oldGrid.SetId(newGrid.id, newGrid.num);
                newGrid.SetId(id, num);
            }
            else {
                SetParentAndPosition(transform, oldParent);
            }
        }
        else {
            SetParentAndPosition(transform, oldParent);
        }
        isRaycastLocationValid = true;
    }

    public void SetParentAndPosition(Transform child, Transform parent) {
        child.SetParent(parent);
        child.position = parent.position;
    }

    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera) {
        return isRaycastLocationValid;
    }

    public void SetId(int id) {
        ObjectInfo info = ObjectsInfo._Instance.GetObjectInfo(id);
        Sprite sp = Resources.Load(info.icon_name, typeof(Sprite)) as Sprite;
        image.sprite = sp;
    }

    public void SetIconName(string icon_name) {
        Sprite sp = Resources.Load(icon_name, typeof(Sprite)) as Sprite;
        image.sprite = sp;
    }

}
