using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public int levelNumber;

    public GameObject player;
    public Rigidbody playerRigidboty;

    private bool wallhack = false;

    // Use this for initialization
    void Start ()
  
    {
        levelNumber = SceneManager.GetActiveScene().buildIndex;
        playerRigidboty = player.GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        //only if debug on
        if (Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        }
    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            //Turn off colisisons
            wallhack = !wallhack; //on of

            playerRigidboty.detectCollisions = !wallhack;

        }
    }

    public void LoadNextLevel()
    {
        levelNumber++;
        if (levelNumber == SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(levelNumber);
        }
    }

    public void RestartGame()
    {

        SceneManager.LoadScene(0);
    }

}
