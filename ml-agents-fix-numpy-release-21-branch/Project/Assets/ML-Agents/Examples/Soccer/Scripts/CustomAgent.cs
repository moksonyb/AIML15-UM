using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors;


/*public enum Team
{
    Blue = 0,
    Purple = 1
}*/

public class CustomAgent: Agent
{
    // Note that that the detectable tags are different for the blue and purple teams. The order is
    // * ball
    // * own goal
    // * opposing goal
    // * wall
    // * own teammate
    // * opposing player

    public enum Position
    {
        Striker,
        Goalie,
        Generic
    }

    [HideInInspector]
    public Team team;
    float m_KickPower;
    // The coefficient for the reward for colliding with a ball. Set using curriculum.
    float m_BallTouch;
    public Position position;

    const float k_Power = 2000f;
    float m_Existential;
    float m_LateralSpeed;
    float m_ForwardSpeed;

    public float soundDetectionRadius = 10f; // Radius within which sounds are detected
    public LayerMask detectableObjectsLayer; // Layer mask for detectable objects



    [HideInInspector]
    public Rigidbody agentRb;
    SoccerSettings m_SoccerSettings;
    BehaviorParameters m_BehaviorParameters;
    public Vector3 initialPos;
    public float rotSign;

    EnvironmentParameters m_ResetParams;

    void Start()
    {
        // Set detectableObjectsLayer in Start to avoid constructor initialization issues
        detectableObjectsLayer = LayerMask.GetMask("SoundSource");
    }



    public override void Initialize()
    {
        SoccerEnvController envController = GetComponentInParent<SoccerEnvController>();
        if (envController != null)
        {
            m_Existential = 1f / envController.MaxEnvironmentSteps;
        }
        else
        {
            m_Existential = 1f / MaxStep;
        }

        m_BehaviorParameters = gameObject.GetComponent<BehaviorParameters>();
        if (m_BehaviorParameters.TeamId == (int)Team.Blue)
        {
            team = Team.Blue;
            initialPos = new Vector3(transform.position.x - 5f, .5f, transform.position.z);
            rotSign = 1f;
        }
        else
        {
            team = Team.Purple;
            initialPos = new Vector3(transform.position.x + 5f, .5f, transform.position.z);
            rotSign = -1f;
        }
        if (position == Position.Goalie)
        {
            m_LateralSpeed = 1.0f;
            m_ForwardSpeed = 1.0f;
        }
        else if (position == Position.Striker)
        {
            m_LateralSpeed = 0.3f;
            m_ForwardSpeed = 1.3f;
        }
        else
        {
            m_LateralSpeed = 0.3f;
            m_ForwardSpeed = 1.0f;
        }
        m_SoccerSettings = FindObjectOfType<SoccerSettings>();
        agentRb = GetComponent<Rigidbody>();
        agentRb.maxAngularVelocity = 500;

        m_ResetParams = Academy.Instance.EnvironmentParameters;
    }

    public void MoveAgent(ActionSegment<int> act)
    {
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        m_KickPower = 0f;

        var forwardAxis = act[0];
        var rightAxis = act[1];
        var rotateAxis = act[2];

        switch (forwardAxis)
        {
            case 1:
                dirToGo = transform.forward * m_ForwardSpeed;
                m_KickPower = 1f;
                break;
            case 2:
                dirToGo = transform.forward * -m_ForwardSpeed;
                break;
        }

        switch (rightAxis)
        {
            case 1:
                dirToGo = transform.right * m_LateralSpeed;
                break;
            case 2:
                dirToGo = transform.right * -m_LateralSpeed;
                break;
        }

        switch (rotateAxis)
        {
            case 1:
                rotateDir = transform.up * -1f;
                break;
            case 2:
                rotateDir = transform.up * 1f;
                break;
        }

        transform.Rotate(rotateDir, Time.deltaTime * 100f);
        agentRb.AddForce(dirToGo * m_SoccerSettings.agentRunSpeed,
            ForceMode.VelocityChange);
    }


    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        if (position == Position.Goalie)
        {
            AddReward(m_Existential);
        }
        else if (position == Position.Striker)
        {
            AddReward(-m_Existential);
        }
        MoveAgent(actionBuffers.DiscreteActions);
    }


    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        //forward
        if (Input.GetKey(KeyCode.W))
        {
            discreteActionsOut[0] = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            discreteActionsOut[0] = 2;
        }
        //rotate
        if (Input.GetKey(KeyCode.A))
        {
            discreteActionsOut[2] = 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            discreteActionsOut[2] = 2;
        }
        //right
        if (Input.GetKey(KeyCode.E))
        {
            discreteActionsOut[1] = 1;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            discreteActionsOut[1] = 2;
        }
    }


    /// <summary>
    /// Used to provide rewards based on collisions with specific objects.
    /// Rewards for hitting the ball and penalizes for colliding with other agents.
    /// </summary>
    void OnCollisionEnter(Collision collision)
    {
        // Reward for colliding with the ball
        if (collision.gameObject.CompareTag("ball"))
        {
            var force = k_Power * m_KickPower;
            if (position == Position.Goalie)
            {
                force = k_Power;
            }

            // Apply force to the ball and reward the agent
            var dir = collision.contacts[0].point - transform.position;
            dir = dir.normalized;
            collision.gameObject.GetComponent<Rigidbody>().AddForce(dir * force);

            AddReward(0.2f * m_BallTouch);  // Positive reward for hitting the ball
            Debug.Log($"{gameObject.name} hit the ball and received a reward.");
        }
        // Penalty for colliding with other agents
        else if (collision.gameObject.CompareTag("agent"))
        {
            AddReward(-0.1f);  // Negative reward (penalty) for hitting another agent
            Debug.Log($"{gameObject.name} collided with another agent and received a penalty.");
        }
    }


    public override void CollectObservations(VectorSensor sensor)
    {
        var testSensor = sensor ?? new VectorSensor(10); // Replace with new sensor if null
        DetectNearbyObjects(testSensor);
    }


    void DetectNearbyObjects(VectorSensor sensor)
    {

        if (sensor == null)
        {
            return; // Exit early if sensor is null
        }

        Debug.Log($"{gameObject.name} is attempting detection.");

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, soundDetectionRadius, detectableObjectsLayer);

        if (hitColliders == null)
        {
            return;
        }

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider == null)
            {
                continue;
            }

            var hitRigidbody = hitCollider.GetComponent<Rigidbody>();
            if (hitRigidbody == null)
            {
                continue;
            }

            if (hitCollider.gameObject == gameObject)
            {
                // Ignore self-detection
                continue;
            }

            Vector3 directionToTarget = (hitCollider.transform.position - transform.position).normalized;
            float distanceToTarget = Vector3.Distance(transform.position, hitCollider.transform.position);

            // Log the detected object details
            Debug.Log($"{gameObject.name} detected {hitCollider.gameObject.name} at distance {distanceToTarget}");

            if (sensor != null)
            {
                sensor.AddObservation(directionToTarget);
                sensor.AddObservation(distanceToTarget);
            }
            else
            {
                Debug.LogError("Sensor is null in DetectNearbyObjects.");
            }
        }
    }



}
