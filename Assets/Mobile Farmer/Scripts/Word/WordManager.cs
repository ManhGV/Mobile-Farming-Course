using System;
using System.IO;
using System.Text;
using UnityEngine;

public class WordManager : MonoBehaviour
{
    [Header("Element")]
    [SerializeField] private Transform word;
    
    [Header("Data")]
    private WordData wordData;
    string dataPath;
    private bool shouldSave;

    private void Awake()
    {
        Chunk.onUnlocked += ChunkUnlockedCallback;
        Chunk.onPriceChanged += ChunkPriceChangeCallback;
    }

    private void OnDestroy()
    {
        Chunk.onUnlocked -= ChunkUnlockedCallback;
        Chunk.onPriceChanged -= ChunkPriceChangeCallback;
    }

    private void Start()
    {
        dataPath = Application.dataPath+"/WordData.txt";
        LoadWord();
        Initialize();
        
        InvokeRepeating("TrySaveGame", 1f, 1f);
    }

    private void Initialize()
    {
        for (int i = 0; i < word.childCount; i++)
        {
            word.GetChild(i).GetComponent<Chunk>().Initialize(wordData.chunkPrices[i]);
        }
    }
    
    private void TrySaveGame()
    {
        if(shouldSave)
        {
            SaveWord();
            shouldSave = false;
        }
    }

    private void ChunkUnlockedCallback()
    {
        SaveWord();
    }

    public void ChunkPriceChangeCallback()
    {
        shouldSave = true;
    }

    private void LoadWord()
    {
        string data = "";
        if (!File.Exists(dataPath))
        {
            FileStream fs = new FileStream(dataPath, FileMode.Create);
            wordData = new WordData();

            for (int i = 0; i < word.childCount; i++)
                wordData.chunkPrices.Add(word.GetChild(i).GetComponent<Chunk>().GetInitialPrice());

            string worldDataString = JsonUtility.ToJson(wordData, true);
            byte[] worldDataByte = Encoding.UTF8.GetBytes(worldDataString);
            
            fs.Write(worldDataByte);
            fs.Close();
        }
        else
        {
            data = File.ReadAllText(dataPath);
            wordData = JsonUtility.FromJson<WordData>(data);
            if (wordData.chunkPrices.Count < word.childCount)
                UpdateData();
        }
    }

    private void UpdateData()
    {
        int missingData = word.childCount - wordData.chunkPrices.Count;
        for (int i = 0; i < missingData; i++)
            wordData.chunkPrices.Add(word.GetChild(word.childCount - missingData + i).GetComponent<Chunk>().GetInitialPrice());
    }

    private void SaveWord()
    {
        if (wordData.chunkPrices.Count != word.childCount)
            wordData = new WordData();

        for (int i = 0; i < word.childCount; i++)
            if(wordData.chunkPrices.Count>i)
                wordData.chunkPrices[i] = word.GetChild(i).GetComponent<Chunk>().GetCurrentPrice();
            else
                wordData.chunkPrices.Add(word.GetChild(i).GetComponent<Chunk>().GetCurrentPrice());
        
        string data = JsonUtility.ToJson(wordData, true);
        
        File.WriteAllText(dataPath, data);
    }
}
