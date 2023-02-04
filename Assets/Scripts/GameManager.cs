using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public List<GameObject> truffles;
    PlayerMovement playerMovement;

    public TMP_Text truffleText;
    int numTrufflesLeft = 0;

    bool gamePaused = false;
    public GameObject pausePanel;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        truffles = new List<GameObject>(GameObject.FindGameObjectsWithTag("Truffle"));
        foreach (var truffle in truffles)
        {
            truffle.GetComponent<TruffleScript>().collected.AddListener(OnTruffleCollected);
        }
        //subscribe to each truffles collected event
        Debug.Log($"Number of Truffles = {truffles.Count}");
        numTrufflesLeft = truffles.Count;
        truffleText.text = $"Truffles: {numTrufflesLeft}";


        SetPaused(gamePaused);
    }

    private void Update()
    {
        //look for an escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gamePaused = !gamePaused;
            SetPaused(gamePaused);
        }
    }



    private void OnApplicationQuit()
    {
        SetPaused(true);
    }

    public GameObject FindNearestTruffle(Vector3 position)
    {
        //make sure there IS a truffle before calling this or it will return 0,0,0
        GameObject closest = null;
        float distance = Mathf.Infinity;
        foreach (var truffle in truffles)
        {
            float d = Vector3.Distance(truffle.transform.position, position);
            if(d < distance)
            {
                //this one is closer
                closest = truffle;
                distance = d;
            }
        }
        return closest;
    }

    void OnTruffleCollected(GameObject go)
    {
        Debug.Log("Collected");
        truffles.Remove(go);
        playerMovement.closestTruffle = FindNearestTruffle(playerMovement.transform.position);
        numTrufflesLeft--;
        truffleText.text = $"Truffles: {numTrufflesLeft}";
        if(numTrufflesLeft <= 0)
        {
            StartCoroutine(LoadWinScene());
        }
    }
 

    IEnumerator LoadWinScene()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("WinScene");
    }



    void SetPaused(bool _paused)
    {
        gamePaused = _paused;
        Cursor.lockState = _paused ? CursorLockMode.None : CursorLockMode.Locked;
        pausePanel.SetActive(_paused);
        Cursor.visible = _paused;
    }

}
