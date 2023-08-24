using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Txt : MonoBehaviour
{
    public TMP_Text textoTMP;
    public float velocidadEscritura = 0.05f;

    private string textoCompleto;
    private string textoActual = "";
    private bool escribiendo = false;

    private void Start()
    {
        textoTMP = GetComponent<TMP_Text>();
        textoCompleto = textoTMP.text;
        textoTMP.text = "";
        StartCoroutine(EscribirLetraPorLetra());
    }
    private IEnumerator EscribirLetraPorLetra()
    {
        for (int i = 0; i < textoCompleto.Length; i++)
        {
            if (!escribiendo)
            {
                yield return new WaitForSeconds(velocidadEscritura);
                escribiendo = true;
                textoActual += textoCompleto[i];
                textoTMP.text = textoActual;
                escribiendo = false;
            }
        }
    }
}
