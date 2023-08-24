using System.Collections;
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

    public Image mentalHealthImage;
    public Image averageImage;
    public Image fatigueImage;
    public Image mentalHealthImageTrans;
    public Image averageImageTrans;
    public Image fatigueImageTrans;

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

        /*mentalHealthImage.fillAmount = mentalHealth / 100f;
        averageImage.fillAmount = average / 5f;
        fatigueImage.fillAmount = fatigue / 100f;*/

        StartCoroutine(UpdateImageFill(mentalHealthImageTrans, mentalHealth / 100f));
        StartCoroutine(UpdateImageFill(averageImageTrans, average / 5f));
        StartCoroutine(UpdateImageFill(fatigueImageTrans, fatigue / 100f));
    }
    private void ShowTransition(int mentalHealthChange, int fatigueChange, float averageChange)
    {
        // Actualizar los textos con los cambios.
        mentalHealthChangeText.text = mentalHealthChange > 0 ? "+" + mentalHealthChange : mentalHealthChange.ToString();
        fatigueChangeText.text = fatigueChange > 0 ? "+" + fatigueChange : fatigueChange.ToString();
        averageChangeText.text = averageChange > 0 ? "+" + averageChange.ToString("F1") : averageChange.ToString("F1");

        transitionCanvas.SetActive(true); // Mostrar el Canvas.

        StartCoroutine(UpdateImageFill(mentalHealthImage, mentalHealth / 100f));
        StartCoroutine(UpdateImageFill(averageImage, average / 5f));
        StartCoroutine(UpdateImageFill(fatigueImage, fatigue / 100f));

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

    private IEnumerator UpdateImageFill(Image image, float targetFill)
    {
        float initialFill = image.fillAmount;
        float elapsedTime = 0f;
        float duration = 1f; // Duración de la transición

        while (elapsedTime < duration)
        {
            float fillAmount = Mathf.Lerp(initialFill, targetFill, elapsedTime / duration);
            image.fillAmount = fillAmount;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Asegurarse de que la imagen tenga el valor exacto al final
        image.fillAmount = targetFill;
    }

    public void Consecuencia(int i)
    {
        if(i ==1)
        {
                i = Random.Range(1, 3); 
                if(i == 1)
            {
                average += 7;

            }
            else
            {
                average -= 7;
                mentalHealth -= 7;
            }
        }
        conseTxt.SetText(consecuencias[i]);
    }
}
