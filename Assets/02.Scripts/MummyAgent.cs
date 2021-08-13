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

    public Material goodMt, badMt;
    private Material originMt;
    private new Renderer renderer;

    // 초기화 작업을 하는 메소드
    public override void Initialize()
    {
        tr = GetComponent<Transform>();
        targetTr = tr.parent.Find("Target").GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();

        // Floor의 MeshRenderer 컴포넌트를 추출
        renderer = tr.parent.Find("Floor").GetComponent<Renderer>();
        originMt = renderer.material;
    }

    // 에피소드(학습의 단위) 시작될때 마다 호출되는 메소드
    public override void OnEpisodeBegin()
    {
        // 물리력을 초기화
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        // 에이젼트의 위치를 불규칙하게 변경
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
        var action = actions.ContinuousActions;
        Debug.Log($"[0]={action[0]}, [1]={action[1]}");

        Vector3 dir = (Vector3.forward * action[0]) + (Vector3.right * action[1]);
        rb.AddForce(dir.normalized * 50.0f);

        // 지속적인 움직임을 유도하기 위한 마이너스 페널티
        SetReward(-0.001f);
    }

    // 테스트를 위한 입력값을 전달하는 메소드
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        /*
            연속(Continues) Input.GetAxis("Horizontal")   -> -1.0f ~ 0.0f ~ +1.0f
            이산(Discrete)  Input.GetAxisRaw("Horizontal")-> -1.0f, 0.0f, +1.0f
        */
        var actions = actionsOut.ContinuousActions;
        //전진 후진 
        actions[0] = Input.GetAxis("Vertical");
        //좌우 이동
        actions[1] = Input.GetAxis("Horizontal");
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.CompareTag("DEAD_ZONE"))
        {
            SetReward(-1.0f);
            EndEpisode();
        }

        if (coll.collider.CompareTag("TARGET"))
        {
            SetReward(+1.0f);
            EndEpisode();
        }
    }

}
