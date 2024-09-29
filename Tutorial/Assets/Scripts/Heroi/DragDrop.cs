using UnityEngine;

public class DragDrop : MonoBehaviour
{
    Vector3 mousePosition;
    private Vector3 GetMousePosition(){
        return Camera.main.WorldToScreenPoint(transform.position);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public void Ativar(){
        mousePosition = Input.mousePosition - GetMousePosition();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePosition);  
    }
}
