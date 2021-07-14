using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class BomberAgent : Agent
{
    public int positionX;
    public int positionY;
    public GameObject target;
    public float previousScore;
    public bool targetFound;
    public GameController gameController;
    private const float distance = 2.0f;
    public float placeReward = 0.1f;
    public float timeReward = -0.0001f;
    public float deathReward = -1.0f;
    public float progressReward = 0.1f;
    public float regressReward = -0.0f;
    public float nearBombReward = -0.05f;
    public float notNearBombReward = 0.000f;
    public float finishReward = 1.0f;

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(gameObject.transform.InverseTransformPoint(target.transform.position));
        sensor.AddObservation(gameObject.GetComponent<Rigidbody>().velocity);
        sensor.AddObservation(gameObject.GetComponent<Character>().bombs);
    }
    private void checkHit(Vector3 vec, string tag, float reward, bool hitted)
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, vec, out hit, distance))
        {
            if (hit.collider.gameObject.CompareTag(tag))
            {
                if(hitted)
                    AddReward(reward);
            }
            else
            {
                if (!hitted)
                    AddReward(reward);
            }
        }
    }
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        var character = gameObject.GetComponent<Character>();

        var moveX = actionBuffers.DiscreteActions[0] - 1;
        var moveZ = actionBuffers.DiscreteActions[1] - 1;
        var placeBomb = actionBuffers.DiscreteActions[2];

        if (character.bombs > 0  && placeBomb == 1)
        {
            
            character.placeBomb();
            checkHit(Vector3.forward, "Block", placeReward,true);
            checkHit(Vector3.left, "Block", placeReward,true);
            checkHit(Vector3.right, "Block", placeReward,true);
            checkHit(Vector3.back, "Block", placeReward,true);
            checkHit(Vector3.forward, "Player", placeReward, true);
            checkHit(Vector3.left, "Player", placeReward, true);
            checkHit(Vector3.right, "Player", placeReward, true);
            checkHit(Vector3.back, "Player", placeReward, true);
        }

        checkHit(Vector3.forward, "Bomb", nearBombReward,true);
        checkHit(Vector3.left, "Bomb", nearBombReward,true);
        checkHit(Vector3.right, "Bomb", nearBombReward,true);
        checkHit(Vector3.back, "Bomb", nearBombReward,true);
        gameObject.GetComponent<Rigidbody>().velocity = new Vector3(5*moveX, 0, 5*moveZ);
        float newScore = (1.0f / (((gameObject.transform.position - target.transform.position).magnitude) + 1.0f));

        if (newScore > previousScore+0.0005f) {
            previousScore = newScore;
            AddReward(progressReward);
        }
        if (regressReward != 0 && character.bombs > 0 && newScore < previousScore)
        {
            previousScore = newScore;
            AddReward(regressReward);
        }
        if (character.isDead()) {
            SetReward(deathReward);
            EndEpisode();
            target.GetComponent<BomberAgent>().targetFound = true;
            character.lifes = 1;
        }
        if (targetFound) {
            AddReward(finishReward);
            EndEpisode();
            targetFound = false;
        }
        AddReward(timeReward);
    }
    public override void OnEpisodeBegin()
    {
        gameController.Fill2D();
        Vector3Int offset = Vector3Int.FloorToInt(this.gameObject.transform.parent.gameObject.transform.position);
        gameObject.transform.position = new Vector3Int(positionX, 1, positionY) + offset;
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
