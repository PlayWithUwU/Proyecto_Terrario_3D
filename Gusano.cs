using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Gusano : MonoBehaviour
{
    public float velocidadNormal = 0.6f;

    public float velocidadHambre = 1.8f;

    public float tiempoCambioDireccion = 5f;

    public float profundidadBajoTierra = -2f;

    public float velocidadEnterrarse = 0.8f;

    public float nutrientes = 30f;

    public float nutrientesMaximos = 100f;

    public float agua = 30f;

    public float aguaMaxima = 100f;

    public float radioDeteccion = 6f;

    private Rigidbody rb;

    private Vector3 direccionMovimiento;

    private float cronometro;

    private CicloLuz cicloLuz;

    private bool esDeNoche;

    private Transform objetivoComida;

    private Transform objetivoAgua;

    public GameObject prefabGusano;

    public int reproduccionesNecesarias = 5;

    private int contadorReproductivo = 0;

    private bool listoParaReproducirse = false;

    private bool yaContabilizo95 = false;

    private Gusano parejaObjetivo;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        cicloLuz =
            FindFirstObjectByType<CicloLuz>();

        ElegirNuevaDireccion();
    }

    void Update()
    {
        if (cicloLuz == null)
            return;

        nutrientes -= Time.deltaTime * 0.25f;

        agua -= Time.deltaTime * 0.2f;

        nutrientes = Mathf.Clamp(
            nutrientes,
            0,
            nutrientesMaximos
        );

        if (
            nutrientes >= 95f
            && !yaContabilizo95
        )
        {
            contadorReproductivo++;

            yaContabilizo95 = true;

            if (
                contadorReproductivo >=
                reproduccionesNecesarias
            )
            {
                listoParaReproducirse = true;
            }
        }

        if (nutrientes < 95f)
        {
            yaContabilizo95 = false;
        }

        agua = Mathf.Clamp(
            agua,
            0,
            aguaMaxima
        );

        VerificarDiaNoche();

        if (listoParaReproducirse)
        {
            BuscarPareja();
        }
        else
        {
            BuscarRecursos();
        }

        cronometro += Time.deltaTime;

        if (
            cronometro >= tiempoCambioDireccion
            && objetivoComida == null
            && objetivoAgua == null
        )
        {
            ElegirNuevaDireccion();

            cronometro = 0;
        }
    }

    void FixedUpdate()
    {
        if (esDeNoche)
        {
            MovimientoSuperficie();
        }
        else
        {
            Enterrarse();
        }
    }

