using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using OpenCover.Framework.Model;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public GameObject hud;
    public GameObject gameOverScreen;
    public GameObject scoreObject;
    TMP_Text scoreText;
    [SerializeField] float scoreAnimationSpeed = 0.2f;

    void Awake ()
    {
        GameManager.Instance.OnGameStart += OnGameStart;
        GameManager.Instance.OnGameOver += OnGameOver;
        scoreText = scoreObject.GetComponent<TMP_Text>();
    }

    void OnDestroy ()
    {
        if (GameManager.Instance == null) return;
        GameManager.Instance.OnGameStart -= OnGameStart;
        GameManager.Instance.OnGameOver -= OnGameOver;
    }

    void OnGameStart ()
    {
        hud.SetActive(true);
        gameOverScreen.SetActive(false);
    }

    void OnGameOver (int s)
    {
        hud.SetActive(false);
        gameOverScreen.SetActive(true);
        scoreText.text = s.ToString();
        StartCoroutine(ScoreAnimation());
        StartCoroutine(PostScore("https://nphy.cc:5580/score"));
    }

    IEnumerator ScoreAnimation()
    {
        bool increasing = false;

        while (true)
        {
            Vector3 scale = scoreText.transform.localScale;
            float speed = Time.fixedDeltaTime * scoreAnimationSpeed;
            if (increasing)
            {
                scale.x += speed;
                scale.y += speed;

                if (scale.x > 1)
                    increasing = false;
            }
            else
            {
                scale.x -= speed;
                scale.y -= speed;

                if (scale.x < 0.7f)
                    increasing = true;
            }

            scoreText.transform.localScale = scale;

            yield return null;
        }
    }

    IEnumerator PostScore(string url)
    {
        string jsonData = "{\"score\":" + GameManager.Instance.Score + "}";
        UnityWebRequest uwr = new(url, "POST");

        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
        uwr.uploadHandler = new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = new DownloadHandlerBuffer();

        uwr.SetRequestHeader("Content-Type", "application/json");
        uwr.SetRequestHeader("Authorization", "findTHeBuG123");

        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
        }
    }
}