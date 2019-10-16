using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    public WorldGrid grid;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            grid.SetColorInPosition(new Vector2Int(Mathf.FloorToInt(mousePos.x), (Mathf.FloorToInt(mousePos.y))), Color.red);
        }
    }
}
