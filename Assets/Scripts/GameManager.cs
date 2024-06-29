using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager instance;

    public static GameManager Instance => instance??FindObjectOfType<GameManager>();

    public event Action OnGameStart;
    public event Action<int> OnGameOver;

    List<Card> cards;
    bool gameIsRunning;
    
    public float TimeRemainder { get; private set; }
    public int Score { get; private set; }

    void Start ()
    {
        cards = FindObjectsByType<Card> (FindObjectsSortMode.None).ToList ();

        StartGame ();
    }

    void StartGame ()
    {
        int bugCardIndex = UnityEngine.Random.Range (0, cards.Count);
        cards[bugCardIndex].ElementToShow = Card.Element.Bug;

        TimeRemainder = 60;
        gameIsRunning = true;

        OnGameStart?.Invoke ();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject.TryGetComponent(out Card card))
                    card.OnClick();
            }
        }
    }

    void FixedUpdate ()
    {
        if (!gameIsRunning)
            return;

        TimeRemainder -= Time.deltaTime;

        if (TimeRemainder < 0)
        {
            gameIsRunning = false;
            OnGameOver?.Invoke (Score);
        }
    }
}