using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringForEvade : Steering {

    public GameObject target;
    private Vector3 desiredVelocity;
    private Vehicle m_vehilcle;
    private float maxSpeed;

    private void Start() {
        m_vehilcle = GetComponent<Vehicle>();
        maxSpeed = m_vehilcle.maxSpeed;
    }

    public override Vector3 Force() {
        Vector3 toTarget = target.transform.position - transform.position;
        //向前预测的时间
        float lookaheadTime = toTarget.magnitude / (maxSpeed + target.GetComponent<Vehicle>().velocity.magnitude);
        //计算预期速度
        desiredVelocity = (transform.position - (target.transform.position +
            target.GetComponent<Vehicle>().velocity * lookaheadTime)).normalized * maxSpeed;
        //返回操控向量
        return (desiredVelocity - m_vehilcle.velocity);
    }
}
