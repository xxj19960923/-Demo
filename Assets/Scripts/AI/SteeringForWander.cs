using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringForWander : Steering {

    //徘徊半径，即Wander圈的半径
    public float wanderRadius;
    //徘徊距离，即Wander圈凸出在AI角色前面的距离
    public float wanderDistance;
    //每秒加到目标的随机位移的最大值
    public float wanderJitter;
    public bool isPlanar;
    private Vector3 desiredVelocity;
    private Vehicle m_vehicle;
    private float maxSpeed;
    private Vector3 circleTarget;
    private Vector3 wanderTarget;

    private void Start() {
        m_vehicle = GetComponent<Vehicle>();
        maxSpeed = m_vehicle.maxSpeed;
        isPlanar = m_vehicle.isPlanar;
        //选取圆圈上的一个点作为初始点
        circleTarget = new Vector3(wanderRadius * 0.707f, 0, wanderRadius * 0.707f);
    }

    public override Vector3 Force() {
        //计算随机位移
        Vector3 randomDisplacement = new Vector3((Random.value - 0.5f) * 2 * wanderJitter,
            (Random.value - 0.5f) * 2 * wanderJitter, (Random.value - 0.5f) * 2 * wanderJitter);
        if (isPlanar)
            randomDisplacement.y = 0;
        //将随机位移加到初始点上，得到新的位置
        circleTarget += randomDisplacement;
        //由于新位置很可能不在圆周上，因此需要投影到圆周上
        circleTarget = wanderRadius * circleTarget.normalized;
        //之前计算出的值是相对于AI角色和AI角色的向前方向的，需要转换为世界坐标
        wanderTarget = m_vehicle.velocity.normalized * wanderDistance +
            circleTarget + transform.position;
        //计算预期速度，返回操控向量
        desiredVelocity = (wanderTarget - transform.position).normalized * maxSpeed;
        return (desiredVelocity - m_vehicle.velocity);
    }
}
