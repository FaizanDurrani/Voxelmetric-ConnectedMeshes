using UnityEngine;
using System;

[Serializable]
public class ConnectedBuilder
{
    public static void BuildRenderer(Chunk chunk, BlockPos pos, MeshData meshData, Direction direction, Vector3 ModelSize, Vector3 ConnMeshSizeX, Vector3 ConnMeshSizeY, Vector3 ConnMeshSizeZ, Direction[] Dir)
    {
        MakeStickFace(chunk, pos, meshData, direction, false, ModelSize);
        Debug.Log(Dir.Length);
        if (Dir.Length > 0)
            MakeFenceFace(chunk, pos, meshData, direction, false, ModelSize, ConnMeshSizeX, ConnMeshSizeY, ConnMeshSizeZ, Dir);
    }

    public static void BuildTextures(Chunk chunk, BlockPos pos, MeshData meshData, Direction direction, TextureCollection[] textureCollections, Vector3 MeshSize, Direction[] Dir)
    {
        MakeTexture(chunk, pos, meshData, direction, textureCollections);
        if (Dir.Length > 0)
            MakeFenceTexture(chunk, pos, meshData, direction, textureCollections,MeshSize, Dir);
    }

    public static void BuildColors(Chunk chunk, BlockPos pos, MeshData meshData, Direction direction, Vector3 MeshSize, Direction[] Dir)
    {
        MakeStickColors(chunk, pos, meshData, direction);
        if(Dir.Length > 0)
            MakeFenceColors(chunk, pos, meshData, direction,MeshSize, Dir);
    }

    public static void BuildCollider(Chunk chunk, BlockPos pos, MeshData meshData, Direction direction, Vector3 MeshSize)
    {
        MakeStickFace(chunk, pos, meshData, direction, true, MeshSize);
    }

    static void MakeTexture(Chunk chunk, BlockPos pos, MeshData meshData, Direction direction, TextureCollection[] textureCollections)
    {
        Rect texture = new Rect();

        switch (direction)
        {
            case Direction.up:
                texture = textureCollections[0].GetTexture(chunk, pos, direction);
                break;
            case Direction.down:
                texture = textureCollections[1].GetTexture(chunk, pos, direction);
                break;
            case Direction.north:
                texture = textureCollections[2].GetTexture(chunk, pos, direction);
                break;
            case Direction.east:
                texture = textureCollections[3].GetTexture(chunk, pos, direction);
                break;
            case Direction.south:
                texture = textureCollections[4].GetTexture(chunk, pos, direction);
                break;
            case Direction.west:
                texture = textureCollections[5].GetTexture(chunk, pos, direction);
                break;
            default:
                break;
        }

        Vector2[] UVs = new Vector2[4];

        UVs[0] = new Vector2(texture.x + texture.width, texture.y);
        UVs[1] = new Vector2(texture.x + texture.width, texture.y + texture.height);
        UVs[2] = new Vector2(texture.x, texture.y + texture.height);
        UVs[3] = new Vector2(texture.x, texture.y);

        meshData.uv.AddRange(UVs);
    }

    static void MakeFenceTexture(Chunk chunk, BlockPos pos, MeshData meshData, Direction direction, TextureCollection[] textureCollections, Vector3 MeshSize, Direction[] Dir)
    {
        Rect texture = new Rect();
        //Converting the position to a vector adjusts it based on block size and gives us real world coordinates for x, y and z
        foreach (Direction localDir in Dir)
        {
            Vector3 vPos = new Vector3();
            if (localDir == Direction.north)
            {
                vPos = new Vector3(pos.x, pos.y, pos.z + MeshSize.z);
            }
            else if (localDir == Direction.east)
            {
                vPos = new Vector3(pos.x + MeshSize.x, pos.y, pos.z);
            }
            else if (localDir == Direction.west)
            {
                vPos = new Vector3(pos.x - MeshSize.x, pos.y, pos.z);
            }
            else if (localDir == Direction.south)
            {
                vPos = new Vector3(pos.x, pos.y, pos.z - MeshSize.z);
            }
            else if (localDir == Direction.up)
            {
                vPos = new Vector3(pos.x, pos.y + MeshSize.y, pos.z);
            }
            else if (localDir == Direction.down)
            {
                vPos = new Vector3(pos.x, pos.y - MeshSize.y, pos.z);
            }
            switch (direction)
            {
                case Direction.up:
                    texture = textureCollections[0].GetTexture(chunk, vPos, direction);
                    break;
                case Direction.down:
                    texture = textureCollections[1].GetTexture(chunk, vPos, direction);
                    break;
                case Direction.north:
                    texture = textureCollections[2].GetTexture(chunk, vPos, direction);
                    break;
                case Direction.east:
                    texture = textureCollections[3].GetTexture(chunk, vPos, direction);
                    break;
                case Direction.south:
                    texture = textureCollections[4].GetTexture(chunk, vPos, direction);
                    break;
                case Direction.west:
                    texture = textureCollections[5].GetTexture(chunk, vPos, direction);
                    break;
                default:
                    break;
            }

            Vector2[] UVs = new Vector2[4];

            UVs[0] = new Vector2(texture.x + texture.width, texture.y);
            UVs[1] = new Vector2(texture.x + texture.width, texture.y + texture.height);
            UVs[2] = new Vector2(texture.x, texture.y + texture.height);
            UVs[3] = new Vector2(texture.x, texture.y);

            meshData.uv.AddRange(UVs);
        }
    }

