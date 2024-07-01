using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using OpenCover.Framework.Model;

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
}