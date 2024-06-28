using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using OpenCover.Framework.Model;

public class UIManager : MonoBehaviour
{
    public GameObject hud;
    public GameObject gameOverScreen;

    void Awake ()
    {
        GameManager.Instance.OnGameStart += OnGameStart;
        GameManager.Instance.OnGameOver += OnGameOver;
    }

    void OnDestroy ()
    {
        GameManager.Instance.OnGameStart -= OnGameStart;
        GameManager.Instance.OnGameOver -= OnGameOver;
    }

    void OnGameStart ()
    {
        hud.SetActive (true);
        gameOverScreen.SetActive (false);
    }

    void OnGameOver (int s)
    {
        hud.SetActive (false);
        gameOverScreen.SetActive (true);

        StartCoroutine (ScoreAnimation ());
    }

    public GameObject scoreText;

    IEnumerator ScoreAnimation ()
    {
        bool increasing = false;

        while (true)
        {
            var scale = scoreText.transform.localScale;

            if (increasing)
            {
                scale.x += Time.fixedDeltaTime;
                scale.y += Time.fixedDeltaTime;

                if (scale.x > 1)
                    increasing = false;
            }
            else
            {
                scale.x -= Time.fixedDeltaTime;
                scale.y -= Time.fixedDeltaTime;

                if (scale.x < 0.7f)
                    increasing = true;
            }

            scoreText.transform.localScale = scale;

            yield return null;
        }
    }
}