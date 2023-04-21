using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationModePuzzleEncryption : MonoBehaviour
{
    private string[] letterCharArray = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R",
                                                        "S", "T", "U", "V", "W", "X", "Y", "Z" };
    public string wordToEncrypt;
    public int encryptionValue;
    public List<string> wordList = new List<string>();
    private List<string> usedWordList = new List<string>();

    private char[] letterCharToEncryptArray;
    private List<int> letterIndexArrayToEncryptList = new List<int>();
    private List<int> letterIndexArrayEncryptedList = new List<int>();
    private List<string> letterEncryptedList = new List<string>();
    public string encryptedWord;
    public string encryptionPuzzleDescription;

    private ExplorationModeObjectInteractableWindowUi PuzzleWindow;

    private void Awake()
    {
        SetupComponent();
        PuzzleWindowNewQuestion();
    }
    private void SetupComponent()
    {
        PuzzleWindow = GetComponent<ExplorationModeObjectInteractableWindowUi>();
    }
    private void PuzzleWindowNewQuestion()
    {
        CreateEncryptedWord();
        PuzzleWindow.SetupWindow(encryptedWord, encryptionPuzzleDescription, wordToEncrypt);
    }
    private void PuzzleWindowFetch()
    {
        PuzzleWindow.SetupWindow(encryptedWord, encryptionPuzzleDescription, wordToEncrypt);
    }

    private void CreateEncryptedWord()
    {
        ClearWordAfterReset();
        RandomWordFormList();
        ConvertWordToLetterArray();
        ConvertLetterArrayToIndexArray();
        IncreaseLetterIndexArray();
        ConvertIndexArrayTostring();
        CreateStringFromStringList();
    }
    private void ClearWordAfterReset()
    {
        letterIndexArrayToEncryptList.Clear();
        letterIndexArrayEncryptedList.Clear();
        letterEncryptedList.Clear();
    }
    private bool isUsedWord;
    private void RandomWordFormList()
    {
        if (usedWordList.Count == 0)
        {
            wordToEncrypt = wordList[Random.Range(0, wordList.Count)];
            usedWordList.Add(wordToEncrypt);
        }
        else if (usedWordList.Count > 0)
        {
            do
            {
                isUsedWord = false;
                wordToEncrypt = wordList[Random.Range(0, wordList.Count)];
                for (int i = 0; i < usedWordList.Count ; i++)
                {
                    if (usedWordList[i] == wordToEncrypt)
                    {
                        isUsedWord = true;
                    }
                }
            } while (isUsedWord == true);
            
            usedWordList.Add(wordToEncrypt);
        }
    }
    private void ConvertWordToLetterArray()
    {
        wordToEncrypt = wordToEncrypt.ToUpper();
        letterCharToEncryptArray = wordToEncrypt.ToCharArray();
    }
    private void ConvertLetterArrayToIndexArray()
    {
        for (int letterChar = 0; letterChar < letterCharToEncryptArray.Length; letterChar++)
        {
            if(letterCharToEncryptArray[letterChar].ToString() == " ")
            {
                    letterIndexArrayToEncryptList.Add(-1);
            }
            else
            {
                for (int letterIndex = 0; letterIndex < letterCharArray.Length; letterIndex++)
                {
                    if (letterCharToEncryptArray[letterChar].ToString() == letterCharArray[letterIndex])
                    {
                        letterIndexArrayToEncryptList.Add(letterIndex);
                    }
                }
            }
        }
    }
    private void IncreaseLetterIndexArray()
    {
        for (int indexToIncrease = 0; indexToIncrease < letterIndexArrayToEncryptList.Count; indexToIncrease++)
        {
            if (letterIndexArrayToEncryptList[indexToIncrease] == -1)
            {
                letterIndexArrayEncryptedList.Add(-1);
            }
            else if (letterIndexArrayToEncryptList[indexToIncrease] + encryptionValue < letterCharArray.Length 
                && letterIndexArrayToEncryptList[indexToIncrease] + encryptionValue > -1)
            {
                letterIndexArrayEncryptedList.Add(letterIndexArrayToEncryptList[indexToIncrease] + encryptionValue);
            }
            else if (letterIndexArrayToEncryptList[indexToIncrease] + encryptionValue > letterCharArray.Length - 1)
            {
                letterIndexArrayEncryptedList.Add((letterIndexArrayToEncryptList[indexToIncrease] + encryptionValue) % letterCharArray.Length);
            }
            else if (letterIndexArrayToEncryptList[indexToIncrease] + encryptionValue < 0)
            {
                letterIndexArrayEncryptedList.Add(letterCharArray.Length + (letterIndexArrayToEncryptList[indexToIncrease] + encryptionValue));
            }
        }
    }
    private void ConvertIndexArrayTostring()
    {
        for (int indexToConvert = 0; indexToConvert < letterIndexArrayEncryptedList.Count; indexToConvert++)
        {
            if (letterIndexArrayEncryptedList[indexToConvert] == -1)
            {
                letterEncryptedList.Add(" ");
            }
            else
            {
                letterEncryptedList.Add(letterCharArray[letterIndexArrayEncryptedList[indexToConvert]]);
            }
        }
    }
    private void CreateStringFromStringList()
    {
        encryptedWord = "";
        foreach (string letter in letterEncryptedList)
        {
            encryptedWord += letter;
        }
    }

    private void FixedUpdate()
    {
        if (PuzzleWindow.isWindowGetNewQuestion == true)
        {
            PuzzleWindowNewQuestion();
            PuzzleWindow.isWindowGetNewQuestion = false;
        }
        else if (PuzzleWindow.isWindowFetch == true)
        {
            PuzzleWindowFetch();
            PuzzleWindow.isWindowFetch = false;
        }
    }
}
