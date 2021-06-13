using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class BomberAgent : Agent
{
    public GameObject target;
    public float previousScore;

    public override void CollectObservations(VectorSensor sensor) 
    {
        sensor.AddObservation(gameObject.transform.InverseTransformPoint(target.transform.position));
        sensor.AddObservation(gameObject.GetComponent<Rigidbody>().velocity);
    }

    public override void OnActionReceived(float[] actionBuffers)
    {
        var moveX = actionBuffers[0] - 1;
        var moveZ = actionBuffers[1] - 1;
        Debug.Log(moveX.ToString()+";"+moveZ.ToString());
        gameObject.GetComponent<Rigidbody>().velocity = new Vector3(5*moveX, 0, 5*moveZ);
        float newScore = (1.0f / (((gameObject.transform.position - target.transform.position).magnitude) + 1.0f));
        if (newScore > previousScore+0.0005f)
        {
            previousScore = newScore;
            AddReward(1.0f);
        }
        if(newScore<previousScore)
        {
            previousScore = newScore;
            AddReward(-1.0f);
        }
    }
    public override void OnEpisodeBegin()
    {
        previousScore = 0;
        Vector3Int offset = Vector3Int.FloorToInt(this.gameObject.transform.parent.gameObject.transform.position);
        gameObject.transform.position = new Vector3Int(13, 1, 13) + offset;
    }
}
