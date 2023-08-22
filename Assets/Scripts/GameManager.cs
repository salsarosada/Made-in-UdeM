using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject[] questions;
    public int mentalHealth = 100;
    public int fatigue = 0;
    public float average = 4.0f; // Nueva variable promedio.

    // Referencias a los textos en pantalla.
    public Text mentalHealthText;
    public Text fatigueText;
    public Text averageText; // Texto para el promedio.

    private int currentQuestionIndex = 0;

    public GameObject transitionCanvas; // Referencia al Canvas de transición.
    public Text mentalHealthChangeText; // Texto que muestra el cambio en salud mental.
    public Text fatigueChangeText; // Texto que muestra el cambio en cansancio.
    public Text averageChangeText; // Texto que muestra el cambio en promedio.
    public AudioClip[] backgroundMusics; // Array de música de fondo para cada estado.
    private AudioSource audioSource; // Fuente de audio para reproducir la música.



    private void Start()
    {
        UpdateUI();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        PlayBackgroundMusic();
    }

    public void MakeDecision(int mentalHealthChange, int fatigueChange, float averageChange, int nextQuestionIndex)
    {
        mentalHealth += mentalHealthChange;
        fatigue += fatigueChange;
        average += averageChange; // Cambiamos el promedio según la decisión.

        mentalHealth = Mathf.Clamp(mentalHealth, 0, 100);
        fatigue = Mathf.Clamp(fatigue, 0, 100);
        average = Mathf.Clamp(average, 0.0f, 5.0f); // Nos aseguramos de que el promedio esté entre 0 y 5.
        ShowTransition(mentalHealthChange, fatigueChange, averageChange);

        UpdateUI();

        ChangeQuestion(nextQuestionIndex);
        PlayBackgroundMusic();
    }

    private void UpdateUI()
    {
        mentalHealthText.text = "Salud Mental: " + mentalHealth;
        fatigueText.text = "Cansancio: " + fatigue;
        averageText.text = "Promedio: " + average.ToString("F1"); // Mostramos el promedio con 1 decimal.
    }
    private void ShowTransition(int mentalHealthChange, int fatigueChange, float averageChange)
    {
        // Actualizar los textos con los cambios.
        mentalHealthChangeText.text = mentalHealthChange > 0 ? "+" + mentalHealthChange : mentalHealthChange.ToString();
        fatigueChangeText.text = fatigueChange > 0 ? "+" + fatigueChange : fatigueChange.ToString();
        averageChangeText.text = averageChange > 0 ? "+" + averageChange.ToString("F1") : averageChange.ToString("F1");

        transitionCanvas.SetActive(true); // Mostrar el Canvas.

        // Desactivar el Canvas después de unos segundos.
        Invoke("HideTransition", 2f); // 2 segundos como ejemplo.
    }

    private void HideTransition()
    {
        transitionCanvas.SetActive(false);
    }

    private void ChangeQuestion(int nextQuestionIndex)
    {
        questions[currentQuestionIndex].SetActive(false);

        if (nextQuestionIndex < questions.Length)
        {
            currentQuestionIndex = nextQuestionIndex;
            questions[currentQuestionIndex].SetActive(true);
        }
        else
        {
            // Cambia a escena final o cualquier otro comportamiento deseado.
        }
    }
    private void PlayBackgroundMusic()
    {
        int musicIndex = 0;
        if (currentQuestionIndex < questions.Length / 3) // Primera tercera parte.
        {
            musicIndex = 0;
        }
        else if (currentQuestionIndex < (2 * questions.Length) / 3) // Segunda tercera parte.
        {
            musicIndex = 1;
        }
        else // Última tercera parte.
        {
            musicIndex = 2;
        }

        if (backgroundMusics.Length > musicIndex && backgroundMusics[musicIndex] != null)
        {
            audioSource.clip = backgroundMusics[musicIndex];
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }
}
