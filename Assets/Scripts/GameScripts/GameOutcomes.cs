using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOutcomes : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    Transform player;
    private bool isCaptured = false;

    void Start()
    {
        StartCoroutine(WaitTime());
    }

    void FixedUpdate()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        GetComponent<UnityEngine.AI.NavMeshAgent>().destination = player.position;

        float captureDistance = 1f;

        if (!isCaptured && Vector3.Distance(transform.position, player.position) < captureDistance)
        {
            isCaptured = true;
            EndGame();
        }
    }

    void EndGame()
    {
        SceneManager.LoadScene("3 GameOverScene");
    }

    void WinGame()
    {
        SceneManager.LoadScene("4 GameVictoryScene");   
    }

    IEnumerator WaitTime()
    {
        enemy.GetComponent<UnityEngine.AI.NavMeshAgent>().isStopped = true;
        yield return new WaitForSeconds(5f);
        enemy.GetComponent<UnityEngine.AI.NavMeshAgent>().isStopped = false;
    }
}
