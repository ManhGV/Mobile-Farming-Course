using System;
using System.IO;
using System.Text;
using UnityEngine;

public class WordManager : MonoBehaviour
{
    enum ChunkShape
    {
        None,
        TopRight,
        BottomRight,
        BottomLeft,
        TopLeft,
        Top,
        Right,
        Bottom,
        Left,
        Four
    }
    [Header("Element")]
    [SerializeField] private Transform wordTF;
    private Chunk[,] grid;

    [Header("Settings")]
    [SerializeReference] private int gridSize;
    [SerializeReference] private int gridScale;
    
    [Header("Data")]
    private WordData wordData;
    string dataPath;
    private bool shouldSave;

    [Header("Chunk Meshes")]
    [SerializeField] private Mesh[] chunkShapes;

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
        for (int i = 0; i < wordTF.childCount; i++)
            wordTF.GetChild(i).GetComponent<Chunk>().Initialize(wordData.chunkPrices[i]);

        InitGrid();
        UpdateChunkWalls();
        UpdateGridRenderers();
    }

    private void InitGrid()
    {
        grid = new Chunk[gridSize,gridSize];

        for (int i = 0; i < wordTF.childCount; i++)
        {
            Chunk chunk = wordTF.GetChild(i).GetComponent<Chunk>();

            Vector2Int chunkGridPosition = new Vector2Int((int)chunk.transform.position.x / gridScale, (int)chunk.transform.position.z / gridScale);

            chunkGridPosition += new Vector2Int(gridSize / 2, gridSize / 2);
            grid[chunkGridPosition.x, chunkGridPosition.y] = chunk;
        }

        // for (int i = 0; i < gridSize; i++)
        //     for (int j = 0; j < gridSize; j++)
        //         if(grid[i, j] != null)
        //             Debug.Log(grid[i, j].name);
    }

    private void UpdateChunkWalls()
    {
        for (int x = 0; x < grid.GetLength(0); x++)
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                Chunk chunk = grid[x, y];
                
                if(chunk != null)
                {
                    Chunk fornt = GetGrid(x, y + 1);
                    Chunk right = GetGrid(x + 1, y);
                    Chunk back = GetGrid(x, y - 1);
                    Chunk left = GetGrid(x - 1, y);

                    //Sử dụng ố ệ nhị phân 1 = 0001; 3 = 0011; 7 = 0111; 15 = 1111;
                    int configuration = 0;
                    if(fornt && fornt.IsUnlocked())
                        configuration = configuration + 1;
                    if(right&& right.IsUnlocked())
                        configuration = configuration + 2;
                    if(back&& back.IsUnlocked())
                        configuration = configuration + 4;
                    if(left&& left.IsUnlocked())
                        configuration = configuration + 8;
                    
                    chunk.UpdateWall(configuration);
                    SetChunkRenderer(chunk, configuration);
                }
            }
    }

    private void SetChunkRenderer(Chunk chunk, int configuration)
    {
        switch (configuration)
        {
            case 0:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.Four]); 
                break;
            case 1:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.Bottom]); 
                break;
            case 2:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.Left]); 
                break;
            case 3:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.BottomLeft]); 
                break;
            case 4:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.Top]); 
                break;
            case 5:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.None]); 
                break;
            case 6:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.TopLeft]); 
                break;
            case 7:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.None]); 
                break;
            case 8:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.Right]); 
                break;
            case 9:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.BottomRight]); 
                break;
            case 10:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.None]); 
                break;
            case 11:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.None]); 
                break;
            case 12:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.TopRight]); 
                break;
            case 13:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.None]); 
                break;
            case 14:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.None]); 
                break;
            case 15:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.None]); 
                break;
        }
    }

    /// <summary>
    /// nhìn thấy các ô bên cạnh ô mở khóa
    /// </summary>
    private void UpdateGridRenderers()
    {
        for (int x = 0; x < grid.GetLength(0); x++)
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                Chunk chunk = grid[x, y];

                if (chunk == null)
                    continue;
                if(chunk.IsUnlocked())
                    continue;
                Chunk fornt = GetGrid(x, y + 1);
                Chunk right = GetGrid(x + 1, y);
                Chunk back = GetGrid(x, y - 1);
                Chunk left = GetGrid(x - 1, y);

                if (fornt && fornt.IsUnlocked())
                    chunk.DisplayLockedElements();    
                else if(right&& right.IsUnlocked())
                    chunk.DisplayLockedElements();    
                else if(back&& back.IsUnlocked())
                    chunk.DisplayLockedElements();    
                else if(left&& left.IsUnlocked())
                    chunk.DisplayLockedElements();    
            }
    }
    
    private Chunk GetGrid(int x, int y)
    {
        if (IsValiGridPosition(x, y))
            return grid[x, y];
        
        return null;
    }
    
    private bool IsValiGridPosition(int x, int y)
    {
        if (x < 0 || x >= gridSize || y < 0 || y >= gridSize)
            return false;
        return true;
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
        UpdateChunkWalls();
        SaveWord();
        UpdateGridRenderers();
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

            for (int i = 0; i < wordTF.childCount; i++)
                wordData.chunkPrices.Add(wordTF.GetChild(i).GetComponent<Chunk>().GetInitialPrice());

            string worldDataString = JsonUtility.ToJson(wordData, true);
            byte[] worldDataByte = Encoding.UTF8.GetBytes(worldDataString);
            
            fs.Write(worldDataByte);
            fs.Close();
        }
        else
        {
            data = File.ReadAllText(dataPath);
            wordData = JsonUtility.FromJson<WordData>(data);
            if (wordData.chunkPrices.Count < wordTF.childCount)
                UpdateData();
        }
    }

    private void UpdateData()
    {
        int missingData = wordTF.childCount - wordData.chunkPrices.Count;
        for (int i = 0; i < missingData; i++)
            wordData.chunkPrices.Add(wordTF.GetChild(wordTF.childCount - missingData + i).GetComponent<Chunk>().GetInitialPrice());
    }

    private void SaveWord()
    {
        if (wordData.chunkPrices.Count != wordTF.childCount)
            wordData = new WordData();

        for (int i = 0; i < wordTF.childCount; i++)
            if(wordData.chunkPrices.Count>i)
                wordData.chunkPrices[i] = wordTF.GetChild(i).GetComponent<Chunk>().GetCurrentPrice();
            else
                wordData.chunkPrices.Add(wordTF.GetChild(i).GetComponent<Chunk>().GetCurrentPrice());
        
        string data = JsonUtility.ToJson(wordData, true);
        
        File.WriteAllText(dataPath, data);
    }
}
