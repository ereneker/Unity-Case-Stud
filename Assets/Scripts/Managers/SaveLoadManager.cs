using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoadManager
{
    private static string saveFilePath = Application.persistentDataPath + "/gamestate.dat";

    public static void SaveGameState(GameManager gameManager)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(saveFilePath);

        GameStateData data = new GameStateData();

        // Save score and combo
        data.score = gameManager.Score;
        data.comboMultiplier = gameManager.ComboMultiplier;

        // Save card states
        data.cardStates = new List<CardData>();
        foreach (Transform cardTransform in gameManager.gameBoard)
        {
            Card card = cardTransform.GetComponent<Card>();
            CardData cardData = new CardData();
            cardData.cardValue = card.cardValue;
            cardData.isFlipped = card.isFlipped;
            data.cardStates.Add(cardData);
        }

        bf.Serialize(file, data);
        file.Close();
    }

    public static void LoadGameState(GameManager gameManager)
    {
        if (File.Exists(saveFilePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(saveFilePath, FileMode.Open);
            GameStateData data = (GameStateData)bf.Deserialize(file);
            file.Close();

            // Load score and combo
            gameManager.Score = data.score;
            gameManager.ComboMultiplier = data.comboMultiplier;

            // Destroy existing cards
            foreach (Transform child in gameManager.gameBoard)
            {
                GameObject.Destroy(child.gameObject);
            }

            // Recreate cards based on saved data
            foreach (CardData cardData in data.cardStates)
            {
                GameObject cardObj = GameObject.Instantiate(gameManager.cardPrefab, gameManager.gameBoard);
                Card card = cardObj.GetComponent<Card>();
                card.cardValue = cardData.cardValue;

                if (cardData.isFlipped)
                {
                    card.FlipCard();
                }
            }
        }
    }
}

[System.Serializable]
public class GameStateData
{
    public int score;
    public int comboMultiplier;
    public List<CardData> cardStates;
}

[System.Serializable]
public class CardData
{
    public int cardValue;
    public bool isFlipped;
}
