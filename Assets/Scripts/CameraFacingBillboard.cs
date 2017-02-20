using UnityEngine;

public class CameraFacingBillboard : MonoBehaviour
{
    private Camera m_Camera;
    private float heigth;

    void Start()
    {
        m_Camera = Camera.main;
        heigth = transform.localPosition.y;
    }
    void Update()
    {
        transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward,
            m_Camera.transform.rotation * Vector3.up);

        transform.position = transform.parent.position + Vector3.up * heigth;
    }
}