using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum AIState
{
    Idle,
    ChaseFlag,
    ReturnFlag
}

public class AIController : MonoBehaviour
{
    public Text AIScoreText;
    public AIState currentState = AIState.Idle;
    public Transform redFlag;
    public Transform redBase;
    public Transform blueBase;
    public Collider dropOffCollider; // Reference to the drop-off zone collider
    public Transform flagSpawner; // Reference to the flag spawner
    private bool carryingFlag = false;
    private NavMeshAgent navMeshAgent;
    public GameObject enemyWonText;
    public int AIScore = 0;

    public GameObject flagPrefab; // Reference to the flag prefab

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = 5f; // Set AI speed
        currentState = AIState.ChaseFlag; // Set initial state to ChaseFlag
    }

    void Update()
    {
        AIScoreText.text = "AI Score: " + AIScore;
        switch (currentState)
        {
            case AIState.Idle:
                LookForFlag();
                break;
            case AIState.ChaseFlag:
                ChaseFlag();
                break;
            case AIState.ReturnFlag:
                ReturnFlag();
                break;
        }

        // Check for score condition
        if (GameController.AIScore < 5 && currentState == AIState.Idle)
        {
            currentState = AIState.ChaseFlag;
        }
    }

    void LookForFlag()
    {
        if (!carryingFlag && Vector3.Distance(transform.position, redFlag.position) < 10f)
        {
            currentState = AIState.ChaseFlag;
        }
    }

    void ChaseFlag()
    {
        if (!carryingFlag)
        {
            navMeshAgent.SetDestination(redFlag.position);
        }
    }

    void ReturnFlag()
    {
        if (carryingFlag)
        {
            navMeshAgent.SetDestination(redBase.position);
            if (Vector3.Distance(transform.position, redBase.position) < 1f)
            {
                carryingFlag = false;
                GameController.AIScore++; // Increase AI score
                currentState = AIState.Idle;
            }
            else if (dropOffCollider != null && dropOffCollider.bounds.Contains(transform.position))
            {
                DropFlag();
            }
        }
    }

    void DropFlag()
    {
        carryingFlag = false;
        redFlag.gameObject.SetActive(true); // Show the flag when dropped off
        redFlag.position = flagSpawner.position; // Spawn flag at flag spawner's position
        currentState = AIState.Idle;

        // Spawn a new flag
        Instantiate(flagPrefab, flagSpawner.position, flagSpawner.rotation);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Flag") && !carryingFlag)
        {
            carryingFlag = true;
            other.gameObject.SetActive(false); // Hide the flag when captured
            currentState = AIState.ReturnFlag;
        }
        else if (other.CompareTag("redDropOffCollider") && carryingFlag)
        {
            carryingFlag = false;
            AIScore++; // Increase AI score
            DropFlag();

            // Check if the AI has won
            if (AIScore >= 5)
            {
                // Display "Enemy Won" text (assuming you have a reference to it)
                enemyWonText.SetActive(true);

                // Wait for 3 seconds before returning to the main menu scene
                Invoke("ReturnToMainMenu", 3f);
            }
        }
    }

    void ReturnToMainMenu()
    {
        // Load the main menu scene
        SceneManager.LoadScene("MainMenu");
    }
}