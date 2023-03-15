using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAreaMeteor : MonoBehaviour
{
    public GameObject[] eventUI;

    public void CheckArea(GameEvent gameEvent)
    {
        var gameEventArea = (GameEventArea)gameEvent;
        switch (gameEventArea.GetArea()[0])

        {
            case Area.TopLeft:
                {
                    LeftUpArea();
                    break;
                }

            case Area.BottomLeft:
                {
                    LeftDownArea();
                    break;
                }
            case Area.TopRight:
                {
                    RightUpArea();
                    break;
                }
            case Area.BottomRight:
                {
                    RightDownArea();
                    break;
                }

        }
    }

    public void LeftUpArea()
    {
        eventUI[0].SetActive(true);
        eventUI[0].GetComponent<UIDeactivate>().OnActivate();
    }
    public void LeftDownArea()
    {
        eventUI[1].SetActive(true);
        eventUI[1].GetComponent<UIDeactivate>().OnActivate();
    }
    public void RightUpArea()
    {
        eventUI[2].SetActive(true);
        eventUI[2].GetComponent<UIDeactivate>().OnActivate();
    }
    public void RightDownArea()
    {
        eventUI[3].SetActive(true);
        eventUI[3].GetComponent<UIDeactivate>().OnActivate();
    }

}
