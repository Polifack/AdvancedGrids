using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            WorldGrid.SetColorInPosition(new Vector2Int(Mathf.FloorToInt(mousePos.x), 
                (Mathf.FloorToInt(mousePos.y))), Color.red);
        }
    }
}
