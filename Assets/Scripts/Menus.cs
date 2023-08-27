using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour
{
    
    public GameObject videoInicio;
    public GameObject videoTutorial;


    private int sceneIndex;
    private float timer;


    void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        timer = 0f;
    }

    void Update()
    {
        if(sceneIndex == 0){
            timer += Time.deltaTime;
            print(timer);
            if(timer >= 8f){
                videoInicio.SetActive(false);
                videoTutorial.SetActive(true);
            }

            if(timer >= 14f){
                SceneManager.LoadScene(1);
            } else {
                return;
            }

        } else{
            return;
        }


    }

  
}
