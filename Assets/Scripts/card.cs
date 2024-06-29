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
        Opening,
        Opened
    }

    public State CardState { get; private set; }
    public Element ElementToShow { get; set; }

    public void OnClick()
    {
        if(CardState == State.Closed)
        {
            CardState = State.Opening;
            StartCoroutine(OpenCard());
        }
    }

    IEnumerator OpenCard()
    {
        bool materialApplied = false;
        while(transform.rotation.eulerAngles.y < 180)
        {
            transform.Rotate(500 * Time.deltaTime * Vector3.up);
            if(transform.rotation.eulerAngles.y > 90 && !materialApplied)
            {
                materialApplied = true;
                GameManager.Instance.SetMaterial();
            }
            yield return null;
        }
        Quaternion currentRotation = transform.rotation;
        transform.rotation = Quaternion.Euler(currentRotation.x, 180, currentRotation.z);
        CardState = State.Opened;
        if (ElementToShow == Element.Bug)
        {
            Debug.Log("Inseto encontrado");
        }
    }
}
