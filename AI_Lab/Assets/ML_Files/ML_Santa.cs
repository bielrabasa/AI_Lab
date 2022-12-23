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
        new Vector2(5, 1),
        new Vector2(5, 2),
        new Vector2(5, 3),
        new Vector2(5, 4),
        new Vector2(5, 5),
        new Vector2(-6, 20),
        new Vector2(-6, 24),
        new Vector2(-6, 27),
        new Vector2(-6, 23),
        new Vector2(20, 23),
        new Vector2(21, 17),
        new Vector2(21, 32),
        new Vector2(25, 32),
        new Vector2(28, 42),
        new Vector2(39, 42),
        new Vector2(20, 46),
        new Vector2(29, 3),
        new Vector2(34, 4),
        new Vector2(34, -5),
        new Vector2(42, -5),
        new Vector2(44, 2),
        new Vector2(42, 7),
        new Vector2(42, -10),
        new Vector2(45, -8),
        new Vector2(41, -15),
        new Vector2(43, -15),
        new Vector2(43, -18),
        new Vector2(45, -22),
        new Vector2(45, -25),
        new Vector2(41, -26),
        new Vector2(44, -27),
        new Vector2(30, -28),
        new Vector2(31, -24),
        new Vector2(25, -27),
        new Vector2(19, -30),
        new Vector2(14, -23),
        new Vector2(13, -17),
        new Vector2(7, -19),
        new Vector2(4, -21),
        new Vector2(4, -13),
        new Vector2(4, -21),
        new Vector2(-1, -23),
        new Vector2(-1, -18),
        new Vector2(-5, -20),
        new Vector2(18, -35),
        new Vector2(25, -32),
        new Vector2(23, -40),
        new Vector2(18, -44),
        new Vector2(25, -44),
        new Vector2(13, -47),
        new Vector2(8, -36),
        new Vector2(8, -47),
        new Vector2(2, -44),
        new Vector2(-5, -39),
        new Vector2(-8, -45),
        new Vector2(-13, -36),
        new Vector2(-16, -41),
        new Vector2(-16, -36),
        new Vector2(-21, -36),
        new Vector2(-24, -41),
        new Vector2(-28, -46),
        new Vector2(-33, -40),
        new Vector2(-39, -42),
        new Vector2(-40, -46),
        new Vector2(-44, -41),
        new Vector2(-48, -46),
        new Vector2(-48, -37),
        new Vector2(-45, -32),
        new Vector2(-40, -25),
        new Vector2(-31, -24),
        new Vector2(-24, -21),
        new Vector2(-20, -27),
        new Vector2(-12, -41),
        new Vector2(-45, -15),
        new Vector2(-40, -13),
        new Vector2(-47, -6),
        new Vector2(-37, -6),
        new Vector2(-37, -1),
        new Vector2(-45, -2),
        new Vector2(-49, 6),
        new Vector2(-42, 9),
        new Vector2(-27, 4),
        new Vector2(-24, -2),
        new Vector2(-17, -6),
        new Vector2(-20, 9),
        new Vector2(-24, 13),
        new Vector2(-21, -20),
        new Vector2(-27, -26),
        new Vector2(-36, 20),
        new Vector2(-44, 20),
        new Vector2(-44, 29),
        new Vector2(-49, 34),
        new Vector2(-49, 40),
        new Vector2(-43, 45),
        new Vector2(-30, 46),
        new Vector2(-27, 39),
        new Vector2(-23, 29),
        new Vector2(-23, 23),
        new Vector2(-16, 28),
        new Vector2(-7, 21),
        new Vector2(-7, 29),
        new Vector2(-1, 26),
        new Vector2(-6, 46),
        new Vector2(7, 46),
        new Vector2(16, 46),
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
