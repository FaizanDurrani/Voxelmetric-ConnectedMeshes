using UnityEngine;
using System.Collections;

public class ProcMesh : BlockController
{
    public bool isSolid = false;
    public bool solidTowardsSameType = false;

    public ProcMesh() : base() { }

    public override void AddBlockData(Chunk chunk, BlockPos pos, MeshData meshData, Block block)
    {
        if ((isSolid || !solidTowardsSameType || chunk.GetBlock(pos.Add(0, 1, 0)) != block))
            BuildFace(chunk, pos, meshData, Direction.up, block);

        if ((isSolid || !solidTowardsSameType || chunk.GetBlock(pos.Add(0, -1, 0)) != block))
            BuildFace(chunk, pos, meshData, Direction.down, block);

        if ((isSolid || !solidTowardsSameType || chunk.GetBlock(pos.Add(0, 0, 1)) != block))
            BuildFace(chunk, pos, meshData, Direction.north, block);

        if ((isSolid || !solidTowardsSameType || chunk.GetBlock(pos.Add(0, 0, -1)) != block))
            BuildFace(chunk, pos, meshData, Direction.south, block);

        if ((isSolid || !solidTowardsSameType || chunk.GetBlock(pos.Add(1, 0, 0)) != block))
            BuildFace(chunk, pos, meshData, Direction.east, block);

        if ((isSolid || !solidTowardsSameType || chunk.GetBlock(pos.Add(-1, 0, 0)) != block))
            BuildFace(chunk, pos, meshData, Direction.west, block);
    }

    public virtual void BuildFace(Chunk chunk, BlockPos pos, MeshData meshData, Direction direction, Block block)
    {

    }
    public override string Name() { return "new Procedural Mesh"; }

    public override bool IsSolid(Direction direction) { return false; }

    public override bool IsTransparent() { return true; }

}
