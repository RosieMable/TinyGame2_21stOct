using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    [SerializeField] CanvasGroup canvasGroup;

    [SerializeField] float fadeTime = 2f;

    public static bool paused;

    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject OptionsMenu;

    public CanvasGroup gameOverScreen;
    public CanvasGroup victoryScreen;

    public GameObject gameScene;

    private void Start()
    {

    }
    private void Awake()
    {
        canvasGroup = this.GetComponentInChildren<CanvasGroup>();
        PauseMenu.SetActive(false);
        OptionsMenu.SetActive(false);

        gameOverScreen.alpha = 0;
        gameOverScreen.blocksRaycasts = false;
        gameOverScreen.interactable = false;

        victoryScreen.alpha = 0;
        victoryScreen.blocksRaycasts = false;
        victoryScreen.interactable = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            OnPause();
    }

    public void OnStart()
    {
        StartCoroutine(FadeThenDoSomething());
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnQuit()
    {
        Application.Quit();
    }

    public void OnPause()
    {
        PauseGame();
        Debug.Log(Time.timeScale);
    }

    public void OnOptions()
    {
        OptionsMenu.SetActive(true);
    }

    public void ExitOptions()
    {
        OptionsMenu.SetActive(false);
    }

    public void OnGameOver()
    {
        gameOverScreen.blocksRaycasts = true;
        gameOverScreen.interactable = true;
        StartCoroutine(FadeCanvas(gameOverScreen, 0, 1, 0.6f));
        gameScene.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }

    public void OnLevelComplete()
    {
        victoryScreen.blocksRaycasts = true;
        victoryScreen.interactable = true;
        StartCoroutine(FadeCanvas(victoryScreen, 0, 1, 0.6f));
        gameScene.SetActive(false);
    }

    public void OnRetry()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    void PauseGame()
    {
        if (paused == false && canvasGroup.gameObject.activeSelf == false)
        {
            Time.timeScale = 0f;
            PauseMenu.SetActive(true);
            paused = true;
        }
        else
        {
            Time.timeScale = 1f;
            PauseMenu.SetActive(false);
            paused = false;
        }
    }

    IEnumerator FadeThenDoSomething()
    {
        yield return StartCoroutine(FadeCanvas(canvasGroup, 1f, 0f, fadeTime));

        //do something here

        canvasGroup.gameObject.SetActive(false);

        ToActivateOnStart();
    }

    IEnumerator FadeCanvas(CanvasGroup canvas, float startAlpha, float endAlpha, float duration)
    {
        var startTime = Time.time;
        var endTime = Time.time + duration;
        var elapsedTime = 0f;

        canvas.alpha = startAlpha;

        while (Time.time <= endTime)
        {
            elapsedTime = Time.time - startTime;
            var percentage = 1 / (duration / elapsedTime);
            if (startAlpha > endAlpha)
            {
                canvas.alpha = startAlpha - percentage;
            }
            else
            {
                canvas.alpha = startAlpha + percentage;
            }
            yield return new WaitForEndOfFrame();
        }
        canvas.alpha = endAlpha;
    }

    void ToActivateOnStart()
    {
    }

    private void OnDisable()
    {
    }


}