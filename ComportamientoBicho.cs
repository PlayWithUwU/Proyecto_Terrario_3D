using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ComportamientoBicho : MonoBehaviour
{
    public float velocidadNormal = 1f;

    public float velocidadHambre = 2.5f;

    public float tiempoDeCambio = 4f;

    public float nutrientes = 40f;

    public float nutrientesMaximos = 100f;

    public float agua = 30f;

    public float aguaMaxima = 100f;

    public float radioDeteccion = 7f;

    public float duracionSaciedad = 10f;

    private Vector3 direccionMovimiento;

    private Rigidbody rb;

    private float cronometro;

    private float tiempoSaciedad;

    private Transform objetivoComida;

    private Transform objetivoAgua;

    public GameObject prefabBicho;

    public int reproduccionesNecesarias = 5;

    private int contadorReproductivo = 0;

    private bool listoParaReproducirse = false;

    private bool yaContabilizo95 = false;

    private ComportamientoBicho parejaObjetivo;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.freezeRotation = true;

        ElegirNuevaDireccion();
    }

    void Update()
    {
        cronometro += Time.deltaTime;

        if (tiempoSaciedad > 0f)
        {
            tiempoSaciedad -= Time.deltaTime;
        }

        nutrientes -= Time.deltaTime * 0.4f;

        agua -= Time.deltaTime * 0.3f;

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

        if (listoParaReproducirse)
        {
            BuscarPareja();
        }
        else
        {
            BuscarRecursos();
        }

        if (
            cronometro >= tiempoDeCambio
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
            objetivoComida != null
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
            objetivoAgua != null
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

        if (direccion != Vector3.zero)
        {
            Quaternion rotacionObjetivo =
                Quaternion.LookRotation(
                    direccion
                );

            rb.rotation =
                Quaternion.Slerp(
                    rb.rotation,
                    rotacionObjetivo,
                    Time.fixedDeltaTime * 5f
                );
        }
    }

    void BuscarRecursos()
    {
        if (tiempoSaciedad > 0f)
        {
            objetivoComida = null;
            objetivoAgua = null;
            return;
        }

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
                && nutrientes < 70f
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
                && nutrientes < 70f
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
            ComportamientoBicho otro =
                col.GetComponent<
                    ComportamientoBicho>();

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

    void ElegirNuevaDireccion()
    {
        float x =
            Random.Range(-1f, 1f);

        float z =
            Random.Range(-1f, 1f);

        direccionMovimiento =
            new Vector3(
                x,
                0,
                z
            ).normalized;
    }

    void Reproducirse(
        ComportamientoBicho pareja
    )
    {
        if (
            prefabBicho == null
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
                prefabBicho,
                posicionCria,
                Quaternion.identity
            );

        ComportamientoBicho bichoCria =
            cria.GetComponent<
                ComportamientoBicho>();

        if (bichoCria != null)
        {
            bichoCria.nutrientes = 40f;

            bichoCria.agua = 40f;
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

        ComportamientoBicho pareja =
            collision.gameObject
                .GetComponent<
                    ComportamientoBicho>();

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

            tiempoSaciedad =
                duracionSaciedad;

            objetivoComida = null;

            ElegirNuevaDireccion();

            Destroy(collision.gameObject);

            return;
        }

        Planta planta =
            collision.gameObject.GetComponent<Planta>();

        if (planta != null)
        {
            float nutrientesConsumidos = 20f;

            if (planta.nutrientes > 20f)
            {
                planta.nutrientes -=
                    nutrientesConsumidos;

                planta.nutrientes =
                    Mathf.Max(
                        planta.nutrientes,
                        20f
                    );

                nutrientes +=
                    nutrientesConsumidos;

                nutrientes = Mathf.Clamp(
                    nutrientes,
                    0,
                    nutrientesMaximos
                );

                tiempoSaciedad =
                    duracionSaciedad;

                objetivoComida = null;

                ElegirNuevaDireccion();
            }

            return;
        }

        Agua aguaObjeto =
            collision.gameObject.GetComponent<Agua>();

        if (aguaObjeto != null)
        {
            float aguaConsumida = 8f;

            if (
                aguaObjeto.cantidadAgua >=
                aguaConsumida
            )
            {
                aguaObjeto.cantidadAgua -=
                    aguaConsumida;

                agua += 20f;

                agua = Mathf.Clamp(
                    agua,
                    0,
                    aguaMaxima
                );

                tiempoSaciedad =
                    duracionSaciedad;

                objetivoAgua = null;

                ElegirNuevaDireccion();
            }
        }
    }
}