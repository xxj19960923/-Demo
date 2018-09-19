using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Steering : MonoBehaviour {

    //表示每个操控力的权重
    public float weight = 1;

    //计算操控力的方法，有派生类实现
    public virtual Vector3 Force() {
        return new Vector3(0, 0, 0);
    }


}
