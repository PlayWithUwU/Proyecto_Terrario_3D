using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera camara;

    void Start()
    {
        camara = Camera.main;
    }

    void LateUpdate()
    {
        transform.LookAt(
            transform.position +
            camara.transform.forward
        );
    }
}