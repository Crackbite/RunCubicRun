using Agava.YandexGames;
using System.Collections.Generic;
using UnityEngine;

public class LanguageSwitch : MonoBehaviour
{
    [SerializeField] private SDK _sdk;

    public IReadOnlyDictionary<string, string> _languages = new Dictionary<string, string>()
    {
        {"en","English" },
        {"ru","Russian" },
        {"tr","Turkish" }
    };

    private void OnEnable()
    {
        _sdk.Initialized += OnSDKInizialixed;
    }

    private void OnDisable()
    {
        _sdk.Initialized -= OnSDKInizialixed;
    }

    private void OnSDKInizialixed()
    {
        Switch();
    }

    private void Switch()
    {
        string currentLanguage = YandexGamesSdk.Environment.i18n.lang;

        foreach (KeyValuePair<string, string> language in _languages)
        {
            if (language.Key == currentLanguage)
            {
                Lean.Localization.LeanLocalization.SetCurrentLanguageAll(language.Value);
            }
        }
    }
}
