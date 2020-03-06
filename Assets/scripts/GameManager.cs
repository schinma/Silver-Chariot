using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum GameState { MENU, PAUSED, PLAYING, WON, LOST };


    public GameObject menuPanel;
    public GameObject gameOverPanel;
    public GameObject pausePanel;
    public GameObject UIPanel;
    public GameObject WinPanel;
    public GameObject helpText;


    private GameState state;

    public static GameManager identity;
    public GameObject player;
    public Transform playerSpawn;
    public GameObject cameraAnchor;
    public PlayerStat stat;
    private Playlist _playlist;
    private GameObject[] foods;
    private GameObject[] allies;

    private bool _isKeyDown = false;

    // Start is called before the first frame update
    void Start()
    {
        if (identity == null)
            identity = this;
        stat.initialize();
        _playlist = GetComponent<Playlist>();
        foods = GameObject.FindGameObjectsWithTag("Food");
        allies = GameObject.FindGameObjectsWithTag("Ally");

        PauseGame();

        menuPanel.SetActive(true);
        UIPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        pausePanel.SetActive(false);
        WinPanel.SetActive(false);
        helpText.SetActive(false);

        state = GameState.MENU;
    }

    // Update is called once per frame
    void Update()
    {
        if ((state == GameState.MENU || state == GameState.LOST || state == GameState.WON) && Input.anyKeyDown)
            StartGame();

        if (Input.GetKeyDown("escape") && !_isKeyDown)
        {
            _isKeyDown = true;
            switch(state)
            {
                case GameState.LOST:
                case GameState.WON:
                    QuitGame();
                    break;
                case GameState.PAUSED:
                    UnpauseGame();
                    break;
                case GameState.PLAYING:
                    PauseGame();
                    break;

            }
        }
        else if (Input.GetKeyUp("escape"))
            _isKeyDown = false;

        if (state == GameState.PLAYING)
        {
            stat.food -= (.3f * (stat.rescued + 1)) * Time.deltaTime;
        }

        if (stat.food <= 0)
            LoseGame();
    }

    public void PauseGame()
    {
        state = GameState.PAUSED;
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        UIPanel.SetActive(false);

    }

    public void UnpauseGame()
    {
        state = GameState.PLAYING;
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        UIPanel.SetActive(false);
    }

    public void StartGame()
    {
        Debug.Log("Start Game");
        state = GameState.PLAYING;
        menuPanel.SetActive(false);
        UIPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        WinPanel.SetActive(false);
        helpText.SetActive(true);
        stat.initialize();
        if (!player.activeSelf)
        {
            ResetPlayer();
        }
        Time.timeScale = 1;
        _playlist.Play();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoseGame()
    {
        Time.timeScale = 0;
        state = GameState.LOST;
        Debug.Log("Game Over");
        gameOverPanel.SetActive(true);
        _playlist.Stop();
    }

    public void WinGame()
    {
        Time.timeScale = 0;
        state = GameState.WON;
        Debug.Log("Win");
        WinPanel.SetActive(true);
    }

    private void ResetPlayer()
    {
        player.transform.position = playerSpawn.position;
        player.transform.rotation = playerSpawn.rotation;
        cameraAnchor.transform.position = playerSpawn.position;
        player.SetActive(true);

        foreach(var ally in player.GetComponent<Movement>()._allies)
        {
            ally.SetActive(false);
        }

        foreach(var food in foods)
        {
            food.SetActive(true);
        }
        foreach (var ally in allies)
        {
            ally.SetActive(true);
        }

        player.GetComponent<Movement>().StartMoving();
    }
}
