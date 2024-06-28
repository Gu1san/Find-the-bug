using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class card : MonoBehaviour
{
    public enum element
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

    public State _state { get; private set; }

    void Update ()
    {
        if (_state == State.Closed)
        {
            if (Input.GetMouseButtonDown (0))
            {
                Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast (ray, out hit))
                {
                    if (hit.collider.gameObject == gameObject)
                        _state = State.opening;
                }
            }
        }
        else if (_state == State.opening)
        {
            transform.Rotate (Vector3.up * Time.deltaTime * 500);

            if (transform.rotation.eulerAngles.y == 180)
                _state = State.Opened;
        }
    }

    public element ElementToShow { get; set; }
}
