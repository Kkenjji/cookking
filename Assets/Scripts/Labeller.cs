using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteAlways]
public class Labeller : MonoBehaviour
{
    TextMeshPro label;
    public Vector2Int coords = new Vector2Int();
    GridManager gridManager;

    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        label = GetComponentInChildren<TextMeshPro>();

        DisplayCoords();
    }

    void Update()
    {
        DisplayCoords();
        transform.name = coords.ToString();
    }
    private void DisplayCoords()
    {
        if(!gridManager) { return; }
        coords.x = Mathf.RoundToInt(transform.position.x / gridManager.UnityGridSize);
        coords.y = Mathf.RoundToInt(transform.position.z / gridManager.UnityGridSize);

        // label.text = $"{coords.x}, {coords.y}";
    }
}
