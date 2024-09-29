using UnityEngine;
using UnityEngine.UI;

public class IdentificarObjeto : MonoBehaviour
{
    private float distanciaAlvo;
    private GameObject objArrastar, objPegar, objAlvo;
    public Text textoTecla, textMsg;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.frameCount % 5 ==0){
            objArrastar = null;
            objPegar = null;

            int ignorarLayer = 7;
            ignorarLayer = 1 << ignorarLayer;
            ignorarLayer = ~ignorarLayer;

            RaycastHit hit;
            if(Physics.SphereCast(transform.position,0.1f,transform.TransformDirection(Vector3.forward),out hit,5,ignorarLayer)){
                distanciaAlvo = hit.distance;

                if(objAlvo != null && hit.transform.gameObject != objAlvo){
                    objAlvo.GetComponent<Outline>().OutlineWidth = 0f;
                    objAlvo = null;
                    EsconderTexto();
                }

                if(hit.transform.gameObject.tag == "Arrastar"){
                    objArrastar = hit.transform.gameObject;
                    objAlvo = objArrastar;
                    //print("Arrastar " + objArrastar);

                    textoTecla.color = new Color(248/255f,248/255f,13/255f);
                    textMsg.color = textoTecla.color;
                    textoTecla.text = "[F]";
                    textMsg.text = "Arrastar/Soltar";
                }
                if(hit.transform.gameObject.tag =="Pegar"){
                    objPegar = hit.transform.gameObject;
                    objAlvo = objPegar;
                    //print("Pegar " + objPegar);

                    textoTecla.color = new Color(51/255f,1,0);
                    textMsg.color = textoTecla.color;
                    textoTecla.text = "[F]";
                    textMsg.text = "Pegar";
                }

                if(objAlvo !=null){
                    objAlvo.GetComponent<Outline>().OutlineWidth = 5f;
                }
            } else {
                if (objAlvo != null){
                    objAlvo.GetComponent<Outline>().OutlineWidth = 0f;
                    objAlvo = null;
                }
            }
        }
    }

    public float GetDistanciaAlvo(){
        return distanciaAlvo;
    }

    public GameObject GetObjArrastar(){
        return objArrastar;
    }

    public GameObject GetObjPegar(){
        return objPegar;
    }

    public void EsconderTexto(){
        textoTecla.text = "";
        textMsg.text = "";
    }
}
