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

    Renderer cardRenderer;
    Transform cardTransform;
    
    public Material[] CardMaterials;
    public Transform bugTransform { get; private set; }
    public float TimeRemainder { get; private set; }
    public int Score { get; private set; }

    void Start ()
    {
        cards = FindObjectsByType<Card> (FindObjectsSortMode.None).ToList ();

        StartGame ();
    }

    void StartGame ()
    {
        int bugCardIndex = UnityEngine.Random.Range(0, cards.Count);
        cards[bugCardIndex].ElementToShow = Card.Element.Bug;
        bugTransform = cards[bugCardIndex].transform;
        Debug.Log(cards[bugCardIndex].name);
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
                {
                    cardTransform = card.transform;
                    cardRenderer = cardTransform.GetComponent<Renderer>();
                    card.OnClick();
                }
            }
        }
    }

    float GetAngle()
    {
        Vector3 direction = bugTransform.position - cardTransform.position;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        angle = Mathf.Repeat(angle, 360f);
        return angle;
    }

    public Material GetMaterialForAngle(float angle)
    {
        if (angle % 45 != 0)
        {
            angle = RoundToNearestDiagonal(angle);
        }
        int index = Mathf.RoundToInt(angle / 45f) % CardMaterials.Length;
        return CardMaterials[index];
    }

    private float RoundToNearestDiagonal(float angle)
    {
        float[] diagonalAngles = { 45f, 135f, 225f, 315f };
        float nearestValue = diagonalAngles[0];
        float minDifference = Mathf.Abs(angle - nearestValue);

        foreach (float possibleValue in diagonalAngles)
        {
            float difference = Mathf.Abs(angle - possibleValue);
            if (difference < minDifference)
            {
                minDifference = difference;
                nearestValue = possibleValue;
            }
        }

        return nearestValue;
    }

    public void SetMaterial()
    {
        float angle = GetAngle();
        Material material = GetMaterialForAngle(angle);
        if (cardRenderer != null)
        {
            cardRenderer.material = material;
        }
    }

    void FixedUpdate()
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