using UnityEngine;

public class Fusionable : MonoBehaviour
{
    public float tamaño = 1f;
    public float tamañoMaximo = 3f;

    public float nutrientes = 40f;
    public float nutrientesMaximos = 80f;

    private bool puedeFusionar = true;

    void OnCollisionEnter(Collision collision)
    {
        if (!puedeFusionar) return;

        Fusionable otro = collision.gameObject.GetComponent<Fusionable>();

        if (otro != null && otro != this && otro.puedeFusionar)
        {
            Absorber(otro);
        }
    }

    void Absorber(Fusionable otro)
    {
        puedeFusionar = false;
        otro.puedeFusionar = false;

        tamaño += otro.tamaño;

        tamaño = Mathf.Min(tamaño, tamañoMaximo);

        nutrientes += otro.nutrientes;

        nutrientes = Mathf.Min(nutrientes, nutrientesMaximos);

        transform.localScale = new Vector3(
            tamaño,
            transform.localScale.y,
            tamaño
        );

        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.mass = tamaño;
        }

        Destroy(otro.gameObject);

        Invoke(nameof(ActivarFusion), 0.2f);
    }

    void ActivarFusion()
    {
        puedeFusionar = true;
    }
}