using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World
{
    private Dictionary<Vector2, Chunk> chunkDict = new Dictionary<Vector2, Chunk>();

    //seed
    private void world()
    {

    }

    public Chunk GetChunk(Vector2 key)
    {
        return chunkDict[key];
    }

    public void SetChunk(Vector2 pos,Chunk newChunk)
    {
        chunkDict.Add(pos, newChunk);
    }

}
