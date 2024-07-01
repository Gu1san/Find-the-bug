using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public enum Element
    {
        Empty,
        Bug
    };

    public enum State
    {
        Closed,
        Opened
    }

    public State CardState { get; private set; }
    public Element ElementToShow { get; set; }

    public void OnClick()
    {
        if(CardState == State.Closed)
        {
            StartCoroutine(RotateCard());
        }
    }

    public void ResetCard()
    {
        StartCoroutine(RotateCard());
    }

    IEnumerator RotateCard()
    {
        bool materialApplied = false;
        float targetAngle = Mathf.Abs(transform.rotation.eulerAngles.y) >= 180 ? 360 : 180;
        float initialAngle = transform.rotation.eulerAngles.y;

        while (Mathf.Abs(transform.rotation.eulerAngles.y - targetAngle) > 5)
        {
            transform.Rotate(500 * Time.deltaTime * Vector3.up, Space.World);

            // Aplicar material no meio da rotação
            if (!materialApplied && Mathf.Abs(transform.rotation.eulerAngles.y - (initialAngle + 90)) < 5f)
            {
                materialApplied = true;
                if (CardState == State.Closed)
                {
                    GameManager.Instance.SetMaterial();
                }
                else
                {
                    GetComponent<Renderer>().material = GameManager.Instance.emptyMaterial;
                }
            }

            yield return null;
        }

        Quaternion currentRotation = transform.rotation;
        transform.rotation = Quaternion.Euler(currentRotation.x, targetAngle, currentRotation.z);

        CardState = CardState == State.Closed ? State.Opened : State.Closed;

        if (ElementToShow == Element.Bug)
        {
            GameManager.Instance.ResetCards();
        }
    }

}
