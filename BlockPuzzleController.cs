using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPuzzleController : MonoBehaviour
{
    public Texture2D image;
    int columns = 3;
    float offset;
    public GameObject blockPuzzleCenter;
    Blocks emptyBlock;
    public int blockNumber;
    Queue<Blocks> inputs;
    bool blockIsMoving;
    Blocks[,] blocks;
    public int shuffleLength = 20;
    int shuffleMovesRemaining;
    Vector2Int previosShuffleOffset;
    public float defaultMoveDuration = .2f;
    public float shuffleMoveDuration = .1f;
    MasterBedroomPuzzleControl theRules;
    enum PuzzleState { Solved, Shuffling, InPlay, Resetting};
    PuzzleState state;

    // Start is called before the first frame update
    void Start()
    {
        theRules = FindObjectOfType<MasterBedroomPuzzleControl>();
        offset = columns;
        blockNumber = 1;
        CreateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateGrid()
    {
        blocks = new Blocks[columns, columns];
        Texture2D[,] imageSlices = ImageSlicer.GetSlices(image, columns);

        for (int y = 0; y < columns; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                GameObject blockObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
                blockObject.transform.position = (new Vector2(blockPuzzleCenter.transform.position.x, blockPuzzleCenter.transform.position.y) * ((offset - 1) * .5f) + new Vector2(x, y));
                blockObject.transform.parent = transform;
                Blocks block = blockObject.AddComponent<Blocks>();
                blockObject.GetComponent<Blocks>().blockNumber = blockNumber;
                block.OnBlockPressed += PlayerMoveBlockInput;
                block.OnFinishedMoving += OnBlockFinishedMoving;
                blockNumber++;
                block.Init(new Vector2Int(x, y), imageSlices[x, y]);
                blocks[x, y] = block;

                if (y == 0 && x == columns - 1)
                {
                    emptyBlock = block;
                }
            }
        }

        inputs = new Queue<Blocks>();
        //StartShuffle();
    }

    void PlayerMoveBlockInput(Blocks blockToMove) 
    {
        if (state == PuzzleState.InPlay)
        {
            inputs.Enqueue(blockToMove);
            NextMove();
        }
    }

    void MoveBlock(Blocks blockToMove, float duration) 
    {
        if (state != PuzzleState.Resetting && (blockToMove.coord - emptyBlock.coord).sqrMagnitude == 1)
        {
            blocks[blockToMove.coord.x, blockToMove.coord.y] = emptyBlock;
            blocks[emptyBlock.coord.x, emptyBlock.coord.y] = blockToMove;
            Vector2Int targetCoord = emptyBlock.coord;
            emptyBlock.coord = blockToMove.coord;
            blockToMove.coord = targetCoord;
            Vector2 targetPosition = emptyBlock.transform.position;
            emptyBlock.transform.position = blockToMove.transform.position;
            blockToMove.MoveToPosition(targetPosition, duration);
            blockIsMoving = true;
        }
        if (state == PuzzleState.Resetting) 
        {
            {
                blocks[blockToMove.coord.x, blockToMove.coord.y] = emptyBlock;
                blocks[emptyBlock.coord.x, emptyBlock.coord.y] = blockToMove;
                Vector2Int targetCoord = emptyBlock.coord;
                emptyBlock.coord = blockToMove.coord;
                blockToMove.coord = targetCoord;
                Vector2 targetPosition = emptyBlock.transform.position;
                emptyBlock.transform.position = blockToMove.transform.position;
                blockToMove.MoveToPosition(targetPosition, duration);
                blockIsMoving = true;
            }
        }
    }

    void OnBlockFinishedMoving() 
    {
        CheckIfSolved();
        blockIsMoving = false;
        if (state == PuzzleState.InPlay)
        {
            NextMove();
        }

        if (state == PuzzleState.Shuffling)
        {
            if (shuffleMovesRemaining > 0)
            {
                MakeNextShuffleMove();
            }
            else 
            {
                state = PuzzleState.InPlay;
                theRules.contestStarted = true;
            }
        }

        if (state == PuzzleState.Resetting) 
        {

        }
    }

    void NextMove() 
    {
        while (inputs.Count > 0 && !blockIsMoving)
        {
            MoveBlock(inputs.Dequeue(), defaultMoveDuration);
        }
    }

    void MakeNextShuffleMove() 
    {
        Vector2Int[] offsets = { new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1) };
        int randomIndex = Random.Range(0, offsets.Length);

        for (int i = 0; i < offsets.Length; i++)
        {
            Vector2Int offset = offsets[(randomIndex + i) % offsets.Length];
            if (offset != previosShuffleOffset * -1)
            {
                Vector2Int moveBlockCoord = emptyBlock.coord + offset;

                if (moveBlockCoord.x >= 0 && moveBlockCoord.x < columns && moveBlockCoord.y >= 0 && moveBlockCoord.y < columns)
                {
                    MoveBlock(blocks[moveBlockCoord.x, moveBlockCoord.y], shuffleMoveDuration);
                    shuffleMovesRemaining--;
                    previosShuffleOffset = offset;
                    break;
                }
            }
        }
    }

    public void StartShuffle() 
    {
        emptyBlock.gameObject.SetActive(false);
        state = PuzzleState.Shuffling;
        shuffleMovesRemaining = shuffleLength;
        MakeNextShuffleMove();
    }

    void CheckIfSolved() 
    {
        foreach(Blocks block in blocks) 
        {
            if (!block.IsAtStartingCoord()) 
            {
                return;
            }
        }

        state = PuzzleState.Solved;
        emptyBlock.gameObject.SetActive(true);
        theRules.GameWin();
    }

    public void ResetPuzzle()
    {
        state = PuzzleState.Shuffling;
        foreach (Blocks block in blocks)
        {
            if (!block.IsAtStartingCoord())
            {
                MoveBlock(blocks[block.startingCoord.x, block.startingCoord.y], .1f);
            }
        }

        state = PuzzleState.Solved;
        emptyBlock.gameObject.SetActive(true);
    }
}
