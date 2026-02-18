using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private RectTransform boardRect;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GridLayoutGroup grid;

    [SerializeField] private float minCardSize = 20f;
    [SerializeField] private float maxCardSize = 300f;

    [SerializeField] private int currentRows = 4;
    [SerializeField] private int currentCols = 4;

    private bool boardGenerated;
    private bool resizeQueued;

    private void OnRectTransformDimensionsChange()
    {
        if (!boardGenerated) return;

        if (!resizeQueued)
            StartCoroutine(ResizeNextFrame());
    }

    public void GenerateBoard()
    {
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = currentCols;

        ClearBoard();
        ResizeCellsSafe();
        SpawnCards(currentRows * currentCols);

        boardGenerated = true;

        Debug.Log($"Board generated: {currentRows} x {currentCols}");
    }

    private void ResizeCellsSafe()
    {
        StartCoroutine(ResizeNextFrame());
    }

    private void ResizeCells()
    {
        float width = boardRect.rect.width;
        float height = boardRect.rect.height;

        if (width <= 0 || height <= 0) return;

        float totalSpacingX = grid.spacing.x * (currentCols - 1);
        float totalSpacingY = grid.spacing.y * (currentRows - 1);

        float totalPaddingX = grid.padding.left + grid.padding.right;
        float totalPaddingY = grid.padding.top + grid.padding.bottom;

        float cellWidth = (width - totalSpacingX - totalPaddingX) / currentCols;
        float cellHeight = (height - totalSpacingY - totalPaddingY) / currentRows;

        float finalSize = Mathf.Min(cellWidth, cellHeight);
        finalSize = Mathf.Clamp(finalSize, minCardSize, maxCardSize);

        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = currentCols;

        grid.cellSize = new Vector2(finalSize, finalSize);
    }

    private void SpawnCards(int count)
    {
        for (int i = 0; i < count; i++)
            Instantiate(cardPrefab, grid.transform);
    }

    private void ClearBoard()
    {
        for (int i = grid.transform.childCount - 1; i >= 0; i--)
            Destroy(grid.transform.GetChild(i).gameObject);
    }

    private IEnumerator ResizeNextFrame()
    {
        resizeQueued = true;
        yield return new WaitForEndOfFrame();
       
        LayoutRebuilder.ForceRebuildLayoutImmediate(boardRect);
        ResizeCells();
        resizeQueued = false;
    }
}