    static void MakeStickFace(Chunk chunk, BlockPos pos, MeshData meshData, Direction direction, bool useCollisionMesh, Vector3 ModelSize)
    {
        //Adding a tiny overlap between block meshes may solve floating point imprecision
        //errors causing pixel size gaps between blocks when looking closely
        float halfBlockX = (ModelSize.x / 2) + Config.Env.BlockFacePadding;
        float halfBlockY = (ModelSize.y / 2) + Config.Env.BlockFacePadding;
        float halfBlockZ = (ModelSize.z / 2) + Config.Env.BlockFacePadding;

        //Converting the position to a vector adjusts it based on block size and gives us real world coordinates for x, y and z
        Vector3 vPos = new Vector3(pos.x, pos.y, pos.z);

        switch (direction)
        {
            case Direction.up:
                meshData.AddVertex(new Vector3(vPos.x - halfBlockX, vPos.y + halfBlockY, vPos.z + halfBlockZ), useCollisionMesh);
                meshData.AddVertex(new Vector3(vPos.x + halfBlockX, vPos.y + halfBlockY, vPos.z + halfBlockZ), useCollisionMesh);
                meshData.AddVertex(new Vector3(vPos.x + halfBlockX, vPos.y + halfBlockY, vPos.z - halfBlockZ), useCollisionMesh);
                meshData.AddVertex(new Vector3(vPos.x - halfBlockX, vPos.y + halfBlockY, vPos.z - halfBlockZ), useCollisionMesh);
                break;
            case Direction.down:
                meshData.AddVertex(new Vector3(vPos.x - halfBlockX, vPos.y - halfBlockY, vPos.z - halfBlockZ), useCollisionMesh);
                meshData.AddVertex(new Vector3(vPos.x + halfBlockX, vPos.y - halfBlockY, vPos.z - halfBlockZ), useCollisionMesh);
                meshData.AddVertex(new Vector3(vPos.x + halfBlockX, vPos.y - halfBlockY, vPos.z + halfBlockZ), useCollisionMesh);
                meshData.AddVertex(new Vector3(vPos.x - halfBlockX, vPos.y - halfBlockY, vPos.z + halfBlockZ), useCollisionMesh);
                break;
            case Direction.north:
                meshData.AddVertex(new Vector3(vPos.x + halfBlockX, vPos.y - halfBlockY, vPos.z + halfBlockZ), useCollisionMesh);
                meshData.AddVertex(new Vector3(vPos.x + halfBlockX, vPos.y + halfBlockY, vPos.z + halfBlockZ), useCollisionMesh);
                meshData.AddVertex(new Vector3(vPos.x - halfBlockX, vPos.y + halfBlockY, vPos.z + halfBlockZ), useCollisionMesh);
                meshData.AddVertex(new Vector3(vPos.x - halfBlockX, vPos.y - halfBlockY, vPos.z + halfBlockZ), useCollisionMesh);
                break;
            case Direction.east:
                meshData.AddVertex(new Vector3(vPos.x + halfBlockX, vPos.y - halfBlockY, vPos.z - halfBlockZ), useCollisionMesh);
                meshData.AddVertex(new Vector3(vPos.x + halfBlockX, vPos.y + halfBlockY, vPos.z - halfBlockZ), useCollisionMesh);
                meshData.AddVertex(new Vector3(vPos.x + halfBlockX, vPos.y + halfBlockY, vPos.z + halfBlockZ), useCollisionMesh);
                meshData.AddVertex(new Vector3(vPos.x + halfBlockX, vPos.y - halfBlockY, vPos.z + halfBlockZ), useCollisionMesh);
                break;
            case Direction.south:
                meshData.AddVertex(new Vector3(vPos.x - halfBlockX, vPos.y - halfBlockY, vPos.z - halfBlockZ), useCollisionMesh);
                meshData.AddVertex(new Vector3(vPos.x - halfBlockX, vPos.y + halfBlockY, vPos.z - halfBlockZ), useCollisionMesh);
                meshData.AddVertex(new Vector3(vPos.x + halfBlockX, vPos.y + halfBlockY, vPos.z - halfBlockZ), useCollisionMesh);
                meshData.AddVertex(new Vector3(vPos.x + halfBlockX, vPos.y - halfBlockY, vPos.z - halfBlockZ), useCollisionMesh);
                break;
            case Direction.west:
                meshData.AddVertex(new Vector3(vPos.x - halfBlockX, vPos.y - halfBlockY, vPos.z + halfBlockZ), useCollisionMesh);
                meshData.AddVertex(new Vector3(vPos.x - halfBlockX, vPos.y + halfBlockY, vPos.z + halfBlockZ), useCollisionMesh);
                meshData.AddVertex(new Vector3(vPos.x - halfBlockX, vPos.y + halfBlockY, vPos.z - halfBlockZ), useCollisionMesh);
                meshData.AddVertex(new Vector3(vPos.x - halfBlockX, vPos.y - halfBlockY, vPos.z - halfBlockZ), useCollisionMesh);
                break;
            default:
                Debug.LogError("Direction not recognized");
                break;
        }

        meshData.AddQuadTriangles(useCollisionMesh);
    }

