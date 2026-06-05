using UnityEngine;

public class CicloLuz : MonoBehaviour
{
    public Light sol;

    public float velocidadTiempo = 18f;

    public float nutrientesSolares = 100f;

    public float regeneracionNutrientes = 5f;

    public float nutrientesMaximos = 100f;

    void Update()
    {
        if (sol != null)
        {
            float rotacion =
                velocidadTiempo *
                Time.deltaTime;

            sol.transform.Rotate(
                Vector3.right * rotacion
            );
        }

        nutrientesSolares +=
            regeneracionNutrientes *
            Time.deltaTime;

        nutrientesSolares =
            Mathf.Clamp(
                nutrientesSolares,
                0,
                nutrientesMaximos
            );
    }

    public float ConsumirNutrientes(float cantidad)
    {
        float nutrientesDisponibles =
            Mathf.Min(
                cantidad,
                nutrientesSolares
            );

        nutrientesSolares -=
            nutrientesDisponibles;

        return nutrientesDisponibles;
    }
}