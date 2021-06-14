using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class BomberAgent : Agent
{
    public GameObject target;
    public float previousScore;
    public bool targetFound;
    public GameController gameController;

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(gameObject.transform.InverseTransformPoint(target.transform.position));
        sensor.AddObservation(gameObject.GetComponent<Rigidbody>().velocity);
        sensor.AddObservation(gameObject.GetComponent<Character>().bombs);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        var character = gameObject.GetComponent<Character>();

        var moveX = actionBuffers.DiscreteActions[0] - 1;
        var moveZ = actionBuffers.DiscreteActions[1] - 1;
        var placeBomb = actionBuffers.DiscreteActions[2];

        if (placeBomb == 1) {
            character.placeBomb();
        }

        gameObject.GetComponent<Rigidbody>().velocity = new Vector3(5*moveX, 0, 5*moveZ);
        float newScore = (1.0f / (((gameObject.transform.position - target.transform.position).magnitude) + 1.0f));

        if (newScore > previousScore+0.0005f) {
            previousScore = newScore;
            AddReward(0.1f);
        }
        if(newScore < previousScore) {
            previousScore = newScore;
            AddReward(-0.1f);
        }
        if (character.isDead()) {
            SetReward(-1.0f);
            EndEpisode();
            character.lifes = 1;
        }
        if (targetFound) {
            AddReward(1.0f);
            EndEpisode();
            targetFound = false;
        }
    }
    public override void OnEpisodeBegin()
    {
        gameController.Fill2D();
        Vector3Int offset = Vector3Int.FloorToInt(this.gameObject.transform.parent.gameObject.transform.position);
        gameObject.transform.position = new Vector3Int(3, 1, 3) + offset;
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        var discreteActionsOut = actionsOut.DiscreteActions;
        switch (Mathf.RoundToInt(Input.GetAxisRaw("Horizontal"))) {
            case -1: discreteActionsOut[0] = 0; break;
            case 0: discreteActionsOut[0] = 1; break;
            case +1: discreteActionsOut[0] = 2; break;
        }
        switch (Mathf.RoundToInt(Input.GetAxisRaw("Vertical"))) {
            case -1: discreteActionsOut[1] = 0; break;
            case 0: discreteActionsOut[1] = 1; break;
            case +1: discreteActionsOut[1] = 2; break;
        }
        discreteActionsOut[2] = Input.GetKey(KeyCode.E) ? 1 : 0;
    }
}