    static void MakeFenceFace(Chunk chunk, BlockPos pos, MeshData meshData, Direction direction, bool useCollisionMesh, Vector3 ModelSize, Vector3 ConnMeshSizeX, Vector3 ConnMeshSizeY, Vector3 ConnMeshSizeZ, Direction[] Dir)
    {
        //Adding a tiny overlap between block meshes may solve floating point imprecision
        //errors causing pixel size gaps between blocks when looking closely
        float halfBlockX = 0;
        float halfBlockY = 0;
        float halfBlockZ = 0;

        //Converting the position to a vector adjusts it based on block size and gives us real world coordinates for x, y and z
        foreach (Direction localDir in Dir)
        {
            Vector3 vPos = new Vector3();
            if (localDir == Direction.north)
            {
                vPos = new Vector3(pos.x, pos.y, pos.z + ModelSize.z);
                halfBlockX = (ConnMeshSizeZ.x / 2) + Config.Env.BlockFacePadding;
                halfBlockY = (ConnMeshSizeZ.y / 2) + Config.Env.BlockFacePadding;
                halfBlockZ = (ConnMeshSizeZ.z / 2) + Config.Env.BlockFacePadding;
            }
            else if (localDir == Direction.south)
            {
                vPos = new Vector3(pos.x, pos.y, pos.z - ModelSize.z);
                halfBlockX = (ConnMeshSizeZ.x / 2) + Config.Env.BlockFacePadding;
                halfBlockY = (ConnMeshSizeZ.y / 2) + Config.Env.BlockFacePadding;
                halfBlockZ = (ConnMeshSizeZ.z / 2) + Config.Env.BlockFacePadding;
            }
            else if (localDir == Direction.east)
            {
                vPos = new Vector3(pos.x + ModelSize.x, pos.y, pos.z);
                halfBlockX = (ConnMeshSizeX.x / 2) + Config.Env.BlockFacePadding;
                halfBlockY = (ConnMeshSizeX.y / 2) + Config.Env.BlockFacePadding;
                halfBlockZ = (ConnMeshSizeX.z / 2) + Config.Env.BlockFacePadding;
            }
            else if (localDir == Direction.west)
            {
                vPos = new Vector3(pos.x - ModelSize.x, pos.y, pos.z);
                halfBlockX = (ConnMeshSizeX.x / 2) + Config.Env.BlockFacePadding;
                halfBlockY = (ConnMeshSizeX.y / 2) + Config.Env.BlockFacePadding;
                halfBlockZ = (ConnMeshSizeX.z / 2) + Config.Env.BlockFacePadding;
            }
            else if (localDir == Direction.up)
            {
                vPos = new Vector3(pos.x, pos.y + ModelSize.y, pos.z);
                halfBlockX = (ConnMeshSizeY.x / 2) + Config.Env.BlockFacePadding;
                halfBlockY = (ConnMeshSizeY.y / 2) + Config.Env.BlockFacePadding;
                halfBlockZ = (ConnMeshSizeY.z / 2) + Config.Env.BlockFacePadding;
            }
            else if (localDir == Direction.down)
            {
                vPos = new Vector3(pos.x, pos.y - ModelSize.y, pos.z);
                halfBlockX = (ConnMeshSizeY.x / 2) + Config.Env.BlockFacePadding;
                halfBlockY = (ConnMeshSizeY.y / 2) + Config.Env.BlockFacePadding;
                halfBlockZ = (ConnMeshSizeY.z / 2) + Config.Env.BlockFacePadding;
            }
            switch (direction)
            {
                case Direction.up:
                    meshData.AddVertex(new Vector3(vPos.x - halfBlockX, vPos.y + halfBlockY, vPos.z + halfBlockZ), useCollisionMesh);
                    meshData.AddVertex(new Vector3(vPos.x + halfBlockX, vPos.y + halfBlockY, vPos.z + halfBlockZ), useCollisionMesh);
                    meshData.AddVertex(new Vector3(vPos.x + halfBlockX, vPos.y + halfBlockY, vPos.z - halfBlockZ), useCollisionMesh);
                    meshData.AddVertex(new Vector3(vPos.x - halfBlockX, vPos.y + halfBlockY, vPos.z - halfBlockZ), useCollisionMesh);
                    break;
                case Direction.down:
                    meshData.AddVertex(new Vector3(vPos.x - halfBlockX, vPos.y - halfBlockY, vPos.z - halfBlockZ), useCollisionMesh);
                    meshData.AddVertex(new Vector3(vPos.x + halfBlockX, vPos.y - halfBlockY, vPos.z - halfBlockZ), useCollisionMesh);
                    meshData.AddVertex(new Vector3(vPos.x + halfBlockX, vPos.y - halfBlockY, vPos.z + halfBlockZ), useCollisionMesh);
                    meshData.AddVertex(new Vector3(vPos.x - halfBlockX, vPos.y - halfBlockY, vPos.z + halfBlockZ), useCollisionMesh);
                    break;
                case Direction.north:
                    meshData.AddVertex(new Vector3(vPos.x + halfBlockX, vPos.y - halfBlockY, vPos.z + halfBlockZ), useCollisionMesh);
                    meshData.AddVertex(new Vector3(vPos.x + halfBlockX, vPos.y + halfBlockY, vPos.z + halfBlockZ), useCollisionMesh);
                    meshData.AddVertex(new Vector3(vPos.x - halfBlockX, vPos.y + halfBlockY, vPos.z + halfBlockZ), useCollisionMesh);
                    meshData.AddVertex(new Vector3(vPos.x - halfBlockX, vPos.y - halfBlockY, vPos.z + halfBlockZ), useCollisionMesh);
                    break;
                case Direction.east:
                    meshData.AddVertex(new Vector3(vPos.x + halfBlockX, vPos.y - halfBlockY, vPos.z - halfBlockZ), useCollisionMesh);
                    meshData.AddVertex(new Vector3(vPos.x + halfBlockX, vPos.y + halfBlockY, vPos.z - halfBlockZ), useCollisionMesh);
                    meshData.AddVertex(new Vector3(vPos.x + halfBlockX, vPos.y + halfBlockY, vPos.z + halfBlockZ), useCollisionMesh);
                    meshData.AddVertex(new Vector3(vPos.x + halfBlockX, vPos.y - halfBlockY, vPos.z + halfBlockZ), useCollisionMesh);
                    break;
                case Direction.south:
                    meshData.AddVertex(new Vector3(vPos.x - halfBlockX, vPos.y - halfBlockY, vPos.z - halfBlockZ), useCollisionMesh);
                    meshData.AddVertex(new Vector3(vPos.x - halfBlockX, vPos.y + halfBlockY, vPos.z - halfBlockZ), useCollisionMesh);
                    meshData.AddVertex(new Vector3(vPos.x + halfBlockX, vPos.y + halfBlockY, vPos.z - halfBlockZ), useCollisionMesh);
                    meshData.AddVertex(new Vector3(vPos.x + halfBlockX, vPos.y - halfBlockY, vPos.z - halfBlockZ), useCollisionMesh);
                    break;
                case Direction.west:
                    meshData.AddVertex(new Vector3(vPos.x - halfBlockX, vPos.y - halfBlockY, vPos.z + halfBlockZ), useCollisionMesh);
                    meshData.AddVertex(new Vector3(vPos.x - halfBlockX, vPos.y + halfBlockY, vPos.z + halfBlockZ), useCollisionMesh);
                    meshData.AddVertex(new Vector3(vPos.x - halfBlockX, vPos.y + halfBlockY, vPos.z - halfBlockZ), useCollisionMesh);
                    meshData.AddVertex(new Vector3(vPos.x - halfBlockX, vPos.y - halfBlockY, vPos.z - halfBlockZ), useCollisionMesh);
                    break;
                default:
                    Debug.LogError("Direction not recognized");
                    break;

            }

            meshData.AddQuadTriangles(useCollisionMesh);
        }
    }

