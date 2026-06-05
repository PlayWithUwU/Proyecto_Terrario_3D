using UnityEngine;

public class ControlCamara : MonoBehaviour
{
    public float velocidad = 5f;

    public float sensibilidad = 2f;

    private float rotacionVertical = 0f;

    void Start()
    {
        Cursor.lockState =
            CursorLockMode.Locked;

        Cursor.visible = false;
    }

    void Update()
    {
        MovimientoMouse();

        MovimientoTeclado();
    }

    void MovimientoMouse()
    {
        float mouseX =
            Input.GetAxis("Mouse X") *
            sensibilidad;

        float mouseY =
            Input.GetAxis("Mouse Y") *
            sensibilidad;

        rotacionVertical -= mouseY;

        rotacionVertical =
            Mathf.Clamp(
                rotacionVertical,
                -90f,
                90f
            );

        transform.localRotation =
            Quaternion.Euler(
                rotacionVertical,
                0f,
                0f
            );

        transform.parent.Rotate(
            Vector3.up * mouseX
        );
    }

    void MovimientoTeclado()
    {
        float horizontal =
            Input.GetAxis("Horizontal");

        float vertical =
            Input.GetAxis("Vertical");

        Vector3 direccion =
            transform.parent.forward *
            vertical +
            transform.parent.right *
            horizontal;

        transform.parent.position +=
            direccion *
            velocidad *
            Time.deltaTime;
    }
}