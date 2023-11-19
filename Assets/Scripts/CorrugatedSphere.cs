using UnityEngine;

public class CorrugatedSphere : MonoBehaviour
{
    public int resolution = 50;
    public float initialRadius = 5f;
    public float initialWaveAmplitude = 0.5f;
    public float waveFrequency = 2f;
    public Material sphereMaterial;
    public float sizeChangeSpeed = 0.1f;

    private float currentRadius;
    private float currentWaveAmplitude;

    void Start()
    {
        currentRadius = initialRadius;
        currentWaveAmplitude = initialWaveAmplitude;

        CreateCorrugatedSphere();
    }

    void Update()
    {
        // Уменьшаем размер сферы по мере прохождения времени
        currentRadius -= sizeChangeSpeed * Time.deltaTime;
        currentRadius = Mathf.Max(currentRadius, 0.1f); // Минимальный радиус

        // Изменяем амплитуду волн для создания формы шапочки
        currentWaveAmplitude = Mathf.Sin(Time.time) * initialWaveAmplitude;

        // Пересоздаем сферу с обновленными параметрами
        CreateCorrugatedSphere();
    }

    void CreateCorrugatedSphere()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Mesh mesh = meshFilter.mesh;
        mesh.Clear();

        Vector3[] vertices = new Vector3[(resolution + 1) * (resolution + 1)];
        Vector2[] uv = new Vector2[vertices.Length];
        Vector3[] normals = new Vector3[vertices.Length];

        for (int i = 0; i <= resolution; i++)
        {
            for (int j = 0; j <= resolution; j++)
            {
                float u = (float)j / resolution;
                float v = (float)i / resolution;

                float theta = 2f * Mathf.PI * u;
                float phi = Mathf.PI * v;

                float x = currentRadius * Mathf.Sin(phi) * Mathf.Cos(theta);
                float y = currentRadius * Mathf.Cos(phi) + currentWaveAmplitude * Mathf.Sin(waveFrequency * theta);
                float z = currentRadius * Mathf.Sin(phi) * Mathf.Sin(theta);

                int index = i * (resolution + 1) + j;

                vertices[index] = new Vector3(x, y, z);
                uv[index] = new Vector2(u, v);
                normals[index] = new Vector3(x, y, z).normalized;
            }
        }

        int[] triangles = new int[resolution * resolution * 6];
        int triangleIndex = 0;

        for (int i = 0; i < resolution; i++)
        {
            for (int j = 0; j < resolution; j++)
            {
                int vertexIndex = i * (resolution + 1) + j;
                triangles[triangleIndex] = vertexIndex;
                triangles[triangleIndex + 1] = vertexIndex + resolution + 1;
                triangles[triangleIndex + 2] = vertexIndex + 1;

                triangles[triangleIndex + 3] = vertexIndex + 1;
                triangles[triangleIndex + 4] = vertexIndex + resolution + 1;
                triangles[triangleIndex + 5] = vertexIndex + resolution + 2;

                triangleIndex += 6;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.normals = normals;

        // Присваиваем материал
        GetComponent<MeshRenderer>().material = sphereMaterial;
    }
}
