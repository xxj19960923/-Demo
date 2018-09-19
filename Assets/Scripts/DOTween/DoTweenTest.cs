using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class DoTweenTest : MonoBehaviour {

    public RectTransform[] rectList;
    private Tweener[] tweenerList;
    bool isShow = false;
    bool timeOut = true;
    int y = 132;

	// Use this for initialization
	void Start () {
        tweenerList = new Tweener[rectList.Length];
        tweenerList[0] = rectList[0].DOLocalMove(new Vector3(-244, 0, 0), 1);
        tweenerList[0].SetAutoKill(false);
        tweenerList[0].SetEase(Ease.OutBack);
        tweenerList[0].Pause();
        for (int i = 1; i < rectList.Length; i++) {
            tweenerList[i] = rectList[i].DOLocalMove(new Vector3(147, y, 0), 1);
            tweenerList[i].SetAutoKill(false);
            tweenerList[i].SetEase(Ease.OutBack);
            tweenerList[i].Pause();
            y -= 88;
        }
    }

    IEnumerator ShowList() {
        yield return new WaitForSeconds(1);
        for (int i = 1; i < rectList.Length; i++) {
            rectList[i].DOPlayForward();
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(0.5f);
        isShow = !isShow;
        timeOut = true;
    }

    IEnumerator HideList() {
        for (int i = 0; i < rectList.Length; i++) {
            rectList[i].DOPlayBackwards();
        }
        yield return new WaitForSeconds(1.5f);
        isShow = !isShow;
        timeOut = true;
    }


    void Update() {
        if (Input.GetKeyDown(KeyCode.V)) {
            if (!isShow && timeOut) {
                timeOut = false;
                rectList[0].DOPlayForward();
                StartCoroutine(ShowList());
            }
            else if (isShow && timeOut) {
                timeOut = false;
                StartCoroutine(HideList());
            }
        }
    }
}
