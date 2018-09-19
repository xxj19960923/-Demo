using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringForArrive : Steering {

    public bool isPlanar = true;
    public float arrivalDistance = 0.3f;
    public float characterRadius = 1.2f;
    public Steering steeringForEvade;
    public GameObject other;
    //当与目标小于这个距离时，开始减速
    public float slowDownDistance;
    public GameObject target;
    private Vector3 desiredVelocity;
    private Vehicle m_vehicle;
    private float maxSpeed;
    private void Start() {
        m_vehicle = GetComponent<Vehicle>();
        maxSpeed = m_vehicle.maxSpeed;
        isPlanar = m_vehicle.isPlanar;
    }


    public override Vector3 Force() {
        if (Vector3.Distance(other.transform.position, transform.position) < 50) {
            steeringForEvade.enabled = true;
            this.enabled = false;
        }
        //计算AI角色与目标之间的距离
        Vector3 toTarget = target.transform.position - transform.position;
        if (isPlanar)
            toTarget.y = 0;
        float distance = toTarget.magnitude;
        //返回的操控向量
        Vector3 returnForce;
        //如果与目标之间的距离大于所设置的减速半径
        if(distance > slowDownDistance) {
            //预期速度是AI角色与目标点之间的距离
            desiredVelocity = toTarget.normalized * maxSpeed;
            //返回预期速度与当前速度的差
            returnForce = desiredVelocity - m_vehicle.velocity;
        }
        else {
            //计算预期速度，并返回预期速度与当前速度的差
            desiredVelocity = toTarget - m_vehicle.velocity;
            //返回预期速度与当前速度的差
            returnForce = desiredVelocity - m_vehicle.velocity;
            //Debug.Log("toTarget: "+ toTarget+ "desiredVelovity: " + desiredVelocity + "m_vehicle.velocity: " + m_vehicle.velocity +
            //   "returnForce: " + returnForce);
        }        
        return returnForce;
    }                       

    private void OnDrawGizmos() {
        //在目标周围画白色线框球，显示出减速范围
        Gizmos.DrawWireSphere(target.transform.position, slowDownDistance);
    }
}
