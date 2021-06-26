using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Transform sphere;
    [SerializeField] private Vector3 offset;
    
    private Camera cam;
    
    void Start()
    {
        cam = Camera.main;
    }
    
    void Update()
    {
        if (!Input.GetMouseButton(0)) return;

        var ray = cam.ScreenPointToRay(Input.mousePosition);
        var isHit = Physics.Raycast(ray, out var hitInfo, 50f, 1 << 6);
        if (!isHit) return;

        sphere.transform.position = hitInfo.point + offset;
    }
}
