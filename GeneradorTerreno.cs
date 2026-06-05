using UnityEngine;

public class GeneradorTerreno : MonoBehaviour
{
    public GameObject fragmentoPrefab;
    public int cantidad = 50;

    void Start()
    {
        for (int i = 0; i < cantidad; i++)
        {
            Vector3 pos = new Vector3(
                Random.Range(-4, 4),
                Random.Range(3, 6),
                Random.Range(-4, 4)
            );

            Instantiate(fragmentoPrefab, pos, Quaternion.identity);
        }
    }
}