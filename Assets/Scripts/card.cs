using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public enum Element
    {
        Left, LeftDown,
        LeftUp,
        Right,
        RightDown, RightUp,
        Up,
        Down,
        Bug
    };

    public enum State
    {
        Closed,
        opening,
        Opened
    }

    public State CardState { get; private set; }

    void Update ()
    {
        if (CardState == State.Closed)
        {
            if (Input.GetMouseButtonDown (0))
            {
                Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.gameObject == gameObject)
                        CardState = State.opening;
                }
            }
        }
        else if (CardState == State.opening)
        {
            transform.Rotate (500 * Time.deltaTime * Vector3.up);

            if (transform.rotation.eulerAngles.y == 180)
                CardState = State.Opened;
        }
    }

    public Element ElementToShow { get; set; }
}
