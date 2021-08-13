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

    public override void Initialize()
    {
        MaxStep = 5000;
    }

    public override void OnEpisodeBegin()
    {
    }

    public override void CollectObservations(VectorSensor sensor)
    {
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
    }
}
