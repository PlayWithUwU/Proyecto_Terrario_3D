using UnityEngine;

public class Musgo : MonoBehaviour
{
    public float nutrientes = 10f;

    public float nutrientesMaximos = 50f;

    public float velocidadCrecimiento = 0.03f;

    public float tamañoMaximo = 2f;

    public LayerMask superficiesValidas;

    private CicloLuz cicloLuz;

    void Start()
    {
        cicloLuz = FindObjectOfType<CicloLuz>();

        VerificarSuperficie();
    }

    void Update()
    {
        if (cicloLuz != null)
        {
            float absorbido =
                cicloLuz.ConsumirNutrientes(
                    Time.deltaTime * 2f
                );

            nutrientes += absorbido;

            nutrientes =
                Mathf.Clamp(
                    nutrientes,
                    0,
                    nutrientesMaximos
                );
        }

        if (nutrientes > 0)
        {
            Crecer();
        }
    }

    void Crecer()
    {
        if (
            transform.localScale.y < tamañoMaximo
            || transform.localScale.z < tamañoMaximo
        )
        {
            transform.localScale +=
                new Vector3(
                    0f,
                    velocidadCrecimiento * 0.5f * Time.deltaTime,
                    velocidadCrecimiento * 1.5f * Time.deltaTime
                );

            nutrientes -= Time.deltaTime;
        }
    }

    void VerificarSuperficie()
    {
        RaycastHit hit;

        if (
            Physics.Raycast(
                transform.position,
                -transform.up,
                out hit,
                2f
            )
        )
        {
            if (
                hit.collider.gameObject.layer ==
                LayerMask.NameToLayer("Planta")
            )
            {
                Destroy(gameObject);
            }
        }
    }
}