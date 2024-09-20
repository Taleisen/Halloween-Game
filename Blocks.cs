using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocks : MonoBehaviour
{
    public event System.Action<Blocks> OnBlockPressed; 
    public event System.Action OnFinishedMoving;
    public int blockNumber;
    public Vector2Int coord;
    public Vector2Int startingCoord;
    public bool reset;
    BlockPuzzleController theRules;

    private void Start()
    {
        theRules = FindObjectOfType<BlockPuzzleController>();
        reset = false;
    }

    private void Update()
    {
        if (reset)
        {
            transform.position = new Vector3Int(startingCoord.x, startingCoord.y, 0);
            reset = false;
        }
    }

    public void Init(Vector2Int startingCoord, Texture2D image)
    {
        this.startingCoord = startingCoord;
        coord = startingCoord;
        GetComponent<MeshRenderer>().material = Resources.Load<Material>("BlockMaterial");
        GetComponent<MeshRenderer>().material.mainTexture = image;
    }

    public void MoveToPosition(Vector2 target, float duration) 
    {
        StartCoroutine(AnimateMove(target, duration));
    }

    private void OnMouseDown()
    {
        if (OnBlockPressed != null) 
        {
            OnBlockPressed(this);
        }
    }

    IEnumerator AnimateMove(Vector2 target, float duration) 
    {
        Vector2 initialPos = transform.position;
        float percent = 0;

        while (percent < 1) 
        {
            percent += Time.deltaTime / duration;
            transform.position = Vector2.Lerp(initialPos, target, percent);
            yield return null;
        }

        if (OnFinishedMoving != null) 
        {
            OnFinishedMoving();
        }
    }

    public bool IsAtStartingCoord() 
    {
        return coord == startingCoord;
    }
}
