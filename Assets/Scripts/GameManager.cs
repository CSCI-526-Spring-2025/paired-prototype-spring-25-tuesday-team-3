using UnityEngine;
using TMPro;
using System.Diagnostics;


public class GameManager : MonoBehaviour
{
    public string player1 = "P1";
    public string player2 = "P2";
    private int currentPlayer = 1;

    public int numRows = 10;
    public int numCols = 10;
    public float spacing = 10.0f;
    public float dotSize = 4.0f;
    public float lineSize = 1.0f;

    private bool[,] horizontalLines;
    private bool[,] verticalLines;
    
    public GameObject playerNamePrefab;
    public GameObject dot;
    public GameObject line;
    public Transform canvasTransform;

    void Start()
    {
        horizontalLines = new bool[numRows, numCols - 1];
        verticalLines = new bool[numRows - 1, numCols];
        populateGrid();
    }

    void populateGrid()
    {
        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numCols; col++)
            {
                // Create dots
                GameObject dotObj = Instantiate(dot, canvasTransform);
                float dotX = (float)(col * spacing - spacing * (numCols / 2 - 0.5));
                float dotY = (float)(row * spacing - spacing * (numRows / 2 - 0.5));
                dotObj.transform.localPosition = new Vector3(dotX, dotY, 0);
                dotObj.transform.localScale = new Vector3(dotSize, dotSize, dotSize);

                // Create vertical lines
                if (row != numRows - 1)
                {
                    GameObject vLineObj = Instantiate(line, canvasTransform);
                    RectTransform vLineRect = vLineObj.GetComponent<RectTransform>();
                    vLineRect.localPosition = new Vector3(dotX, dotY + spacing / 2, 0);
                    vLineRect.sizeDelta = new Vector2(lineSize, spacing - dotSize / 2 - 1);

                    RectTransform vLineImageRect = vLineObj.transform.GetChild(0).GetComponent<RectTransform>();
                    vLineImageRect.sizeDelta = vLineRect.sizeDelta;

                    LineController lineController = vLineObj.GetComponent<LineController>();
                    lineController.isHorizontal = false;
                    lineController.row = row;
                    lineController.col = col;
                }

                // Create horizontal lines
                if (col != numCols - 1)
                {
                    GameObject hLineObj = Instantiate(line, canvasTransform);
                    RectTransform hLineRect = hLineObj.GetComponent<RectTransform>();
                    hLineRect.localPosition = new Vector3(dotX + spacing / 2, dotY, 0);
                    hLineRect.sizeDelta = new Vector2(spacing - dotSize / 2 - 1, lineSize);

                    RectTransform hLineImageRect = hLineObj.transform.GetChild(0).GetComponent<RectTransform>();
                    hLineImageRect.sizeDelta = hLineRect.sizeDelta;

                    LineController lineController = hLineObj.GetComponent<LineController>();
                    lineController.isHorizontal = true;
                    lineController.row = row;
                    lineController.col = col;
                }
            }
        }
    }

    public void HandleClick(int row, int col, bool isHorizontal)
    {
        if (isHorizontal)
            horizontalLines[numRows - 1 - row, col] = true;
        else
            verticalLines[numRows - 2 - row, col] = true;

        UnityEngine.Debug.Log("CLICK AT ROW " + (numRows - 1 - row) + ", COL " + col + " " + (isHorizontal ? "HORIZONTAL" : "VERTICAL"));
        //UnityEngine.Debug.Log("horizontal lines array");
        //printGrid(horizontalLines);
        //UnityEngine.Debug.Log("vertical lines array");
        //printGrid(verticalLines);

        // if box is completed
        bool[] directions = boxComplete(numRows - 1 - row, col, isHorizontal);
        if (directions[0] || directions[1] || directions[2] || directions[3])
        {
            float dotX = (float)(col * spacing - spacing * (numCols / 2 - 0.5));
            float dotY = (float)(row * spacing - spacing * (numRows / 2 - 0.5));           

            // top
            if (directions[0])
            {
                UnityEngine.Debug.Log("top box");
                GameObject textObj = Instantiate(playerNamePrefab, canvasTransform);
                TextMeshProUGUI playerText = textObj.GetComponent<TextMeshProUGUI>();
                RectTransform textRect = textObj.GetComponent<RectTransform>();
                textRect.sizeDelta = new Vector2(spacing, spacing);
                playerText.text = (currentPlayer == 1) ? player1 : player2;
                playerText.transform.localPosition = new Vector3(dotX + spacing/2, dotY + spacing / 2, 0);
            }
            // bottom
            if (directions[1])
            {
                UnityEngine.Debug.Log("bottom box");
                GameObject textObj = Instantiate(playerNamePrefab, canvasTransform);
                TextMeshProUGUI playerText = textObj.GetComponent<TextMeshProUGUI>();
                RectTransform textRect = textObj.GetComponent<RectTransform>();
                textRect.sizeDelta = new Vector2(spacing, spacing);
                playerText.text = (currentPlayer == 1) ? player1 : player2;
                playerText.transform.localPosition = new Vector3(dotX + spacing / 2, dotY - spacing / 2, 0);
            }
            // left
            if (directions[2])
            {
                UnityEngine.Debug.Log("left box");
                GameObject textObj = Instantiate(playerNamePrefab, canvasTransform);
                TextMeshProUGUI playerText = textObj.GetComponent<TextMeshProUGUI>();
                RectTransform textRect = textObj.GetComponent<RectTransform>();
                textRect.sizeDelta = new Vector2(spacing, spacing);
                playerText.text = (currentPlayer == 1) ? player1 : player2;
                playerText.transform.localPosition = new Vector3(dotX - spacing / 2, dotY + spacing / 2, 0);
            }
            // right
            if (directions[3])
            {
                UnityEngine.Debug.Log("right box");
                GameObject textObj = Instantiate(playerNamePrefab, canvasTransform);
                TextMeshProUGUI playerText = textObj.GetComponent<TextMeshProUGUI>();
                RectTransform textRect = textObj.GetComponent<RectTransform>();
                textRect.sizeDelta = new Vector2(spacing, spacing);
                playerText.text = (currentPlayer == 1) ? player1 : player2;
                playerText.transform.localPosition = new Vector3(dotX + spacing / 2, dotY + spacing / 2, 0);
            }
            
        }
        // otherwise, switch turns
        else
        {
            currentPlayer = (currentPlayer == 1) ? 2 : 1;
        }
    }

    bool[] boxComplete(int row, int col, bool isHorizontal)
    {
        bool[] directions = new bool[4];
       
        if(isHorizontal)
        {
            // check top box if any
            if (row != 0 && horizontalLines[row - 1, col] && verticalLines[row - 1, col] && verticalLines[row - 1, col + 1])  directions[0] = true;

            // check bottom box if any
            if (row != numRows - 1 && horizontalLines[row + 1, col] && verticalLines[row, col] && verticalLines[row, col + 1]) directions[1] = true;
        }
        else
        {
            // check left box if any
            if (col != 0 && verticalLines[row - 1, col - 1] && horizontalLines[row, col - 1] && horizontalLines[row - 1, col - 1]) directions[2] = true;

            // check right box if any
            if (col != numCols - 1 && verticalLines[row - 1, col + 1] && horizontalLines[row, col] && horizontalLines[row - 1, col]) directions[3] = true;
        }

        return directions;
    }

    void printGrid(bool[,] grid)
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            string row = "";
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                row += grid[i, j] + " ";
            }
            UnityEngine.Debug.Log(row);
        }
    }

}
