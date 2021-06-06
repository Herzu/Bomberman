using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class BomberAgent : Agent
{
    public GameObject target;

    public override void CollectObservations(VectorSensor sensor) {}

    public override void OnActionReceived(float[] actionBuffers)
    {
        var moveX = actionBuffers[0] - 1;
        var moveZ = actionBuffers[1] - 1;

        gameObject.GetComponent<Rigidbody>().velocity = new Vector3(moveX, 0, moveZ);
        SetReward(1.0f / ((gameObject.transform.position - target.transform.position).magnitude) + 1.0f);
    }
}
