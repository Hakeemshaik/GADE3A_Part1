using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    public float moveSpeed;
    public Transform blueBase;
    public Collider dropOffCollider; 
    public Transform flagSpawner;
    public GameObject flagPrefab; 
    public Transform respawnPoint; 
    private GameObject carryingFlag;
    private bool canPickUpFlag = true;
    private bool aiInCollider = false;
    private float aiEnterTime = 0f;
    private float aiTimeThreshold = 2f; 
    private bool gameEnded = false;
    public GameObject playerWinText;

    void Start()
    {
       
    }

    void Update()
    {
        // Movement input
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // Movement
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);


        if (aiInCollider)
        {
            if (Time.time - aiEnterTime > aiTimeThreshold)
            {
                RespawnAndDropFlag();
            }
        }

        
        if (Input.GetKeyDown(KeyCode.E) && carryingFlag != null)
        {
            DropFlag();
        }
    }
    

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Flag") && carryingFlag == null && canPickUpFlag)
        {
            PickUpFlag(other.gameObject);
        }
        else if (other.CompareTag("DropOffCollider") && carryingFlag != null)
        {
            DropOffFlag();
        }
        else if (other.CompareTag("AI"))
        {
            aiInCollider = true;
            aiEnterTime = Time.time;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Flag") && carryingFlag == other.gameObject)
        {
            carryingFlag = null;
        }
        else if (other.CompareTag("AI"))
        {
            aiInCollider = false;
        }
    }

    void PickUpFlag(GameObject flag)
    {
        carryingFlag = flag;
        carryingFlag.transform.parent = transform; 
        carryingFlag.SetActive(false); 
    }

    IEnumerator FlagPickupCooldown()
    {
        canPickUpFlag = false;
        yield return new WaitForSeconds(3f); 
        canPickUpFlag = true;
    }

    void DropFlag()
    {
        carryingFlag.transform.parent = null; 
        carryingFlag.SetActive(true); 
        StartCoroutine(FlagPickupCooldown());
    }

    void DropOffFlag()
    {
        carryingFlag.SetActive(false); 
        carryingFlag.transform.parent = null; 
        SpawnNewFlag();
        Destroy(carryingFlag); 
        carryingFlag = null;

         GameController.playerScore++; // Increase player score
        if (GameController.playerScore < 5)
        {
            gameEnded = false;
            SpawnNewFlag();
         }
         else
         {
            gameEnded = true;
            Time.timeScale = 1f; // Pause the game
           playerWinText.SetActive(true); // Show the player win text
            Invoke(nameof(ReturnToMainMenu), 3f); // Return to main menu after 3 seconds
        }
        
    }
void ReturnToMainMenu()
        {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // Load the main menu scene
        }
    void RespawnAndDropFlag()
    {
        
        transform.position = respawnPoint.position;

        if (carryingFlag != null)
        {
            DropFlag();
        }
    }
    public void DropFlagAtDeath()
    {
        if (carryingFlag != null)
        {
            carryingFlag.transform.parent = null; 
            carryingFlag.transform.position = transform.position; 
            carryingFlag.SetActive(true); 
            carryingFlag = null; 
        }
    }
    public bool IsCarryingFlag()
    {
        return carryingFlag != null;
    }

    void SpawnNewFlag()
    {
        GameObject newFlag = Instantiate(flagPrefab, flagSpawner.position, flagSpawner.rotation); // Spawn flag at flag spawner's position
    }
}