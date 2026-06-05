using UnityEngine;

public class Planta : MonoBehaviour
{
    public float nutrientes = 10f;

    public float nutrientesMaximos = 100f;

    public float crecimiento = 0.015f;

    public float tamañoMaximo = 2.5f;

    public float nutrientesParaMadurar = 60f;

    public float nutrientesParaSoltarHojas = 60f;

    public GameObject prefabHoja;

    public Material materialPlantaPequeña;

    public Material materialPlantaGrande;

    public float tiempoEntreHojas = 8f;

    private float cronometroHojas = 0f;

    private bool esAdulta = false;

    public bool EsAdulta
    {
        get { return esAdulta; }
    }

    private CicloLuz cicloLuz;

    private Renderer renderizador;

    void Start()
    {
        cicloLuz =
            FindFirstObjectByType<CicloLuz>();

        renderizador =
            GetComponentInChildren<Renderer>();

        if (renderizador != null)
        {
            renderizador.material =
                materialPlantaPequeña;
        }
    }

    void Update()
    {
        if (cicloLuz != null)
        {
            nutrientes +=
                cicloLuz.nutrientesSolares *
                crecimiento *
                Time.deltaTime;
        }

        nutrientes =
            Mathf.Clamp(
                nutrientes,
                0,
                nutrientesMaximos
            );

        float tamaño =
            1 +
            (nutrientes / nutrientesMaximos)
            * tamañoMaximo;

        transform.localScale = new Vector3(
            1f + (tamaño * 0.08f),
            tamaño,
            1f + (tamaño * 0.08f)
        );

        if (
            nutrientes >= nutrientesParaMadurar
            && !esAdulta
        )
        {
            ConvertirEnAdulta();
        }

        cronometroHojas += Time.deltaTime;

        if (
            nutrientes >= nutrientesParaSoltarHojas
            && cronometroHojas >= tiempoEntreHojas
        )
        {
            SoltarHoja();

            cronometroHojas = 0f;
        }
    }

    void ConvertirEnAdulta()
    {
        esAdulta = true;

        if (
            renderizador != null
            && materialPlantaGrande != null
        )
        {
            renderizador.material =
                materialPlantaGrande;
        }
    }

    void SoltarHoja()
    {
        if (prefabHoja == null)
            return;

        Vector3 posicionHoja =
            transform.position +
            Vector3.up;

        GameObject hoja =
            Instantiate(
                prefabHoja,
                posicionHoja,
                Quaternion.identity
            );

        Hoja hojaScript =
            hoja.GetComponent<Hoja>();

        if (hojaScript != null)
        {
            hojaScript.nutrientes =
                nutrientes * 0.08f;
        }

        nutrientes -= 35f;
    }
}