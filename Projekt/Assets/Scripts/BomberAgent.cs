using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class BomberAgent : Agent
{
    public GameObject target;

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(gameObject.transform.position.z - target.transform.position.z);
        sensor.AddObservation(gameObject.transform.rotation.x - target.transform.position.z);
    }

    public override void OnActionReceived(float[] actionBuffers)
    {
        var moveX = actionBuffers[0];
        var moveZ = actionBuffers[1];

        gameObject.transform.position = gameObject.transform.position + new Vector3(moveX-0.5, 0.0f, moveZ-0.5);
        SetReward(1.0f / ((gameObject.transform.position - target.transform.position).magnitude) + 1.0f);
    }
}
