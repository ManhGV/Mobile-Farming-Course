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
    private ChunkGround[,] grid;

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
        ChunkGround.onUnlocked += ChunkUnlockedCallback;
        ChunkGround.onPriceChanged += ChunkPriceChangeCallback;
    }

    private void OnDestroy()
    {
        ChunkGround.onUnlocked -= ChunkUnlockedCallback;
        ChunkGround.onPriceChanged -= ChunkPriceChangeCallback;
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
            wordTF.GetChild(i).GetComponent<ChunkGround>().Initialize(wordData.chunkPrices[i]);

        InitGrid();
        UpdateChunkWalls();
        UpdateGridRenderers();
    }

    private void InitGrid()
    {
        grid = new ChunkGround[gridSize,gridSize];

        for (int i = 0; i < wordTF.childCount; i++)
        {
            ChunkGround chunkGround = wordTF.GetChild(i).GetComponent<ChunkGround>();

            Vector2Int chunkGridPosition = new Vector2Int((int)chunkGround.transform.position.x / gridScale, (int)chunkGround.transform.position.z / gridScale);

            chunkGridPosition += new Vector2Int(gridSize / 2, gridSize / 2);
            grid[chunkGridPosition.x, chunkGridPosition.y] = chunkGround;
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
                ChunkGround chunkGround = grid[x, y];
                
                if(chunkGround != null)
                {
                    ChunkGround fornt = GetGrid(x, y + 1);
                    ChunkGround right = GetGrid(x + 1, y);
                    ChunkGround back = GetGrid(x, y - 1);
                    ChunkGround left = GetGrid(x - 1, y);

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
                    
                    chunkGround.UpdateWall(configuration);
                    SetChunkRenderer(chunkGround, configuration);
                }
            }
    }

    private void SetChunkRenderer(ChunkGround chunkGround, int configuration)
    {
        switch (configuration)
        {
            case 0:
                chunkGround.SetRenderer(chunkShapes[(int)ChunkShape.Four]); 
                break;
            case 1:
                chunkGround.SetRenderer(chunkShapes[(int)ChunkShape.Bottom]); 
                break;
            case 2:
                chunkGround.SetRenderer(chunkShapes[(int)ChunkShape.Left]); 
                break;
            case 3:
                chunkGround.SetRenderer(chunkShapes[(int)ChunkShape.BottomLeft]); 
                break;
            case 4:
                chunkGround.SetRenderer(chunkShapes[(int)ChunkShape.Top]); 
                break;
            case 5:
                chunkGround.SetRenderer(chunkShapes[(int)ChunkShape.None]); 
                break;
            case 6:
                chunkGround.SetRenderer(chunkShapes[(int)ChunkShape.TopLeft]); 
                break;
            case 7:
                chunkGround.SetRenderer(chunkShapes[(int)ChunkShape.None]); 
                break;
            case 8:
                chunkGround.SetRenderer(chunkShapes[(int)ChunkShape.Right]); 
                break;
            case 9:
                chunkGround.SetRenderer(chunkShapes[(int)ChunkShape.BottomRight]); 
                break;
            case 10:
                chunkGround.SetRenderer(chunkShapes[(int)ChunkShape.None]); 
                break;
            case 11:
                chunkGround.SetRenderer(chunkShapes[(int)ChunkShape.None]); 
                break;
            case 12:
                chunkGround.SetRenderer(chunkShapes[(int)ChunkShape.TopRight]); 
                break;
            case 13:
                chunkGround.SetRenderer(chunkShapes[(int)ChunkShape.None]); 
                break;
            case 14:
                chunkGround.SetRenderer(chunkShapes[(int)ChunkShape.None]); 
                break;
            case 15:
                chunkGround.SetRenderer(chunkShapes[(int)ChunkShape.None]); 
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
                ChunkGround chunkGround = grid[x, y];

                if (chunkGround == null)
                    continue;
                if(chunkGround.IsUnlocked())
                    continue;
                ChunkGround fornt = GetGrid(x, y + 1);
                ChunkGround right = GetGrid(x + 1, y);
                ChunkGround back = GetGrid(x, y - 1);
                ChunkGround left = GetGrid(x - 1, y);

                if (fornt && fornt.IsUnlocked())
                    chunkGround.DisplayLockedElements();    
                else if(right&& right.IsUnlocked())
                    chunkGround.DisplayLockedElements();    
                else if(back&& back.IsUnlocked())
                    chunkGround.DisplayLockedElements();    
                else if(left&& left.IsUnlocked())
                    chunkGround.DisplayLockedElements();    
            }
    }
    
    private ChunkGround GetGrid(int x, int y)
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
                wordData.chunkPrices.Add(wordTF.GetChild(i).GetComponent<ChunkGround>().GetInitialPrice());

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
            wordData.chunkPrices.Add(wordTF.GetChild(wordTF.childCount - missingData + i).GetComponent<ChunkGround>().GetInitialPrice());
    }

    private void SaveWord()
    {
        if (wordData.chunkPrices.Count != wordTF.childCount)
            wordData = new WordData();

        for (int i = 0; i < wordTF.childCount; i++)
            if(wordData.chunkPrices.Count>i)
                wordData.chunkPrices[i] = wordTF.GetChild(i).GetComponent<ChunkGround>().GetCurrentPrice();
            else
                wordData.chunkPrices.Add(wordTF.GetChild(i).GetComponent<ChunkGround>().GetCurrentPrice());
        
        string data = JsonUtility.ToJson(wordData, true);
        
        File.WriteAllText(dataPath, data);
    }
}
