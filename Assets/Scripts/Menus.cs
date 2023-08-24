using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menus : MonoBehaviour
{
    public GameObject botonInicio;
    public GameObject pantallaGrande;
    public GameObject medidores;
    public GameObject primeraPregunta;



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        botonInicio.SetActive(false);
        pantallaGrande.SetActive(true);
        primeraPregunta.SetActive(true);
        medidores.SetActive(true);
    }

}
