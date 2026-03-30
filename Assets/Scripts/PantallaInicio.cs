using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PantallaInicio : MonoBehaviour
{
    [SerializeField] private string nombreEscenaJuego = "SampleScene";

    private void Start()
    {
        CrearUI();
    }

    private void CrearUI()
    {
        // --- EventSystem ---
        if (FindAnyObjectByType<EventSystem>() == null)
        {
            var esObj = new GameObject("EventSystem");
            esObj.AddComponent<EventSystem>();
            // Soporte para ambos sistemas de input
            esObj.AddComponent<StandaloneInputModule>();
        }

        // --- Canvas ---
        var canvasObj = new GameObject("CanvasInicio");
        var canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;
        var scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);
        canvasObj.AddComponent<GraphicRaycaster>();

        // --- Fondo (sin raycast para no bloquear clicks) ---
        var fondoObj = new GameObject("Fondo");
        fondoObj.transform.SetParent(canvasObj.transform, false);
        var fondoRect = fondoObj.AddComponent<RectTransform>();
        fondoRect.anchorMin = Vector2.zero;
        fondoRect.anchorMax = Vector2.one;
        fondoRect.offsetMin = Vector2.zero;
        fondoRect.offsetMax = Vector2.zero;
        var fondoImg = fondoObj.AddComponent<Image>();
        fondoImg.color = new Color(0.12f, 0.12f, 0.18f, 1f);
        fondoImg.raycastTarget = false;

        // --- Titulo ---
        CrearTexto(canvasObj.transform, "Titulo", "DSH",
            new Vector2(0.5f, 0.75f), new Vector2(600f, 100f),
            60, Color.white, FontStyle.Bold);

        // --- Boton Iniciar Partida ---
        var btnIniciar = CrearBoton(canvasObj.transform, "BotonIniciar", "Iniciar Partida",
            new Vector2(0.5f, 0.50f), new Vector2(300f, 60f),
            new Color(0.2f, 0.6f, 0.3f, 1f));
        btnIniciar.GetComponent<Button>().onClick.AddListener(IniciarPartida);

        // --- Boton Salir ---
        var btnSalir = CrearBoton(canvasObj.transform, "BotonSalir", "Salir",
            new Vector2(0.5f, 0.38f), new Vector2(300f, 60f),
            new Color(0.8f, 0.2f, 0.2f, 1f));
        btnSalir.GetComponent<Button>().onClick.AddListener(SalirDelJuego);

        // --- Creditos ---
        CrearTexto(canvasObj.transform, "Creditos",
            "Desarrollado por: Daniel Castillo, Yedra García y Daniel Colodro",
            new Vector2(0.5f, 0.12f), new Vector2(800f, 50f),
            22, new Color(0.8f, 0.8f, 0.8f, 1f), FontStyle.Italic);
    }

    private void CrearTexto(Transform parent, string nombre, string contenido,
        Vector2 anchorPos, Vector2 size, int fontSize, Color color, FontStyle style = FontStyle.Normal)
    {
        var obj = new GameObject(nombre);
        obj.transform.SetParent(parent, false);
        var rect = obj.AddComponent<RectTransform>();
        rect.anchorMin = anchorPos;
        rect.anchorMax = anchorPos;
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = size;
        var text = obj.AddComponent<Text>();
        text.text = contenido;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = fontSize;
        text.fontStyle = style;
        text.color = color;
        text.alignment = TextAnchor.MiddleCenter;
        text.raycastTarget = false;
    }

    private GameObject CrearBoton(Transform parent, string nombre, string texto,
        Vector2 anchorPos, Vector2 size, Color color)
    {
        var btnObj = new GameObject(nombre);
        btnObj.transform.SetParent(parent, false);

        var rect = btnObj.AddComponent<RectTransform>();
        rect.anchorMin = anchorPos;
        rect.anchorMax = anchorPos;
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = size;

        var img = btnObj.AddComponent<Image>();
        img.color = color;
        img.raycastTarget = true;

        var btn = btnObj.AddComponent<Button>();
        btn.targetGraphic = img;
        btn.interactable = true;

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
        t.fontSize = 26;
        t.color = Color.white;
        t.alignment = TextAnchor.MiddleCenter;
        t.raycastTarget = false;

        return btnObj;
    }

    private void IniciarPartida()
    {
        Debug.Log("[PantallaInicio] Boton Iniciar pulsado. Cargando: " + nombreEscenaJuego);
        try
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(nombreEscenaJuego);
        }
        catch (Exception e)
        {
            Debug.LogError("[PantallaInicio] Error al cargar escena: " + e.Message);
        }
    }

    private void SalirDelJuego()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
