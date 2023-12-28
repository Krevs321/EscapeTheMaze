using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{    
    public float threshold;
    [SerializeField] MazeGenerator mazeGenerator;
    [SerializeField] MazeRenderer mazeRenderer;

    // If you fall in the hole and reach the threshold you die
    void Update()
    {

        if(transform.position.y < threshold)
        {
            transform.position = new Vector3(0f, 0f, 0f);
            SceneManager.LoadScene("3 GameOverScene");
        }
    }

}
