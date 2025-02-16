using UnityEngine;
using TMPro;
using System.Diagnostics;
using System.Collections.Generic;



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
    public TextMeshProUGUI player1ScoreText;
    public TextMeshProUGUI player2ScoreText;
    public TextMeshProUGUI winner;
    private int player1Score = 0;
    private int player2Score = 0;
    private int count = 0;
    private int prev_count = 0;


    HashSet<Vector2Int> selectedNodes = new HashSet<Vector2Int>();

    void Start()
    {
        horizontalLines = new bool[numRows, numCols - 1];
        verticalLines = new bool[numRows - 1, numCols];
        populateGrid();
        PlaceRandomCircles();
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
        // bool[] directions = boxComplete(numRows - 1 - row, col, isHorizontal);
        // if (directions[0] || directions[1] || directions[2] || directions[3])
        // {
        //     float dotX = (float)(col * spacing - spacing * (numCols / 2 - 0.5));
        //     float dotY = (float)(row * spacing - spacing * (numRows / 2 - 0.5));           

        //     // top
        //     if (directions[0])
        //     {
        //         UnityEngine.Debug.Log("top box");
        //         GameObject textObj = Instantiate(playerNamePrefab, canvasTransform);
        //         TextMeshProUGUI playerText = textObj.GetComponent<TextMeshProUGUI>();
        //         RectTransform textRect = textObj.GetComponent<RectTransform>();
        //         textRect.sizeDelta = new Vector2(spacing, spacing);
        //         playerText.text = (currentPlayer == 1) ? player1 : player2;
        //         playerText.transform.localPosition = new Vector3(dotX + spacing/2, dotY + spacing / 2, 0);
        //     }
        //     // bottom
        //     if (directions[1])
        //     {
        //         UnityEngine.Debug.Log("bottom box");
        //         GameObject textObj = Instantiate(playerNamePrefab, canvasTransform);
        //         TextMeshProUGUI playerText = textObj.GetComponent<TextMeshProUGUI>();
        //         RectTransform textRect = textObj.GetComponent<RectTransform>();
        //         textRect.sizeDelta = new Vector2(spacing, spacing);
        //         playerText.text = (currentPlayer == 1) ? player1 : player2;
        //         playerText.transform.localPosition = new Vector3(dotX + spacing / 2, dotY - spacing / 2, 0);
        //     }
        //     // left
        //     if (directions[2])
        //     {
        //         UnityEngine.Debug.Log("left box");
        //         GameObject textObj = Instantiate(playerNamePrefab, canvasTransform);
        //         TextMeshProUGUI playerText = textObj.GetComponent<TextMeshProUGUI>();
        //         RectTransform textRect = textObj.GetComponent<RectTransform>();
        //         textRect.sizeDelta = new Vector2(spacing, spacing);
        //         playerText.text = (currentPlayer == 1) ? player1 : player2;
        //         playerText.transform.localPosition = new Vector3(dotX - spacing / 2, dotY + spacing / 2, 0);
        //     }
        //     // right
        //     if (directions[3])
        //     {
        //         UnityEngine.Debug.Log("right box");
        //         GameObject textObj = Instantiate(playerNamePrefab, canvasTransform);
        //         TextMeshProUGUI playerText = textObj.GetComponent<TextMeshProUGUI>();
        //         RectTransform textRect = textObj.GetComponent<RectTransform>();
        //         textRect.sizeDelta = new Vector2(spacing, spacing);
        //         playerText.text = (currentPlayer == 1) ? player1 : player2;
        //         playerText.transform.localPosition = new Vector3(dotX + spacing / 2, dotY + spacing / 2, 0);
        //     }
            
        // }
        // otherwise, switch turns
        // else
        // {
            
        // }
        generateCirclePositions();
        currentPlayer = (currentPlayer == 1) ? 2 : 1;
    }

    void generateCirclePositions(){
        HashSet<Vector2Int> newSelectedNodes = new HashSet<Vector2Int>();

        foreach (Vector2Int node in selectedNodes)
        {
            int newRow = node.x;
            int newCol = node.y;
            bool moved = false;

            while (!moved)
            {
                List<int> moveOrder = new List<int> { 0, 1, 2, 3 }; // Possible moves
                moveOrder.Sort((a, b) => Random.Range(-1, 2)); // Shuffle the order
                
                foreach (int move in moveOrder) 
                {   
                    // Move down if no horizontal line below
                    if (move == 0 && newRow >= 1 && !horizontalLines[numRows - 1 - newRow, newCol] && !newSelectedNodes.Contains(new Vector2Int(newRow - 1, newCol)))
                    {
                        newRow--;
                        moved = true;
                        break;
                    }
                    // Move up if no horizontal line above
                    else if (move == 1 && newRow < numRows - 2 && !horizontalLines[numRows - 1 - (newRow + 1), newCol] && !newSelectedNodes.Contains(new Vector2Int(newRow + 1, newCol)))
                    {
                        newRow++;
                        moved = true;
                        break;
                    }
                    // Move right if no vertical line to the right
                    else if (move == 2 && newCol < numCols - 2 && !verticalLines[numRows - 2 - newRow, newCol + 1] && !newSelectedNodes.Contains(new Vector2Int(newRow, newCol + 1)))
                    {
                        newCol++;
                        moved = true;
                        break;
                    }
                    // Move left if no vertical line to the left
                    else if (move == 3 && newCol >= 1 && !verticalLines[numRows - 2 - newRow, newCol] && !newSelectedNodes.Contains(new Vector2Int(newRow, newCol - 1)))
                    {
                        newCol--;
                        moved = true;
                        break;
                    }
                }

                // If all four directions are blocked, exit the loop without moving
                if (!moved)
                {
                    UnityEngine.Debug.Log("No valid move for node at: " + newRow + ", " + newCol);
                    count++;
                    if (count > prev_count) // Ensures score updates only when a new circle is trapped
                    {
                        UpdateScore(currentPlayer);
                        // prev_count = count;
                    }
                    break;
                }
                prev_count = count;
                // UpdateScore(currentPlayer);
            }            
            // UpdateScore(currentPlayer);
            newSelectedNodes.Add(new Vector2Int(newRow, newCol));
            // UpdateScore(currentPlayer);

        }

        // Replace old nodes with new nodes
        selectedNodes = newSelectedNodes;

        // Move circles in the UI to new positions
        UpdateCirclePositions();
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
            //UnityEngine.Debug.Log(row);
        }
    }

    void PlaceRandomCircles()
    {
        //HashSet<Vector2Int> selectedNodes = new HashSet<Vector2Int>();
        while (selectedNodes.Count < 3)
        {
            int randomRow = Random.Range(1, numRows-1);
            int randomCol = Random.Range(1, numCols-1);
            Vector2Int newNode = new Vector2Int(randomRow, randomCol);

            if (!selectedNodes.Contains(newNode))
            {
                selectedNodes.Add(newNode);
                CreateCircleAtNode(randomRow, randomCol);
            }
        }
    }

    void CreateCircleAtNode(int row, int col)
    {
        float dotX = col * spacing - spacing * (numCols / 2 - 0.5f);
        float dotY = row * spacing - spacing * (numRows / 2 - 0.5f);

        float centerX = dotX + spacing / 2; 
        float centerY = dotY + spacing / 2;
          // Instantiate a dot as a circle
        GameObject circleObj = Instantiate(dot, canvasTransform);
        circleObj.name = "Circle"; 
        circleObj.transform.localPosition = new Vector3(centerX,centerY, 0);
        circleObj.transform.localScale = new Vector3(dotSize * 1.5f, dotSize * 1.5f, dotSize * 1.5f); 

        // Change the color to visually distinguish the circle from normal dots
        SpriteRenderer circleRenderer = circleObj.GetComponent<SpriteRenderer>();
        if (circleRenderer != null)
        {
            circleRenderer.color = Color.red; // Change color to red or any other visible color
        }
    }

    void UpdateCirclePositions()
    {
        List<GameObject> circlesToRemove = new List<GameObject>();

        // Identify existing circles based on name
        foreach (Transform child in canvasTransform)
        {
            if (child.gameObject.name.Contains("Circle"))
            {
                circlesToRemove.Add(child.gameObject);
            }
        }

        // Destroy the old circles
        foreach (GameObject circle in circlesToRemove)
        {
            Destroy(circle);
        }

        // Re-create circles at new positions
        foreach (Vector2Int node in selectedNodes)
        {
            CreateCircleAtNode(node.x, node.y);
        }
    }
    void UpdateScore(int player)
    {
        if(player1Score<=2 && player2Score<=2){
            if (player == 1)
            {

                player1Score+=1;
                player1ScoreText.text = $"{player1}: {player1Score}";
            }
            else
            {
                player2Score+=1;
                player2ScoreText.text = $"{player2}: {player2Score}";
            }
        }else{
            // winner.text = (player1Score > 2 ) ? $"PLAYER {player1} WINS!!" : $"PLAYER {player2} WINS!!";
            if(player1Score>=2){
                winner.text = $"PLAYER {player1} WINS!!";
            }else if(player2Score>=2){
                winner.text = $"PLAYER {player2} WINS!!";
            }    
        }
    }
    }

