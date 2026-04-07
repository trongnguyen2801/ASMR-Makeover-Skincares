using TMPro;
using UnityEditor;
using UnityEngine;

public class LocalizationTextMeshPro : MonoBehaviour
{
    public string key;

    public bool autoDisplayOnAwake = true;

#if UNITY_EDITOR
    public LanguageType previewLanguage = LanguageType.ENGLISH;
#endif
    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();

        if (autoDisplayOnAwake)
        {
            CreateValue();
        }

        CustomLocalization.UpdateLanguageEvent += OnUpdateLanguage;
    }

    private void OnDestroy()
    {
        CustomLocalization.UpdateLanguageEvent -= OnUpdateLanguage;
    }

#if UNITY_EDITOR
    public void CreateKey()
    {
        key = CustomLocalization.GetKey(GetText(), out previewLanguage);
    }
#endif

    public void CreateValue()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            SetText(CustomLocalization.Get(key, previewLanguage));
            // SetFont(CustomLocalization.GetFont(previewLanguage));
        }
        else
        {
            SetText(CustomLocalization.Get(key));
            // SetFont(CustomLocalization.GetFont());
        }
#else
        string value = CustomLocalization.Get(key);
        if (value == null) value = key;
            SetText(value);

        SetFont(CustomLocalization.GetFont());
#endif
    }

    public void OnUpdateLanguage(LanguageType language)
    {
        CreateValue();
    }

    public void ApplyTextWithKey(string key)
    {
        if (text == null)
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        this.key = key;
        CreateValue();
    }


    public void ApplyTextColorWithKey(string key, string colorHex)
    {
        if (text == null)
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        this.key = key;

        string value = CustomLocalization.Get(key);

        if (value != null)
        {
            value = value.Replace("(", "<color=#" + colorHex + ">");
            if (value != null)
                text.text = value.Replace(")", "</color>");
        }
    }

    public void SetText(string value)
    {
        // Debug.Log("String: " + value);
#if UNITY_EDITOR
        if (!string.IsNullOrEmpty(value))
        {
            if (!Application.isPlaying)
            {
                GetComponent<TextMeshProUGUI>().text = value;
            }
            else
            {
                GetComponent<TextMeshProUGUI>().text = value;
            }
        }
#else
        if (!string.IsNullOrEmpty(value))
            text.text = value;
#endif
    }

    public void SetFont(Font font)
    {
#if UNITY_EDITOR
        /*if (!Application.isPlaying)
        {
            GetComponent<TextMeshProUGUI>().font = font;
        }
        else
        {
            GetComponent<TextMeshProUGUI>().font = font;
        }*/
#else
    // text.font = font;
#endif
    }

    private string GetText()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            return GetComponent<TextMeshProUGUI>().text;
        }
        else
        {
            return text.text;
        }
#else
    return text.text;
#endif
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(LocalizationTextMeshPro))]
public class LocalizationTextMeshProEditor : Editor
{
    public LocalizationTextMeshPro textController;

    public void OnEnable()
    {
        textController = target as LocalizationTextMeshPro;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(25f);

        if (GUILayout.Button("Key -> Value"))
        {
            textController.CreateValue();
        }

        if (GUILayout.Button("Value -> Key"))
        {
            textController.CreateKey();
        }
    }
}
#endif