using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField][Range(25, 55)] int cardsAmount = 55;
    [SerializeField] GameObject cardObject;
    [SerializeField] float spacing = 0.1f; // Espaçamento entre as cartas
    Vector2 cardSize;

    private void Start()
    {
        Mesh cardMesh = cardObject.GetComponent<MeshFilter>().sharedMesh;
        Vector3 cardScale = cardObject.transform.localScale;
        cardSize = new Vector2(cardMesh.bounds.size.x * cardScale.x, cardMesh.bounds.size.y * cardScale.y);
        FillBoard();
    }

    void FillBoard()
    {
        float screenWidth = Camera.main.orthographicSize * 2f * Camera.main.aspect;
        int columns = Mathf.FloorToInt(screenWidth / (cardSize.x + spacing));
        columns = Mathf.Clamp(columns, 1, cardsAmount);
        int rows = Mathf.CeilToInt((float)cardsAmount / columns);

        Vector3 startPos = transform.position - new Vector3((columns - 1) * (cardSize.x + spacing) / 2, (rows - 1) * (cardSize.y + spacing) / -2, 0);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                int index = i * columns + j;
                if (index >= cardsAmount)
                    return;

                Vector3 pos = startPos + new Vector3(j * (cardSize.x + spacing), -i * (cardSize.y + spacing), 0);

                Instantiate(cardObject, pos, Quaternion.identity, transform);
            }
        }
    }
}
