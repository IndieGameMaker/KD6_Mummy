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
        var action = actions.DiscreteActions;
        //Debug.Log($"[0]={action[0]}, [1]={action[1]}");
        Vector3 dir = Vector3.zero;
        Vector3 rot = Vector3.zero;

        // Branch 0
        switch (action[0])
        {
            case 1: dir = tr.forward; break;
            case 2: dir = -tr.forward; break;
        }
        // Branch 1
        switch (action[1])
        {
            case 1: rot = -tr.up; break; //왼쪽 회전
            case 2: rot = tr.up; break;  //오른쪽 회전
        }

        tr.Rotate(rot, Time.fixedDeltaTime * turnSpeed);
        rb.AddForce(dir * moveSpeed, ForceMode.VelocityChange);

        // 마이너스 패널티
        AddReward(-1 / (float)MaxStep); // 5000 -> 0.005 
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var actions = actionsOut.DiscreteActions;
        actions.Clear();

        // Branch 0 - 이동 (정지/전진/후진) 0, 1, 2 : Size 3
        if (Input.GetKey(KeyCode.W))
        {
            actions[0] = 1; //전진
        }
        if (Input.GetKey(KeyCode.S))
        {
            actions[0] = 2; //후진
        }
        // Branch 1 - 회전 (정지/왼쪽회전/오른쪽회전) 0, 1, 2 : Size 3
        if (Input.GetKey(KeyCode.A))
        {
            actions[1] = 1; //왼쪽 회전
        }
        if (Input.GetKey(KeyCode.D))
        {
            actions[1] = 2; //오른쪽 회전
        }
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.CompareTag("GOOD_ITEM"))
        {
            Destroy(coll.gameObject);
            AddReward(+1.0f);
            rb.velocity = rb.angularVelocity = Vector3.zero;
        }

        if (coll.collider.CompareTag("BAD_ITEM"))
        {
            AddReward(-1.0f);
            EndEpisode();
        }

        if (coll.collider.CompareTag("WALL"))
        {
            AddReward(-0.1f);
        }
    }
}
