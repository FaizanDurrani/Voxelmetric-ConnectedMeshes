using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockConnected : ProcMesh
{

    public string blockName;
    public Vector3 MeshSize;
    public Vector3 ConnMeshSizeX;
    public Vector3 ConnMeshSizeY;
    public Vector3 ConnMeshSizeZ;
    public DirectionsEnabled dirE;
    public TextureCollection[] textures;


    public BlockConnected(Vector3 MeshS, Vector3 conMX, Vector3 conMY, Vector3 conMZ, DirectionsEnabled dir)
    {
        MeshSize = MeshS;
        ConnMeshSizeX = conMX;
        ConnMeshSizeY = conMY;
        ConnMeshSizeZ = conMZ;

        dirE = dir;
    }

    public override void BuildFace(Chunk chunk, BlockPos pos, MeshData meshData, Direction direction, Block block)
    {
        List<Direction> dir = new List<Direction>();

        //East
        if (Block.Air != chunk.GetBlock(new BlockPos(pos.x + 1, pos.y, pos.z)) && dirE.east != false)
        {
            dir.Add(Direction.east);
        }
        //West
        if (Block.Air != chunk.GetBlock(new BlockPos(pos.x - 1, pos.y, pos.z)) && dirE.west != false)
        {
            dir.Add(Direction.west);
        }
        //North
        if (Block.Air != chunk.GetBlock(new BlockPos(pos.x, pos.y, pos.z + 1)) && dirE.north != false)
        {
            dir.Add(Direction.north);
        }
        //South
        if (Block.Air != chunk.GetBlock(new BlockPos(pos.x, pos.y, pos.z - 1)) && dirE.south != false)
        {
            dir.Add(Direction.south);
        }
        //Up
        if (Block.Air != chunk.GetBlock(new BlockPos(pos.x, pos.y + 1, pos.z)) && dirE.up != false)
        {
            dir.Add(Direction.up);
        }
        //Down
        if (Block.Air != chunk.GetBlock(new BlockPos(pos.x, pos.y - 1, pos.z)) && dirE.down != false)
        {
            dir.Add(Direction.down);
        }

        ConnectedBuilder.BuildRenderer(chunk, pos, meshData, direction, MeshSize, ConnMeshSizeX, ConnMeshSizeY, ConnMeshSizeZ, dir.ToArray());
        ConnectedBuilder.BuildTextures(chunk, pos, meshData, direction, textures, MeshSize, dir.ToArray());
        ConnectedBuilder.BuildColors(chunk, pos, meshData, direction, MeshSize, dir.ToArray());
        
        if (Config.Toggle.UseCollisionMesh)
        {
            BlockBuilder.BuildCollider(chunk, pos, meshData, direction);
        }
    }

    public override string Name()
    {
        return blockName;
    }

    public override bool IsSolid(Direction direction)
    {
        return isSolid;
    }

}

[System.Serializable]
public class DirectionsEnabled
{
    public bool up, down, north, south, east, west;
    public DirectionsEnabled(bool u, bool d, bool n, bool s, bool e, bool w)
    {
        up = u;
        down = d;
        north = n;
        south = s;
        east = e;
        west = w;
    }
}