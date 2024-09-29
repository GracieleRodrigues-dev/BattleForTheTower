using UnityEngine;

public class MagazineGlock : MonoBehaviour, IPegavel
{
    // Start is called before the first frame update
    public void Pegar(){
        Glock g = GameObject.FindWithTag("Arma").GetComponent<Glock>();
        g.AddCarregador();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
