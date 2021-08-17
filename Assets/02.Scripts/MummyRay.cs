using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MummyRay : Agent
{
    private Transform tr;
    private Rigidbody rb;
    private StageManager stageManager;

    public float moveSpeed = 1.5f;
    public float turnSpeed = 200.0f;

    public override void Initialize()
    {
        MaxStep = 5000;

        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        stageManager = tr.parent.GetComponent<StageManager>();
    }

    public override void OnEpisodeBegin()
    {
        // 스테이지를 초기화
        stageManager.InitStage();

        // 물리엔진 초기화
        rb.velocity = rb.angularVelocity = Vector3.zero;

        // 에이젼트의 위치 변경
        tr.localPosition = new Vector3(Random.Range(-20.0f, 20.0f),
                                       0.05f,
                                       Random.Range(-20.0f, 20.0f));

        tr.localRotation = Quaternion.Euler(Vector3.up * Random.Range(0, 360));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var actions = actionsOut.DiscreteActions;
        actions.Clear();

        // Branch 0 - 이동 (정지/전진/후진) 0, 1, 2 : Size 3
        if (Input.GetKey(KeyCode.W))
        {
            actions[0] = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            actions[0] = 2;
        }
    }
}
