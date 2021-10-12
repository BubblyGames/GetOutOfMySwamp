using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshData
{
    public List<Vector3> vertices = new List<Vector3>();
    public List<int> triangles = new List<int>();
    public List<Vector2> uvs = new List<Vector2>();

    public List<Vector3> colliderVertices = new List<Vector3>();
    public List<int> colliderTriangles = new List<int>();

    public MeshData waterMesh;
    private bool isMainMesh = true;

    public MeshData(bool isMainMesh)
    {
        this.isMainMesh = isMainMesh;
        if (isMainMesh)
        {
            waterMesh = new MeshData(false);
        }
    }

    public void AddFace(Direction direction, int x, int y, int z, BlockType blockType)
    {
        if (blockType == BlockType.Swamp && isMainMesh)
        {
            waterMesh.AddFace(direction, x, y, z, blockType);
        }

        switch (direction)
        {
            case Direction.Back:
                AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
                AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
                AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
                AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
                break;
            case Direction.Front:
                AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
                AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
                AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
                AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
                break;
            case Direction.Left:
                AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
                AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
                AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
                AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
                break;
            case Direction.Right:
                AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
                AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
                AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
                AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
                break;
            case Direction.Down:
                AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
                AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
                AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
                AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
                break;
            case Direction.Up:
                AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
                AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
                AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
                AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
                break;
            default:
                break;
        }

        float u = 0;
        float v = 0;

        switch (blockType)
        {
            case BlockType.Air:
                break;
            case BlockType.Path:
                u = 0;
                v = .5f;
                break;
            case BlockType.Grass:
                u = .5f;
                v = .5f;
                break;
            case BlockType.Rock:
                u = 0;
                v = 0;
                break;
            case BlockType.Swamp:
                u = .5f;
                v = 0;
                break;
            default:
                break;
        }

        float offset = 0.063f;

        AddUV(new Vector2(u + offset, v + offset));
        AddUV(new Vector2(u + offset, v + .5f - offset));
        AddUV(new Vector2(u + .5f - offset, v + .5f - offset));
        AddUV(new Vector2(u + .5f - offset, v + offset));

        AddQuadTriangles();
    }

    public void AddVertex(Vector3 vertex)
    {
        vertices.Add(vertex);
    }

    public void AddUV(Vector2 uv)
    {
        uvs.Add(uv);
    }


    public void AddQuadTriangles()
    {
        triangles.Add(vertices.Count - 4);
        triangles.Add(vertices.Count - 3);
        triangles.Add(vertices.Count - 2);

        triangles.Add(vertices.Count - 4);
        triangles.Add(vertices.Count - 2);
        triangles.Add(vertices.Count - 1);
    }
}
