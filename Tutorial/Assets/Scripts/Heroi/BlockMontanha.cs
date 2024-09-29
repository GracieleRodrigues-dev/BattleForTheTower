using UnityEngine;

public class BlockMontanha : MonoBehaviour
{
    public GameObject texto;

    void OnTriggerEnter(Collider other){
        texto.SetActive(true);
    }

    void OnTriggerExit(Collider other){
        texto.SetActive(false);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
