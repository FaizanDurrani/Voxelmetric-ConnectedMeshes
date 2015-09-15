using UnityEngine;
using System.Collections;

public class ConnMeshDefinition : BlockDefinition
{

    public string[] textures = new string[6];

    public bool blockIsSolid = true;
    public bool solidTowardsSameType = true;

    public Vector3 MeshSize;
    public Vector3 ConnMeshSizeX;
    public Vector3 ConnMeshSizeY;
    public Vector3 ConnMeshSizeZ;

    //<summary>Only the values of the 1st must be changed</summary>
    public DirectionsEnabled[] EnabledDirections = new DirectionsEnabled[1];

    public override BlockController Controller()
    {
        BlockConnected controller = new BlockConnected(MeshSize, ConnMeshSizeX, ConnMeshSizeY, ConnMeshSizeZ, EnabledDirections[0]);
        controller.blockName = blockName;
        controller.isSolid = blockIsSolid;
        controller.solidTowardsSameType = solidTowardsSameType;

        TextureCollection[] textureCoordinates = new TextureCollection[6];

        for (int i = 0; i < 6; i++)
        {
            try
            {
                textureCoordinates[i] = Block.index.textureIndex.GetTextureCollection(textures[i]);
            }
            catch
            {
                if (Application.isPlaying)
                    Debug.LogError("Couldn't find texture for " + textures[i]);
            }
        }

        controller.textures = textureCoordinates;

        return controller;
    }
}
