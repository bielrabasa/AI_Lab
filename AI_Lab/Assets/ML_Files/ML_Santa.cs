using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;

public class ML_Santa : Agent
{
    Vector3 pos;
    float stoppedTimer;

    Vector2[] targetCheckpoints = new[]
    {
        new Vector2(0, 0),
        new Vector2(50, 50),
    };

    public Transform Target;

    public override void OnEpisodeBegin()
    {
        // If the Agent fell, zero its momentum
        if (this.transform.localPosition.y < 0)
        {
            this.transform.localPosition = new Vector3(0, 0.5f, 0);
        }
        //Set position
        pos = transform.position;

        // Move the target to a new spot
        Vector2 newPos = targetCheckpoints[Random.Range(0, targetCheckpoints.Length)];
        Target.localPosition = new Vector3(newPos.x, 1, newPos.y);

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions
        sensor.AddObservation(Target.localPosition);
        sensor.AddObservation(this.transform.localPosition);
        sensor.AddObservation(this.transform.rotation.eulerAngles);
    }

    public float speed = 1;
    public float turnSpeed = 120;
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        //Check if always on same position
        if (pos == transform.position)
        {
            stoppedTimer += Time.deltaTime;
        }
        else
        {
            pos = transform.position;
            stoppedTimer = 0.0f;
        }

        //Possible movements
        int actionValue = actionBuffers.DiscreteActions[0];
        switch (actionValue)
        {
            case 1:
                {
                    transform.Rotate(transform.up * -turnSpeed * Time.deltaTime);
                }
                break;
            case 2:
                {
                    transform.Rotate(transform.up * turnSpeed * Time.deltaTime);
                }
                break;
            case 3:
                {
                    transform.position += transform.forward * speed * Time.deltaTime;
                }
                break;
            case 4:
                {
                    transform.Rotate(transform.up * -turnSpeed * Time.deltaTime);
                    transform.position += transform.forward * speed * Time.deltaTime;
                }
                break;
            case 5:
                {
                    transform.Rotate(transform.up * turnSpeed * Time.deltaTime);
                    transform.position += transform.forward * speed * Time.deltaTime;
                }
                break;
        }

        //Negative reward for stopping
        if (stoppedTimer > 3.0f)
        {
            SetReward(-0.05f);
        }

        // Reward for reaching target
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);
        if (distanceToTarget < 1.42f)
        {
            SetReward(1.0f);
            EndEpisode();
        }

        // Negative reward for -> Out of bounds
        else if (this.transform.localPosition.x > 50 || 
            this.transform.localPosition.x < -50 || 
            this.transform.localPosition.z > 50 || 
            this.transform.localPosition.z < -50)
        {
            SetReward(-1.0f);
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        int value = 0;
        if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.UpArrow)) value = 4;
        else if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.UpArrow)) value = 5;
        else if (Input.GetKey(KeyCode.LeftArrow)) value = 1;
        else if (Input.GetKey(KeyCode.RightArrow)) value = 2;
        else if (Input.GetKey(KeyCode.UpArrow)) value = 3;

        var temp = actionsOut.DiscreteActions;
        temp[0] = value;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //If it touches anything (except floor), negative reward
        if (!collision.gameObject.CompareTag("IGNORE_COLLISION"))
        {
            SetReward(-0.05f);
        }
    }
}
