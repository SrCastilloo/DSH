using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinDePartida : MonoBehaviour
{
    [SerializeField] private string nombreEscenaInicio = "Inicio";

    private Canvas canvas;
    private GameObject panelFin;
    private Button botonSalir;

    private void Start()
    {
        CrearUI();
    }

    private void CrearUI()
    {
        // --- EventSystem (necesario para que funcionen los botones) ---
        if (FindAnyObjectByType<EventSystem>() == null)
        {
            var esObj = new GameObject("EventSystem");
            esObj.AddComponent<EventSystem>();
            esObj.AddComponent<StandaloneInputModule>();
        }

        // --- Canvas ---
        var canvasObj = new GameObject("CanvasFinDePartida");
        canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;
        canvasObj.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasObj.AddComponent<GraphicRaycaster>();

        // --- Boton esquina superior derecha ---
        var btnObj = CrearBoton(canvasObj.transform, "BotonSalir", "Salir",
            new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(-80f, -30f),
            new Vector2(120f, 50f), new Color(0.8f, 0.2f, 0.2f, 1f));
        botonSalir = btnObj.GetComponent<Button>();
        botonSalir.onClick.AddListener(MostrarPantallaFin);

        // --- Panel de fin de partida (oculto al inicio) ---
        panelFin = new GameObject("PanelFinDePartida");
        panelFin.transform.SetParent(canvasObj.transform, false);
        var panelRect = panelFin.AddComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;

        var panelImg = panelFin.AddComponent<Image>();
        panelImg.color = new Color(0f, 0f, 0f, 0.85f);

        // Titulo
        var tituloObj = new GameObject("Titulo");
        tituloObj.transform.SetParent(panelFin.transform, false);
        var tituloRect = tituloObj.AddComponent<RectTransform>();
        tituloRect.anchorMin = new Vector2(0.5f, 0.65f);
        tituloRect.anchorMax = new Vector2(0.5f, 0.65f);
        tituloRect.anchoredPosition = Vector2.zero;
        tituloRect.sizeDelta = new Vector2(600f, 80f);
        var tituloText = tituloObj.AddComponent<Text>();
        tituloText.text = "Fin de Partida";
        tituloText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        tituloText.fontSize = 48;
        tituloText.color = Color.white;
        tituloText.alignment = TextAnchor.MiddleCenter;

        // Boton salir definitivo
        var btnSalirDef = CrearBoton(panelFin.transform, "BotonSalirDefinitivo", "Salir del Juego",
            new Vector2(0.5f, 0.40f), new Vector2(0.5f, 0.40f), Vector2.zero,
            new Vector2(280f, 60f), new Color(0.8f, 0.2f, 0.2f, 1f));
        btnSalirDef.GetComponent<Button>().onClick.AddListener(SalirDefinitivo);

        // Boton volver a la partida
        var btnVolver = CrearBoton(panelFin.transform, "BotonVolver", "Volver",
            new Vector2(0.5f, 0.28f), new Vector2(0.5f, 0.28f), Vector2.zero,
            new Vector2(280f, 60f), new Color(0.3f, 0.3f, 0.3f, 1f));
        btnVolver.GetComponent<Button>().onClick.AddListener(OcultarPantallaFin);

        panelFin.SetActive(false);
    }

    private GameObject CrearBoton(Transform parent, string nombre, string texto,
        Vector2 anchorMin, Vector2 anchorMax, Vector2 anchoredPos,
        Vector2 size, Color color)
    {
        var btnObj = new GameObject(nombre);
        btnObj.transform.SetParent(parent, false);

        var rect = btnObj.AddComponent<RectTransform>();
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.anchoredPosition = anchoredPos;
        rect.sizeDelta = size;

        var img = btnObj.AddComponent<Image>();
        img.color = color;

        btnObj.AddComponent<Button>();

        var textObj = new GameObject("Text");
        textObj.transform.SetParent(btnObj.transform, false);
        var textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        var t = textObj.AddComponent<Text>();
        t.text = texto;
        t.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        t.fontSize = 24;
        t.color = Color.white;
        t.alignment = TextAnchor.MiddleCenter;

        return btnObj;
    }

    private void MostrarPantallaFin()
    {
        panelFin.SetActive(true);
        botonSalir.gameObject.SetActive(false);
        Time.timeScale = 0f;
    }

    private void OcultarPantallaFin()
    {
        panelFin.SetActive(false);
        botonSalir.gameObject.SetActive(true);
        Time.timeScale = 1f;
    }

    private void SalirDefinitivo()
    {
        Time.timeScale = 1f;

        // Desconectar de la red si hay NetworkManager activo
        if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsListening)
        {
            NetworkManager.Singleton.Shutdown();
        }

        if (!string.IsNullOrWhiteSpace(nombreEscenaInicio))
        {
            SceneManager.LoadScene(nombreEscenaInicio);
        }
        else
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
