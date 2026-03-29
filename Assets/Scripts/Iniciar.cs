using UnityEngine;
using UnityEngine.SceneManagement;

public class Iniciar : MonoBehaviour
{
    [SerializeField] private string nombreEscenaJuego = "Juego";

    public void IniciarPartida()
    {
        if (string.IsNullOrWhiteSpace(nombreEscenaJuego))
        {
            Debug.LogError("No se asigno la escena de juego en Iniciar.");
            return;
        }

        // Reinicia la velocidad del tiempo por si viene de un menu en pausa.
        Time.timeScale = 1f;
        SceneManager.LoadScene(nombreEscenaJuego);
    }
}
