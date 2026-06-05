using UnityEngine;

public class TierraAgrupacion : MonoBehaviour
{
    public float radio = 1.5f;
    public float fuerza = 5f;

    public float nutrientes = 70f;
    public float nutrientesMaximos = 100f;

    public LayerMask capaTierra;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Collider[] cercanos = Physics.OverlapSphere(
            transform.position,
            radio,
            capaTierra
        );

        foreach (Collider col in cercanos)
        {
            if (col.gameObject != gameObject)
            {
                Vector3 direccion =
                    col.transform.position - transform.position;

                rb.AddForce(direccion.normalized * fuerza);

                TierraAgrupacion otro =
                    col.GetComponent<TierraAgrupacion>();

                if (otro != null)
                {
                    float diferencia =
                        nutrientes - otro.nutrientes;

                    float transferencia =
                        diferencia * 0.01f * Time.deltaTime;

                    nutrientes -= transferencia;
                    otro.nutrientes += transferencia;
                }
            }
        }

        nutrientes =
            Mathf.Clamp(nutrientes, 0, nutrientesMaximos);
    }
}