    static void MakeFenceColors(Chunk chunk, BlockPos pos, MeshData meshData, Direction direction,Vector3 MeshSize, Direction[] Dir)
    {
        bool nSolid = false;
        bool eSolid = false;
        bool sSolid = false;
        bool wSolid = false;

        bool wnSolid = false;
        bool neSolid = false;
        bool esSolid = false;
        bool swSolid = false;

        float light = 0;

        foreach (Direction localDir in Dir)
        {
            Vector3 vPos = new Vector3();
            if (localDir == Direction.north)
            {
                vPos = new Vector3(pos.x, pos.y, pos.z + MeshSize.z);
            }
            else if (localDir == Direction.east)
            {
                vPos = new Vector3(pos.x + MeshSize.x, pos.y, pos.z);
            }
            else if (localDir == Direction.west)
            {
                vPos = new Vector3(pos.x - MeshSize.x, pos.y, pos.z);
            }
            else if (localDir == Direction.south)
            {
                vPos = new Vector3(pos.x, pos.y, pos.z - MeshSize.z);
            }
            else if (localDir == Direction.up)
            {
                vPos = new Vector3(pos.x, pos.y + MeshSize.y, pos.z);
            }
            else if (localDir == Direction.down)
            {
                vPos = new Vector3(pos.x, pos.y - MeshSize.y, pos.z);
            }

            pos = vPos;
            switch (direction)
            {
                case Direction.up:
                    nSolid = chunk.GetBlock(pos.Add(0, 1, 1)).controller.IsSolid(Direction.south);
                    eSolid = chunk.GetBlock(pos.Add(1, 1, 0)).controller.IsSolid(Direction.west);
                    sSolid = chunk.GetBlock(pos.Add(0, 1, -1)).controller.IsSolid(Direction.north);
                    wSolid = chunk.GetBlock(pos.Add(-1, 1, 0)).controller.IsSolid(Direction.east);

                    wnSolid = chunk.GetBlock(pos.Add(-1, 1, 1)).controller.IsSolid(Direction.east) && chunk.GetBlock(pos.Add(-1, 1, 1)).controller.IsSolid(Direction.south);
                    neSolid = chunk.GetBlock(pos.Add(1, 1, 1)).controller.IsSolid(Direction.south) && chunk.GetBlock(pos.Add(1, 1, 1)).controller.IsSolid(Direction.west);
                    esSolid = chunk.GetBlock(pos.Add(1, 1, -1)).controller.IsSolid(Direction.west) && chunk.GetBlock(pos.Add(1, 1, -1)).controller.IsSolid(Direction.north);
                    swSolid = chunk.GetBlock(pos.Add(-1, 1, -1)).controller.IsSolid(Direction.north) && chunk.GetBlock(pos.Add(-1, 1, -1)).controller.IsSolid(Direction.east);

                    light = chunk.GetBlock(pos.Add(0, 1, 0)).data1 / 255f;

                    break;
                case Direction.down:
                    nSolid = chunk.GetBlock(pos.Add(0, -1, -1)).controller.IsSolid(Direction.south);
                    eSolid = chunk.GetBlock(pos.Add(1, -1, 0)).controller.IsSolid(Direction.west);
                    sSolid = chunk.GetBlock(pos.Add(0, -1, 1)).controller.IsSolid(Direction.north);
                    wSolid = chunk.GetBlock(pos.Add(-1, -1, 0)).controller.IsSolid(Direction.east);

                    wnSolid = chunk.GetBlock(pos.Add(-1, -1, -1)).controller.IsSolid(Direction.east) && chunk.GetBlock(pos.Add(-1, -1, -1)).controller.IsSolid(Direction.south);
                    neSolid = chunk.GetBlock(pos.Add(1, -1, -1)).controller.IsSolid(Direction.south) && chunk.GetBlock(pos.Add(1, -1, -1)).controller.IsSolid(Direction.west);
                    esSolid = chunk.GetBlock(pos.Add(1, -1, 1)).controller.IsSolid(Direction.west) && chunk.GetBlock(pos.Add(1, -1, 1)).controller.IsSolid(Direction.north);
                    swSolid = chunk.GetBlock(pos.Add(-1, -1, 1)).controller.IsSolid(Direction.north) && chunk.GetBlock(pos.Add(-1, -1, 1)).controller.IsSolid(Direction.east);

                    light = chunk.GetBlock(pos.Add(0, -1, 0)).data1 / 255f;

                    break;
                case Direction.north:
                    nSolid = chunk.GetBlock(pos.Add(1, 0, 1)).controller.IsSolid(Direction.west);
                    eSolid = chunk.GetBlock(pos.Add(0, 1, 1)).controller.IsSolid(Direction.down);
                    sSolid = chunk.GetBlock(pos.Add(-1, 0, 1)).controller.IsSolid(Direction.east);
                    wSolid = chunk.GetBlock(pos.Add(0, -1, 1)).controller.IsSolid(Direction.up);

                    esSolid = chunk.GetBlock(pos.Add(-1, 1, 1)).controller.IsSolid(Direction.east) && chunk.GetBlock(pos.Add(-1, 1, 1)).controller.IsSolid(Direction.south);
                    neSolid = chunk.GetBlock(pos.Add(1, 1, 1)).controller.IsSolid(Direction.south) && chunk.GetBlock(pos.Add(1, 1, 1)).controller.IsSolid(Direction.west);
                    wnSolid = chunk.GetBlock(pos.Add(1, -1, 1)).controller.IsSolid(Direction.west) && chunk.GetBlock(pos.Add(1, -1, 1)).controller.IsSolid(Direction.north);
                    swSolid = chunk.GetBlock(pos.Add(-1, -1, 1)).controller.IsSolid(Direction.north) && chunk.GetBlock(pos.Add(-1, -1, 1)).controller.IsSolid(Direction.east);

                    light = chunk.GetBlock(pos.Add(0, 0, 1)).data1 / 255f;

                    break;
                case Direction.east:
                    nSolid = chunk.GetBlock(pos.Add(1, 0, -1)).controller.IsSolid(Direction.up);
                    eSolid = chunk.GetBlock(pos.Add(1, 1, 0)).controller.IsSolid(Direction.west);
                    sSolid = chunk.GetBlock(pos.Add(1, 0, 1)).controller.IsSolid(Direction.down);
                    wSolid = chunk.GetBlock(pos.Add(1, -1, 0)).controller.IsSolid(Direction.east);

                    esSolid = chunk.GetBlock(pos.Add(1, 1, 1)).controller.IsSolid(Direction.west) && chunk.GetBlock(pos.Add(1, 1, 1)).controller.IsSolid(Direction.north);
                    neSolid = chunk.GetBlock(pos.Add(1, 1, -1)).controller.IsSolid(Direction.south) && chunk.GetBlock(pos.Add(1, 1, -1)).controller.IsSolid(Direction.west);
                    wnSolid = chunk.GetBlock(pos.Add(1, -1, -1)).controller.IsSolid(Direction.east) && chunk.GetBlock(pos.Add(1, -1, -1)).controller.IsSolid(Direction.north);
                    swSolid = chunk.GetBlock(pos.Add(1, -1, 1)).controller.IsSolid(Direction.north) && chunk.GetBlock(pos.Add(1, -1, 1)).controller.IsSolid(Direction.east);

                    light = chunk.GetBlock(pos.Add(1, 0, 0)).data1 / 255f;

                    break;
                case Direction.south:
                    nSolid = chunk.GetBlock(pos.Add(-1, 0, -1)).controller.IsSolid(Direction.down);
                    eSolid = chunk.GetBlock(pos.Add(0, 1, -1)).controller.IsSolid(Direction.west);
                    sSolid = chunk.GetBlock(pos.Add(1, 0, -1)).controller.IsSolid(Direction.up);
                    wSolid = chunk.GetBlock(pos.Add(0, -1, -1)).controller.IsSolid(Direction.south);

                    esSolid = chunk.GetBlock(pos.Add(1, 1, -1)).controller.IsSolid(Direction.west) && chunk.GetBlock(pos.Add(1, 1, -1)).controller.IsSolid(Direction.north);
                    neSolid = chunk.GetBlock(pos.Add(-1, 1, -1)).controller.IsSolid(Direction.south) && chunk.GetBlock(pos.Add(-1, 1, -1)).controller.IsSolid(Direction.west);
                    wnSolid = chunk.GetBlock(pos.Add(-1, -1, -1)).controller.IsSolid(Direction.east) && chunk.GetBlock(pos.Add(-1, -1, -1)).controller.IsSolid(Direction.north);
                    swSolid = chunk.GetBlock(pos.Add(1, -1, -1)).controller.IsSolid(Direction.north) && chunk.GetBlock(pos.Add(1, -1, -1)).controller.IsSolid(Direction.east);

                    light = chunk.GetBlock(pos.Add(0, 0, -1)).data1 / 255f;

                    break;
                case Direction.west:
                    nSolid = chunk.GetBlock(pos.Add(-1, 0, 1)).controller.IsSolid(Direction.up);
                    eSolid = chunk.GetBlock(pos.Add(-1, 1, 0)).controller.IsSolid(Direction.west);
                    sSolid = chunk.GetBlock(pos.Add(-1, 0, -1)).controller.IsSolid(Direction.down);
                    wSolid = chunk.GetBlock(pos.Add(-1, -1, 0)).controller.IsSolid(Direction.east);

                    esSolid = chunk.GetBlock(pos.Add(-1, 1, -1)).controller.IsSolid(Direction.west) && chunk.GetBlock(pos.Add(-1, 1, -1)).controller.IsSolid(Direction.north);
                    neSolid = chunk.GetBlock(pos.Add(-1, 1, 1)).controller.IsSolid(Direction.south) && chunk.GetBlock(pos.Add(-1, 1, 1)).controller.IsSolid(Direction.west);
                    wnSolid = chunk.GetBlock(pos.Add(-1, -1, 1)).controller.IsSolid(Direction.east) && chunk.GetBlock(pos.Add(-1, -1, 1)).controller.IsSolid(Direction.north);
                    swSolid = chunk.GetBlock(pos.Add(-1, -1, -1)).controller.IsSolid(Direction.north) && chunk.GetBlock(pos.Add(-1, -1, -1)).controller.IsSolid(Direction.east);

                    light = chunk.GetBlock(pos.Add(-1, 0, 0)).data1 / 255f;

                    break;
                default:
                    Debug.LogError("Direction not recognized");
                    break;
            }

            AddColors(meshData, wnSolid, nSolid, neSolid, eSolid, esSolid, sSolid, swSolid, wSolid, light);
        }
    }

