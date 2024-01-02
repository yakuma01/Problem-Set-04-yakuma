using UnityEngine;
using Random = UnityEngine.Random;

public class GenerateSurface : MonoBehaviour
{
    private Mesh _mesh;
    [SerializeField] private int xSize, zSize;
    private Vector3[] _vertices;
    private Vector4[] _tangents;
    private Vector2[] _uv;
    private int[] _triangles;

    private void Start()
    {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;

        CreateSurface();
        SetMeshValues();
    }

    private void CreateSurface()
    { 
        _vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        _uv = new Vector2[(xSize + 1) * (zSize + 1)];
        _tangents = new Vector4[_vertices.Length];
        var tangent = new Vector4(1f, 0f, 0f, -1f);

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int j = 0; j <= xSize; j++)
            {
                float y = 0;
                //Perlin noise -
                y = Mathf.PerlinNoise(j * 0.3f,z * 0.3f) * 2f;
                //Given in PS4 -
                //y = Mathf.Pow(2, -(((j-1) * (j - 1) + (y-2) * (y - 2))/(9))) + Mathf.Pow(1, -(((j + 1) * (j + 1) + (y + 3) * (y + 3)))) ;
                //y = Random.Range(0f, 1f);
                //Ripple effect y = Mathf.Sin(2 * (j ^ 2 + z ^ 2)) / 2f;
                //Conical - y = Mathf.Pow(j * j + z * z, 0.5f);
                
                _vertices[i] = new Vector3(j, y, z);
                _uv[i] = new Vector2((float)j / xSize, (float)z / zSize);
                _tangents[i] = tangent;
                i++;
            }
        }

        var v = 0;
        var t = 0;
        
        _triangles = new int[xSize * zSize * 6];

        for (int j = 0;  j < zSize; j++)
        {
            for (int i = 0; i < xSize; i++)
            {
                _triangles[t + 0] = v;
                _triangles[t + 1] = v + xSize + 1;
                _triangles[t + 2] = v + 1;
                _triangles[t + 3] = v + 1;
                _triangles[t + 4] = v + xSize + 1;
                _triangles[t + 5] = v + xSize + 2;
                v++;
                t += 6;
            }
            v++;
        }

    }

    private void SetMeshValues()
    {
        _mesh.Clear();
        _mesh.vertices = _vertices;
        _mesh.triangles = _triangles;
        _mesh.uv = _uv;
        _mesh.tangents = _tangents;
        _mesh.RecalculateNormals();
        _mesh.Optimize();
    }
}