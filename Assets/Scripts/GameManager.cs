using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject[] questions;
    public int mentalHealth = 100;
    public int fatigue = 0;
    public int motivation = 100;
    public float average = 4.0f; // Nueva variable promedio.

    // Referencias a los textos en pantalla.
    public Text mentalHealthText;
    public Text fatigueText;
    public Text averageText; // Texto para el promedio.
    public Text motivationText;

    private int currentQuestionIndex = 0;

    public GameObject transitionCanvas; // Referencia al Canvas de transición.
    public Text mentalHealthChangeText; // Texto que muestra el cambio en salud mental.
    public Text motivationChangeText;
    public Text fatigueChangeText; // Texto que muestra el cambio en cansancio.
    public Text averageChangeText; // Texto que muestra el cambio en promedio.
    public AudioClip[] backgroundMusics; // Array de música de fondo para cada estado.
    private AudioSource audioSource; // Fuente de audio para reproducir la música.

    public Image mentalHealthImage;
    public Image averageImage;
    public Image fatigueImage;
    public Image motivationImage;
    public Image mentalHealthImageTrans;
    public Image averageImageTrans;
    public Image fatigueImageTrans;
    public Image motivationImageTrans;

    public GameObject mentalHealthIcon;
    public GameObject motivationIcon;
    public GameObject fatigueIcon;
    public GameObject averageIcon;

    public TMP_Text conseTxt;
    public string[] consecuencias;
    public GameObject panelIdiomas;
    public GameObject panelEnfasis;
    public TMP_Text enfasisTxt;

    public float decisionTime = 10f; // Tiempo en segundos para tomar una decisión.
    private float currentTime;
    private bool isTimeOut = false; // Indica si el tiempo se ha agotado.

    public Image timeBarFill;
    private bool gameStarted = false;
    public GameObject timeBarBackground;

    //Boton del inicio
    public GameObject botonInicio;




    private void Start()
    {

        UpdateUI();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        

        SetOffIcons();
        currentTime = decisionTime; // Inicializa el temporizador.
        timeBarFill.gameObject.SetActive(false);
        timeBarBackground.SetActive(false);


    }

    public void MakeDecision(int mentalHealthChange, int motivationChange, int fatigueChange, float averageChange, int nextQuestionIndex)
    {
        mentalHealth += mentalHealthChange;
        fatigue += fatigueChange;
        average += averageChange; // Cambiamos el promedio según la decisión.
        motivation += motivationChange;

        mentalHealth = Mathf.Clamp(mentalHealth, 0, 100);
        fatigue = Mathf.Clamp(fatigue, 0, 100);
        motivation = Mathf.Clamp(motivation, 0, 100);
        average = Mathf.Clamp(average, 0, 100f); // Nos aseguramos de que el promedio esté entre 0 y 5.
        ShowTransition(mentalHealthChange, motivationChange, fatigueChange, averageChange);

        UpdateUI();

        StartCoroutine(ChangeQuestion(nextQuestionIndex, 5f));
        PlayBackgroundMusic();
        // Reinicia el temporizador.
        currentTime = decisionTime;
        isTimeOut = false;

    }
    private void Update()
    {
        // Si estás en la última pregunta, no hagas nada.
        if (currentQuestionIndex == questions.Length - 1) return;

        if (!gameStarted) return;
        // Si ya se mostró el Canvas de tiempo agotado, no hacer nada.
        if (isTimeOut) return;

        // Disminuye el tiempo.
        currentTime -= Time.deltaTime;
        timeBarFill.fillAmount = currentTime / decisionTime;

        // Si el tiempo se agota, muestra el Canvas y empeora las variables.
        if (currentTime <= 0)
        {
            isTimeOut = true;
            ShowTimeOutCanvas();
            EmpeorarVariables();
        }

    }
    private void ShowTimeOutCanvas()
    {
        conseTxt.SetText("La vida es para los rápidos, los lentos se quedan");
        ShowTransition(-10, -10, 10, -0.5f); // Estos valores son un ejemplo. Ajusta según lo que desees.
        if (currentQuestionIndex < questions.Length - 1)
        {
            currentQuestionIndex++;
        }

        // Verifica que solo la pregunta actual esté activa
        VerifyActiveQuestion();
    }

    private void EmpeorarVariables()
    {
        mentalHealth -= 10;
        fatigue += 10;
        motivation -= 10;
        average -= 0.5f;
        UpdateUI();
    }


    private void UpdateUI()
    {
        mentalHealthText.text = "Salud Mental: " + mentalHealth;
        fatigueText.text = "Cansancio: " + fatigue;
        motivationText.text = motivation.ToString();
        averageText.text = "Promedio: " + average.ToString("F1"); // Mostramos el promedio con 1 decimal.


        /*mentalHealthImage.fillAmount = mentalHealth / 100f;
        averageImage.fillAmount = average / 5f;
        fatigueImage.fillAmount = fatigue / 100f;*/

        StartCoroutine(UpdateImageFill(mentalHealthImageTrans, mentalHealth / 100f));
        StartCoroutine(UpdateImageFill(averageImageTrans, average / 100f));
        StartCoroutine(UpdateImageFill(fatigueImageTrans, fatigue / 100f));
        StartCoroutine(UpdateImageFill(motivationImageTrans, motivation / 100f));
    }
    private void ShowTransition(int mentalHealthChange, int motivationChange, int fatigueChange, float averageChange)
    {
        if (mentalHealthChange > 100)
        {
            mentalHealthChange = 100;
        }

        if (motivationChange > 100)
        {
            motivationChange = 100;
        }

        if (fatigueChange > 100)
        {
            fatigueChange = 100;
        }

        if (averageChange > 100)
        {
            averageChange = 100;
        }


        // Actualizar los textos con los cambios.
        mentalHealthChangeText.text = mentalHealthChange.ToString();
        fatigueChangeText.text = fatigueChange.ToString();
        averageChangeText.text = averageChange.ToString();
        motivationChangeText.text = motivationChange.ToString();

        transitionCanvas.SetActive(true); // Mostrar el Canvas.
        SetOffIcons();

        StartCoroutine(UpdateImageFill(mentalHealthImage, mentalHealth / 100f));
        StartCoroutine(UpdateImageFill(averageImage, average / 100f));
        StartCoroutine(UpdateImageFill(fatigueImage, fatigue / 100f));
        StartCoroutine(UpdateImageFill(motivationImage, motivation / 100f));

        // Desactivar el Canvas después de unos segundos.
        Invoke("HideTransition", 5f); // 2 segundos como ejemplo.

    }

    private void HideTransition()
    {
        transitionCanvas.SetActive(false);
        SetIcons();
        // No hay necesidad de cambiar o activar/desactivar las preguntas aquí.
    }

    private IEnumerator ChangeQuestion(int nextQuestionIndex, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (currentQuestionIndex < questions.Length)
        {
            Debug.Log("Desactivando pregunta: " + currentQuestionIndex);
            questions[currentQuestionIndex].SetActive(false);
        }

        if (nextQuestionIndex < questions.Length)
        {
            currentQuestionIndex = nextQuestionIndex;
            Debug.Log("Activando pregunta: " + currentQuestionIndex);

            // Aquí verificamos que solo la pregunta actual esté activa
            VerifyActiveQuestion();

            // Si estás en la última pregunta, oculta las UI
            if (currentQuestionIndex == questions.Length - 1)
            {
                HideUIElementsForFinalScene();
            }
            else
            {
                // Si no estás en la última pregunta, muestra las UI
                ShowUIElements();

                // Reinicia el temporizador cada vez que se muestra una nueva pregunta.
                currentTime = decisionTime;
                isTimeOut = false;
            }
        }
        else
        {
            // Cambia a la escena final o cualquier otro comportamiento deseado.
        }
    }

    private void VerifyActiveQuestion()
    {
        for (int i = 0; i < questions.Length; i++)
        {
            if (i == currentQuestionIndex)
            {
                questions[i].SetActive(true);
            }
            else
            {
                questions[i].SetActive(false);
            }
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
        if (i == 1)
        {
            i = Random.Range(1, 3);
            if (i == 1)
            {
                average += 7;

            }
            else
            {
                average -= 7;
                mentalHealth -= 7;
                motivation -= 3;
            }
        }

        if (i == 11)
        {
            i = Random.Range(11, 13);
            if (i == 11)
            {
                mentalHealth -= 5;
                motivation -= 5;
                average -= 3;
            }
            else
            {
                mentalHealth += 5;
                motivation += 5;
            }
        }

        if (i == 15)
        {
            i = Random.Range(15, 17);
            if (i == 15)
            {
                mentalHealth -= 3;
                motivation -= 3;

            }
            else
            {
                mentalHealth += 5;
                motivation += 5;
            }
        }

        if (i == 17)
        {
            Time.timeScale = 1;
            panelIdiomas.SetActive(false);
        }


        if (i == 18)
        {
            Time.timeScale = 1;
            panelIdiomas.SetActive(false);
        }

        conseTxt.SetText(consecuencias[i]);
        ShowTransition(mentalHealth, motivation, fatigue, average);

    }

    public void BotonInicio()
    {
        botonInicio.SetActive(false);
        questions[0].SetActive(true);
        PlayBackgroundMusic();

        // Si estás en la última pregunta
        if (currentQuestionIndex == questions.Length - 1)
        {
            HideUIElementsForFinalScene();
        }
        else
        {
            SetIcons();

            // Muestra la barra de tiempo y su fondo
            timeBarFill.gameObject.SetActive(true);
            timeBarBackground.SetActive(true);

            // Establece que el juego ha comenzado
            gameStarted = true;

            // Inicia el temporizador al mostrar la primera pregunta.
            currentTime = decisionTime;
            isTimeOut = false;
        }
    }
    private void ShowUIElements()
    {
        // Muestra los textos
        /* mentalHealthText.gameObject.SetActive(true);
        fatigueText.gameObject.SetActive(true);
        motivationText.gameObject.SetActive(true);
        averageText.gameObject.SetActive(true);
 */
        // Muestra las imágenes de estadísticas
        mentalHealthImage.gameObject.SetActive(true);
        averageImage.gameObject.SetActive(true);
        fatigueImage.gameObject.SetActive(true);
        motivationImage.gameObject.SetActive(true);
        mentalHealthImageTrans.gameObject.SetActive(true);
        averageImageTrans.gameObject.SetActive(true);
        fatigueImageTrans.gameObject.SetActive(true);
        motivationImageTrans.gameObject.SetActive(true);

        // Muestra los iconos
        mentalHealthIcon.SetActive(true);
        motivationIcon.SetActive(true);
        averageIcon.SetActive(true);
        fatigueIcon.SetActive(true);

        // Muestra la barra de tiempo
        timeBarFill.gameObject.SetActive(true);
        timeBarBackground.SetActive(true);
    }

    private void HideUIElementsForFinalScene()
    {
        // Oculta los textos
        mentalHealthText.gameObject.SetActive(false);
        fatigueText.gameObject.SetActive(false);
        motivationText.gameObject.SetActive(false);
        averageText.gameObject.SetActive(false);

        // Oculta las imágenes de estadísticas
        mentalHealthImage.gameObject.SetActive(false);
        averageImage.gameObject.SetActive(false);
        fatigueImage.gameObject.SetActive(false);
        motivationImage.gameObject.SetActive(false);
        mentalHealthImageTrans.gameObject.SetActive(false);
        averageImageTrans.gameObject.SetActive(false);
        fatigueImageTrans.gameObject.SetActive(false);
        motivationImageTrans.gameObject.SetActive(false);

        // Oculta los iconos
        mentalHealthIcon.SetActive(false);
        motivationIcon.SetActive(false);
        averageIcon.SetActive(false);
        fatigueIcon.SetActive(false);

        // Oculta la barra de tiempo
        timeBarFill.gameObject.SetActive(false);
        timeBarBackground.SetActive(false);
    }


    private void HideEnfasis()
    {
        panelEnfasis.SetActive(false);
    }

    public void Idiomas()
    {
        Time.timeScale = 0;
        panelIdiomas.SetActive(true);
    }

    public void SetIcons()
    {
        mentalHealthIcon.SetActive(true);
        motivationIcon.SetActive(true);
        averageIcon.SetActive(true);
        fatigueIcon.SetActive(true);
    }

     public void SetOffIcons()
    {
        mentalHealthIcon.SetActive(false);
        motivationIcon.SetActive(false);
        averageIcon.SetActive(false);
        fatigueIcon.SetActive(false);
    }
}
