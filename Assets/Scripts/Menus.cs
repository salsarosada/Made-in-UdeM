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
    public AudioClip recordInProgres; // Clip de audio para el estado inicial.
    public AudioClip Circus; // Clip de audio para el estado de circo.
    private AudioSource audioSource; // Fuente de audio para reproducir la música.

    void Start()
    {
        // Inicializar la fuente de audio.
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // Si no hay un AudioSource en este objeto, crear uno.
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Poner el primer clip de audio y reproducirlo.
        if (recordInProgres != null)
        {
            audioSource.clip = recordInProgres;
            audioSource.Play();
        }

        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        timer = 0f;
    }

    void Update()
    {
        if (sceneIndex == 0)
        {
            timer += Time.deltaTime;
            print(timer);

            if (timer >= 8f && audioSource.clip != Circus)
            {
                // Cambiar a Circus y reproducir.
                if (Circus != null)
                {
                    audioSource.clip = Circus;
                    audioSource.Play();
                }
                
                videoInicio.SetActive(false);
                videoTutorial.SetActive(true);
            }

            if (timer >= 18f)
            {
                SceneManager.LoadScene(1);
            }
        }
    }
}
