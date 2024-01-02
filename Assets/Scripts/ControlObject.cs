using UnityEngine;

public class ControlObject: MonoBehaviour
{
    private Vector2 _prevPosition;
    [SerializeField] private GameObject parent;
    private const float Speed = 10f;
    private const float Radius = 6;
    
    private void OnMouseDown() {
        _prevPosition = Input.mousePosition;
    }
    
    private void OnMouseDrag() {
        Vector2 curPosition = Input.mousePosition;
        var delta = curPosition - _prevPosition;
        
        var dx = delta.x;
        var dy = delta.y;
        
        var dr = Mathf.Pow(dx*dx + dy*dy, 0.5f);
        if (dr < double.Epsilon)
        {
            return;
        }
        var n = new Vector3(-dy/dr, dx/dr, 0);
        
        var theta = dr/Radius;
        transform.parent = parent.transform;
        parent.transform.localRotation *= Quaternion.Euler(-n*theta*Speed);
        _prevPosition = curPosition;
        delta = curPosition - _prevPosition;
    }
}
