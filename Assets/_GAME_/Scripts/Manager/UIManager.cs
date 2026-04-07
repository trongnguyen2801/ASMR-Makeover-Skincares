using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public ScreenUI[] screens;
    private GameManager _gameManager;
    public Canvas canvas;
    public Camera camUI;
    public GameObject transition;

    private ScreenUI _activeScreen;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance.gameObject);
        }
        else if (Instance.GetInstanceID() != GetInstanceID())
        {
            Destroy(gameObject);
        }
    }

    public void Initialize(GameManager manager)
    {
        _gameManager = manager;
        foreach (var t in screens)
        {
            t.Initialize(this);
        }
        ResUi();
    }

    public void DeActiveAllScreen(bool showTopUI)
    {
        foreach (var t in screens)
        {
            t.DeActive();
        }
    }

    public T ActiveScreen<T>(bool showTopUI = true) where T : ScreenUI
    {
        T screen = default;
        foreach (var t in screens)
        {
            if (t is T)
            {
                t.Active();
                screen = t.GetComponent<T>();
                _activeScreen = screen;
            }
            else if (t.isShowing)
            {
                t.DeActive();
            }
        }

        return screen;
    }

    public T GetScreen<T>()
    {
        foreach (var t in screens)
        {
            if (t is T)
            {
                return t.GetComponent<T>();
            }
        }

        return default;
    }

    private void ResUi()
    {
        float scrWidth = Screen.width;
        float scrHeight = Screen.height;
        float tile = scrHeight / scrWidth;
        Debug.Log("width=" + scrWidth + " Height= " + scrHeight);
        if (tile >= 1920f / 1080f)
        {
            canvas.GetComponent<CanvasScaler>().matchWidthOrHeight = 0;
        }
        else
        {
            canvas.GetComponent<CanvasScaler>().matchWidthOrHeight = 1f;
        }
    }

    public void ResetTransition()
    {
        transition.SetActive(false);
    }

    public ScreenUI ActiveScreenUI()
    {
        return _activeScreen;
    }
}