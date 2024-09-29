using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    private Card firstFlippedCard;
    private Card secondFlippedCard;
    private GridLayoutGroup gridLayout;
    private RectTransform boardRect;
    
    public GameObject cardPrefab;
    public Transform gameBoard;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;

    private int matchesFound = 0;
    private int totalMatches;
    private bool isProcessing = false;

    public bool IsProcessing
    {
        get { return isProcessing; }
    }

    private List<int> cardValues = new List<int>();
    
    private int score = 0;
    private int comboMultiplier = 1;

    public int Score
    {
        get { return score; }
        set { score = value; }
    }

    public int ComboMultiplier
    {
        get { return comboMultiplier; }
        set { comboMultiplier = value; }
    }

    private void Awake()
    {
        boardRect = gameBoard.GetComponent<RectTransform>();
    }

    private void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        int gridRows = Random.Range(2, 4);
        int gridCols = Random.Range(2, 4);
        totalMatches = (gridRows * gridCols) / 2;

        gridLayout = gameBoard.GetComponent<GridLayoutGroup>();

        gridLayout.constraintCount = gridCols;
        
        for (int i = 0; i < totalMatches; i++)
        {
            cardValues.Add(i);
            cardValues.Add(i);
        }
        Shuffle(cardValues);

        SetupScreenSize(gridRows, gridCols);
        
        foreach (int value in cardValues)
        {
            GameObject cardObj = Instantiate(cardPrefab, gameBoard);
            Card card = cardObj.GetComponent<Card>();
            card.cardValue = value;
        }
    }

    public void CardFlipped(Card card)
    {
        if (firstFlippedCard == null)
        {
            firstFlippedCard = card;
            return;
        }

        secondFlippedCard = card;
        StartCoroutine(CheckMatch());
    }

    private IEnumerator CheckMatch()
    {
        isProcessing = true;
        gridLayout.enabled = false;
        
        yield return new WaitForSeconds(1f);

        if (firstFlippedCard.cardValue == secondFlippedCard.cardValue)
        {
            matchesFound++;
            score += 10 * comboMultiplier;
            comboMultiplier++;

            Destroy(firstFlippedCard.gameObject);
            Destroy(secondFlippedCard.gameObject);

            // Check for game over
            if (matchesFound >= totalMatches)
            {
                gridLayout.enabled = true;
                GameOver();
            }
        }
        else
        {
            firstFlippedCard.FlipBack();
            secondFlippedCard.FlipBack();
            comboMultiplier = 1;

        }

        UpdateUI();
        firstFlippedCard = null;
        secondFlippedCard = null;
        isProcessing = false;

    }

    private void SetupScreenSize(int gridRows, int gridCols)
    {
        float boardWidth = boardRect.rect.width;
        float boardHeight = boardRect.rect.height;

        float spacingX = boardWidth * 0.02f;
        float spacingY = boardHeight * 0.02f;

        gridLayout.spacing = new Vector2(spacingX, spacingY);

        float availableWidth = boardWidth - (spacingX * (gridCols - 1));
        float availableHeight = boardHeight - (spacingY * (gridRows - 1));

        float cellWidth = availableWidth / gridCols;
        float cellHeight = availableHeight / gridRows;

        gridLayout.cellSize = new Vector2(cellWidth, cellHeight);

    }

    private void UpdateUI()
    {
        scoreText.text = "Score: " + score;
        comboText.text = "Combo x" + comboMultiplier;
    }

    private void GameOver()
    {
        // Play game over sound
        //AudioManager.Instance.PlaySound("GameOver");
        // Display game over UI or restart the game
        
        cardValues.Clear();
        InitializeGame();
    }

    /// <summary>
    /// Fisher-Yates
    /// </summary>
    /// <param name="list"></param>
    private void Shuffle(List<int> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int rand = Random.Range(0, i + 1);
            int temp = list[i];
            list[i] = list[rand];
            list[rand] = temp;
        }
    }

    
}
