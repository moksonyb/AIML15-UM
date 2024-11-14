using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;
using TMPro;

public class SoccerEnvController : MonoBehaviour
{
    [System.Serializable]
    public class PlayerInfo
    {
        public AgentSoccer Agent;
        [HideInInspector]
        public Vector3 StartingPos;
        [HideInInspector]
        public Quaternion StartingRot;
        [HideInInspector]
        public Rigidbody Rb;
    }


    /// <summary>
    /// Max Academy steps before this platform resets
    /// </summary>
    /// <returns></returns>
    [Tooltip("Max Environment Steps")] public int MaxEnvironmentSteps = 25000;

    /// <summary>
    /// The area bounds.
    /// </summary>

    /// <summary>
    /// We will be changing the ground material based on success/failue
    /// </summary>

    public GameObject ball;
    [HideInInspector]
    public Rigidbody ballRb;
    Vector3 m_BallStartingPos;

    //List of Agents On Platform
    public List<PlayerInfo> AgentsList = new List<PlayerInfo>();

    private SoccerSettings m_SoccerSettings;


    private SimpleMultiAgentGroup m_BlueAgentGroup;
    private SimpleMultiAgentGroup m_PurpleAgentGroup;

    private int m_ResetTimer;

    public TMP_Text scoreDisplay;
    public bool resetScore = false;

    private int blueScoreValue;
    private int purpleScoreValue;

    void Start()
    {
        m_SoccerSettings = FindObjectOfType<SoccerSettings>();

        if (m_SoccerSettings == null)
        {
            Debug.LogError("SoccerSettings not found in the scene.");
        }

        // Initialize TeamManager
        m_BlueAgentGroup = new SimpleMultiAgentGroup();
        m_PurpleAgentGroup = new SimpleMultiAgentGroup();

        if (ball == null)
        {
            Debug.LogError("Ball is not assigned in SoccerEnvController.");
            ball = GameObject.Find("Ball"); // Adjust name if necessary
        }

        if (ball != null)
        {
            ballRb = ball.GetComponent<Rigidbody>();
            m_BallStartingPos = new Vector3(ball.transform.position.x, ball.transform.position.y, ball.transform.position.z);
        }

        if (scoreDisplay == null)
        {
            Debug.LogError("ScoreDisplay (TextMeshProUGUI) is not assigned in SoccerEnvController.");
            scoreDisplay = GetComponentInChildren<TextMeshProUGUI>(); // Adjust if needed
        }

        if (scoreDisplay != null)
        {
            scoreDisplay.text = "Start!";
        }

        if (resetScore)
        {
            blueScoreValue = 0;
            purpleScoreValue = 0;
        }

        if (AgentsList == null || AgentsList.Count == 0)
        {
            Debug.LogError("AgentsList is empty or not assigned in SoccerEnvController.");
            // Optionally populate AgentsList dynamically if needed.
        }

        foreach (var item in AgentsList)
        {
            if (item.Agent != null)
            {
                item.StartingPos = item.Agent.transform.position;
                item.StartingRot = item.Agent.transform.rotation;
                item.Rb = item.Agent.GetComponent<Rigidbody>();

                if (item.Agent.team == Team.Blue)
                {
                    m_BlueAgentGroup.RegisterAgent(item.Agent);
                }
                else
                {
                    m_PurpleAgentGroup.RegisterAgent(item.Agent);
                }
            }
            else
            {
                Debug.LogError("An agent in AgentsList is null.");
            }
        }

        if (scoreDisplay != null)
        {
            scoreDisplay.text = blueScoreValue.ToString() + ":" + purpleScoreValue.ToString();
        }

        ResetScene();
    }


    void FixedUpdate()
    {
        m_ResetTimer += 1;
        if (m_ResetTimer >= MaxEnvironmentSteps && MaxEnvironmentSteps > 0)
        {
            m_BlueAgentGroup.GroupEpisodeInterrupted();
            m_PurpleAgentGroup.GroupEpisodeInterrupted();
            ResetScene();
        }
    }


    public void ResetBall()
    {
        var randomPosX = Random.Range(-2.5f, 2.5f);
        var randomPosZ = Random.Range(-2.5f, 2.5f);

        ball.transform.position = m_BallStartingPos + new Vector3(randomPosX, 0f, randomPosZ);
        ballRb.velocity = Vector3.zero;
        ballRb.angularVelocity = Vector3.zero;

    }

    public void GoalTouched(Team scoredTeam)
    {
        Debug.Log("GoalTouched");
        if (scoredTeam == Team.Blue)
        {
            m_BlueAgentGroup.AddGroupReward(1 - (float)m_ResetTimer / MaxEnvironmentSteps);
            m_PurpleAgentGroup.AddGroupReward(-1);
            blueScoreValue = blueScoreValue + 1;
        }
        else
        {
            m_PurpleAgentGroup.AddGroupReward(1 - (float)m_ResetTimer / MaxEnvironmentSteps);
            m_BlueAgentGroup.AddGroupReward(-1);
            purpleScoreValue = purpleScoreValue + 1;
        }
        m_PurpleAgentGroup.EndGroupEpisode();
        m_BlueAgentGroup.EndGroupEpisode();
        scoreDisplay.text = blueScoreValue.ToString() + ":" + purpleScoreValue.ToString();
        ResetScene();

    }


    public void ResetScene()
    {
        m_ResetTimer = 0;
        ResetBall();

        //Reset Agents
        foreach (var item in AgentsList)
        {
            var randomPosX = Random.Range(-5f, 5f);
            var newStartPos = item.Agent.initialPos + new Vector3(randomPosX, 0f, 0f);
            var rot = item.Agent.rotSign * Random.Range(80.0f, 100.0f);
            var newRot = Quaternion.Euler(0, rot, 0);
            item.Agent.transform.SetPositionAndRotation(newStartPos, newRot);

            item.Rb.velocity = Vector3.zero;
            item.Rb.angularVelocity = Vector3.zero;
        }
    }
}
