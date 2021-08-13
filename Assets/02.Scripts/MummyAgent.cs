using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class MummyAgent : Agent
{
    /*
        1. 주변환경을 관측(Observations)
        2. 정책(Policy)에 의한 행동(Actions)
        3. 보상(Reward)
    */

    private Transform tr;
    private Transform targetTr;
    private Rigidbody rb;

    // 초기화 작업을 하는 메소드
    public override void Initialize()
    {
        tr = GetComponent<Transform>();
        targetTr = tr.parent.Find("Target").GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
    }

    // 에피소드(학습의 단위) 시작될때 마다 호출되는 메소드
    public override void OnEpisodeBegin()
    {
        // 물리력을 초기화
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        // 에이젼의 위치를 불규칙하게 변경
        tr.localPosition = new Vector3(Random.Range(-4.0f, 4.0f),
                                       0.05f,
                                       Random.Range(-4.0f, 4.0f));
        // 타겟의 위치도 불규칙하게 변경
        targetTr.localPosition = new Vector3(Random.Range(-4.0f, 4.0f),
                                             0.55f,
                                             Random.Range(-4.0f, 4.0f));
    }

    // 주변환경을 관측하는 메소드
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(targetTr.localPosition);  // (x, y, z)  3
        sensor.AddObservation(tr.localPosition);        // (x, y, z)  3
        sensor.AddObservation(rb.velocity.x);           // 1
        sensor.AddObservation(rb.velocity.z);           // 1
    }

    // 정책(Policy)에 따라서 행동을 처리 메소드
    public override void OnActionReceived(ActionBuffers actions)
    {
    }

    // 테스트를 위한 입력값을 전달하는 메소드
    public override void Heuristic(in ActionBuffers actionsOut)
    {
    }
}