void BuscarRecursos()
{
    objetivoComida = null;

    objetivoAgua = null;

    Collider[] objetos =
        Physics.OverlapSphere(
            transform.position,
            radioDeteccion
        );

    float distanciaComida =
        Mathf.Infinity;

    float distanciaAgua =
        Mathf.Infinity;

    foreach (Collider col in objetos)
    {
        Hoja hoja =
            col.GetComponent<Hoja>();

        if (
            hoja != null
            && nutrientes < 50f
        )
        {
            float distancia =
                Vector3.Distance(
                    transform.position,
                    col.transform.position
                );

            if (distancia < distanciaComida)
            {
                distanciaComida =
                    distancia;

                objetivoComida =
                    col.transform;
            }
        }

        Planta planta =
            col.GetComponent<Planta>();

        if (
            planta != null
            && planta.nutrientes > 20f
            && nutrientes < 50f
        )
        {
            float distancia =
                Vector3.Distance(
                    transform.position,
                    col.transform.position
                );

            if (distancia < distanciaComida)
            {
                distanciaComida =
                    distancia;

                objetivoComida =
                    col.transform;
            }
        }

        Agua aguaObjeto =
            col.GetComponent<Agua>();

        if (
            aguaObjeto != null
            && agua < 50f
        )
        {
            float distancia =
                Vector3.Distance(
                    transform.position,
                    col.transform.position
                );

            if (distancia < distanciaAgua)
            {
                distanciaAgua =
                    distancia;

                objetivoAgua =
                    col.transform;
            }
        }
    }
}
    void BuscarPareja()
    {
        parejaObjetivo = null;

        Collider[] objetos =
            Physics.OverlapSphere(
                transform.position,
                radioDeteccion
            );

        float distanciaMasCercana =
            Mathf.Infinity;

        foreach (Collider col in objetos)
        {
            Gusano otro =
                col.GetComponent<Gusano>();

            if (
                otro != null
                && otro != this
                && otro.listoParaReproducirse
            )
            {
                float distancia =
                    Vector3.Distance(
                        transform.position,
                        otro.transform.position
                    );

                if (
                    distancia <
                    distanciaMasCercana
                )
                {
                    distanciaMasCercana =
                        distancia;

                    parejaObjetivo =
                        otro;
                }
            }
        }
    }

    void VerificarDiaNoche()
    {
        if (cicloLuz.sol.transform.forward.y < 0)
        {
            esDeNoche = true;
        }
        else
        {
            esDeNoche = false;
        }
    }

    void MovimientoSuperficie()
    {
        float velocidadActual =
            velocidadNormal;

        Vector3 direccion =
            direccionMovimiento;

        if (
            listoParaReproducirse
            && parejaObjetivo != null
        )
        {
            direccion =
                (
                    parejaObjetivo.transform.position
                    - transform.position
                ).normalized;
        }

        if (
            nutrientes < 20f
            || agua < 20f
        )
        {
            velocidadActual =
                velocidadHambre;
        }

        if (
            !listoParaReproducirse
            && objetivoComida != null
            && nutrientes < agua
        )
        {
            direccion =
                (
                    objetivoComida.position
                    - transform.position
                ).normalized;
        }

        if (
            !listoParaReproducirse
            && objetivoAgua != null
            && agua < nutrientes
        )
        {
            direccion =
                (
                    objetivoAgua.position
                    - transform.position
                ).normalized;
        }

        Vector3 movimiento =
            direccion *
            velocidadActual *
            Time.fixedDeltaTime;

        rb.MovePosition(
            rb.position + movimiento
        );

        Vector3 posicion =
            rb.position;

        posicion.y =
            Mathf.Lerp(
                posicion.y,
                0.2f,
                Time.fixedDeltaTime *
                velocidadEnterrarse
            );

        rb.MovePosition(posicion);

        if (direccion != Vector3.zero)
        {
            Quaternion rotacion =
                Quaternion.LookRotation(
                    direccion
                );

            rb.rotation =
                Quaternion.Slerp(
                    rb.rotation,
                    rotacion,
                    Time.fixedDeltaTime * 5f
                );
        }
    }

    void Enterrarse()
    {
        Vector3 posicion =
            rb.position;

        posicion.y =
            Mathf.Lerp(
                posicion.y,
                profundidadBajoTierra,
                Time.fixedDeltaTime *
                velocidadEnterrarse
            );

        rb.MovePosition(posicion);
    }

    void ElegirNuevaDireccion()
    {
        float x =
            Random.Range(-1f, 1f);

        float z =
            Random.Range(-1f, 1f);

        direccionMovimiento =
            new Vector3(x, 0, z).normalized;
    }

    void Reproducirse(
    Gusano pareja
)
    {
        if (
            prefabGusano == null
            || pareja == null
        )
        {
            return;
        }

        Vector3 posicionCria =
            (
                transform.position
                + pareja.transform.position
            ) / 2f;

        GameObject cria =
            Instantiate(
                prefabGusano,
                posicionCria,
                Quaternion.identity
            );

        Gusano gusanoCria =
            cria.GetComponent<Gusano>();

        if (gusanoCria != null)
        {
            gusanoCria.nutrientes = 40f;

            gusanoCria.agua = 40f;
        }

        nutrientes -= 60f;

        pareja.nutrientes -= 60f;

        nutrientes =
            Mathf.Max(
                nutrientes,
                20f
            );

        pareja.nutrientes =
            Mathf.Max(
                pareja.nutrientes,
                20f
            );

        contadorReproductivo = 0;

        pareja.contadorReproductivo = 0;

        listoParaReproducirse = false;

        pareja.listoParaReproducirse = false;

        yaContabilizo95 = false;

        pareja.yaContabilizo95 = false;

        parejaObjetivo = null;

        pareja.parejaObjetivo = null;

        ElegirNuevaDireccion();

        pareja.ElegirNuevaDireccion();
    }

    void OnCollisionEnter(Collision collision)
    {

        Gusano pareja =
            collision.gameObject
                .GetComponent<Gusano>();

        if (
            pareja != null
            && listoParaReproducirse
            && pareja.listoParaReproducirse
        )
        {
            Reproducirse(pareja);

            return;
        }

        Hoja hoja =
            collision.gameObject.GetComponent<Hoja>();

        if (hoja != null)
        {
            nutrientes += hoja.nutrientes;

            nutrientes = Mathf.Clamp(
                nutrientes,
                0,
                nutrientesMaximos
            );

            Destroy(collision.gameObject);

            return;
        }

        Agua aguaObjeto =
            collision.gameObject.GetComponent<Agua>();

        if (aguaObjeto != null)
        {
            float aguaConsumida = 5f;

            if (
                aguaObjeto.cantidadAgua >=
                aguaConsumida
            )
            {
                aguaObjeto.cantidadAgua -=
                    aguaConsumida;

                agua += 15f;

                agua = Mathf.Clamp(
                    agua,
                    0,
                    aguaMaxima
                );
            }
        }
    }
}