    static void MakeStickColors(Chunk chunk, BlockPos pos, MeshData meshData, Direction direction)
    {
        bool nSolid = false;
        bool eSolid = false;
        bool sSolid = false;
        bool wSolid = false;

        bool wnSolid = false;
        bool neSolid = false;
        bool esSolid = false;
        bool swSolid = false;

        float light = 0;

        switch (direction)
        {
            case Direction.up:
                nSolid = chunk.GetBlock(pos.Add(0, 1, 1)).controller.IsSolid(Direction.south);
                eSolid = chunk.GetBlock(pos.Add(1, 1, 0)).controller.IsSolid(Direction.west);
                sSolid = chunk.GetBlock(pos.Add(0, 1, -1)).controller.IsSolid(Direction.north);
                wSolid = chunk.GetBlock(pos.Add(-1, 1, 0)).controller.IsSolid(Direction.east);

                wnSolid = chunk.GetBlock(pos.Add(-1, 1, 1)).controller.IsSolid(Direction.east) && chunk.GetBlock(pos.Add(-1, 1, 1)).controller.IsSolid(Direction.south);
                neSolid = chunk.GetBlock(pos.Add(1, 1, 1)).controller.IsSolid(Direction.south) && chunk.GetBlock(pos.Add(1, 1, 1)).controller.IsSolid(Direction.west);
                esSolid = chunk.GetBlock(pos.Add(1, 1, -1)).controller.IsSolid(Direction.west) && chunk.GetBlock(pos.Add(1, 1, -1)).controller.IsSolid(Direction.north);
                swSolid = chunk.GetBlock(pos.Add(-1, 1, -1)).controller.IsSolid(Direction.north) && chunk.GetBlock(pos.Add(-1, 1, -1)).controller.IsSolid(Direction.east);

                light = chunk.GetBlock(pos.Add(0, 1, 0)).data1 / 255f;

                break;
            case Direction.down:
                nSolid = chunk.GetBlock(pos.Add(0, -1, -1)).controller.IsSolid(Direction.south);
                eSolid = chunk.GetBlock(pos.Add(1, -1, 0)).controller.IsSolid(Direction.west);
                sSolid = chunk.GetBlock(pos.Add(0, -1, 1)).controller.IsSolid(Direction.north);
                wSolid = chunk.GetBlock(pos.Add(-1, -1, 0)).controller.IsSolid(Direction.east);

                wnSolid = chunk.GetBlock(pos.Add(-1, -1, -1)).controller.IsSolid(Direction.east) && chunk.GetBlock(pos.Add(-1, -1, -1)).controller.IsSolid(Direction.south);
                neSolid = chunk.GetBlock(pos.Add(1, -1, -1)).controller.IsSolid(Direction.south) && chunk.GetBlock(pos.Add(1, -1, -1)).controller.IsSolid(Direction.west);
                esSolid = chunk.GetBlock(pos.Add(1, -1, 1)).controller.IsSolid(Direction.west) && chunk.GetBlock(pos.Add(1, -1, 1)).controller.IsSolid(Direction.north);
                swSolid = chunk.GetBlock(pos.Add(-1, -1, 1)).controller.IsSolid(Direction.north) && chunk.GetBlock(pos.Add(-1, -1, 1)).controller.IsSolid(Direction.east);

                light = chunk.GetBlock(pos.Add(0, -1, 0)).data1 / 255f;

                break;
            case Direction.north:
                nSolid = chunk.GetBlock(pos.Add(1, 0, 1)).controller.IsSolid(Direction.west);
                eSolid = chunk.GetBlock(pos.Add(0, 1, 1)).controller.IsSolid(Direction.down);
                sSolid = chunk.GetBlock(pos.Add(-1, 0, 1)).controller.IsSolid(Direction.east);
                wSolid = chunk.GetBlock(pos.Add(0, -1, 1)).controller.IsSolid(Direction.up);

                esSolid = chunk.GetBlock(pos.Add(-1, 1, 1)).controller.IsSolid(Direction.east) && chunk.GetBlock(pos.Add(-1, 1, 1)).controller.IsSolid(Direction.south);
                neSolid = chunk.GetBlock(pos.Add(1, 1, 1)).controller.IsSolid(Direction.south) && chunk.GetBlock(pos.Add(1, 1, 1)).controller.IsSolid(Direction.west);
                wnSolid = chunk.GetBlock(pos.Add(1, -1, 1)).controller.IsSolid(Direction.west) && chunk.GetBlock(pos.Add(1, -1, 1)).controller.IsSolid(Direction.north);
                swSolid = chunk.GetBlock(pos.Add(-1, -1, 1)).controller.IsSolid(Direction.north) && chunk.GetBlock(pos.Add(-1, -1, 1)).controller.IsSolid(Direction.east);

                light = chunk.GetBlock(pos.Add(0, 0, 1)).data1 / 255f;

                break;
            case Direction.east:
                nSolid = chunk.GetBlock(pos.Add(1, 0, -1)).controller.IsSolid(Direction.up);
                eSolid = chunk.GetBlock(pos.Add(1, 1, 0)).controller.IsSolid(Direction.west);
                sSolid = chunk.GetBlock(pos.Add(1, 0, 1)).controller.IsSolid(Direction.down);
                wSolid = chunk.GetBlock(pos.Add(1, -1, 0)).controller.IsSolid(Direction.east);

                esSolid = chunk.GetBlock(pos.Add(1, 1, 1)).controller.IsSolid(Direction.west) && chunk.GetBlock(pos.Add(1, 1, 1)).controller.IsSolid(Direction.north);
                neSolid = chunk.GetBlock(pos.Add(1, 1, -1)).controller.IsSolid(Direction.south) && chunk.GetBlock(pos.Add(1, 1, -1)).controller.IsSolid(Direction.west);
                wnSolid = chunk.GetBlock(pos.Add(1, -1, -1)).controller.IsSolid(Direction.east) && chunk.GetBlock(pos.Add(1, -1, -1)).controller.IsSolid(Direction.north);
                swSolid = chunk.GetBlock(pos.Add(1, -1, 1)).controller.IsSolid(Direction.north) && chunk.GetBlock(pos.Add(1, -1, 1)).controller.IsSolid(Direction.east);

                light = chunk.GetBlock(pos.Add(1, 0, 0)).data1 / 255f;

                break;
            case Direction.south:
                nSolid = chunk.GetBlock(pos.Add(-1, 0, -1)).controller.IsSolid(Direction.down);
                eSolid = chunk.GetBlock(pos.Add(0, 1, -1)).controller.IsSolid(Direction.west);
                sSolid = chunk.GetBlock(pos.Add(1, 0, -1)).controller.IsSolid(Direction.up);
                wSolid = chunk.GetBlock(pos.Add(0, -1, -1)).controller.IsSolid(Direction.south);

                esSolid = chunk.GetBlock(pos.Add(1, 1, -1)).controller.IsSolid(Direction.west) && chunk.GetBlock(pos.Add(1, 1, -1)).controller.IsSolid(Direction.north);
                neSolid = chunk.GetBlock(pos.Add(-1, 1, -1)).controller.IsSolid(Direction.south) && chunk.GetBlock(pos.Add(-1, 1, -1)).controller.IsSolid(Direction.west);
                wnSolid = chunk.GetBlock(pos.Add(-1, -1, -1)).controller.IsSolid(Direction.east) && chunk.GetBlock(pos.Add(-1, -1, -1)).controller.IsSolid(Direction.north);
                swSolid = chunk.GetBlock(pos.Add(1, -1, -1)).controller.IsSolid(Direction.north) && chunk.GetBlock(pos.Add(1, -1, -1)).controller.IsSolid(Direction.east);

                light = chunk.GetBlock(pos.Add(0, 0, -1)).data1 / 255f;

                break;
            case Direction.west:
                nSolid = chunk.GetBlock(pos.Add(-1, 0, 1)).controller.IsSolid(Direction.up);
                eSolid = chunk.GetBlock(pos.Add(-1, 1, 0)).controller.IsSolid(Direction.west);
                sSolid = chunk.GetBlock(pos.Add(-1, 0, -1)).controller.IsSolid(Direction.down);
                wSolid = chunk.GetBlock(pos.Add(-1, -1, 0)).controller.IsSolid(Direction.east);

                esSolid = chunk.GetBlock(pos.Add(-1, 1, -1)).controller.IsSolid(Direction.west) && chunk.GetBlock(pos.Add(-1, 1, -1)).controller.IsSolid(Direction.north);
                neSolid = chunk.GetBlock(pos.Add(-1, 1, 1)).controller.IsSolid(Direction.south) && chunk.GetBlock(pos.Add(-1, 1, 1)).controller.IsSolid(Direction.west);
                wnSolid = chunk.GetBlock(pos.Add(-1, -1, 1)).controller.IsSolid(Direction.east) && chunk.GetBlock(pos.Add(-1, -1, 1)).controller.IsSolid(Direction.north);
                swSolid = chunk.GetBlock(pos.Add(-1, -1, -1)).controller.IsSolid(Direction.north) && chunk.GetBlock(pos.Add(-1, -1, -1)).controller.IsSolid(Direction.east);

                light = chunk.GetBlock(pos.Add(-1, 0, 0)).data1 / 255f;

                break;
            default:
                Debug.LogError("Direction not recognized");
                break;
        }

        AddColors(meshData, wnSolid, nSolid, neSolid, eSolid, esSolid, sSolid, swSolid, wSolid, light);
    }

    static void AddColors(MeshData meshData, bool wnSolid, bool nSolid, bool neSolid, bool eSolid, bool esSolid, bool sSolid, bool swSolid, bool wSolid, float light)
    {
        float ne = 1;
        float es = 1;
        float sw = 1;
        float wn = 1;

        float aoContrast = 0.2f;

        if (nSolid)
        {
            wn -= aoContrast;
            ne -= aoContrast;
        }

        if (eSolid)
        {
            ne -= aoContrast;
            es -= aoContrast;
        }

        if (sSolid)
        {
            es -= aoContrast;
            sw -= aoContrast;
        }

        if (wSolid)
        {
            sw -= aoContrast;
            wn -= aoContrast;
        }

        if (neSolid)
            ne -= aoContrast;

        if (swSolid)
            sw -= aoContrast;

        if (wnSolid)
            wn -= aoContrast;

        if (esSolid)
            es -= aoContrast;

        meshData.AddColors(ne, es, sw, wn, light);
    }
}
