using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LanguageToggle : MonoBehaviour
{
    private List<Languages> languages;
    private int index;

    private void Awake()
    {
        languages = ((Languages[]) Enum.GetValues(typeof( Languages))).ToList();
        languages = languages.OrderBy(value => value).ToList();
        languages.Remove(Languages.Uninitialized);
    }

    private void OnEnable()
    {
        UpdateIndex(LanguageSetting.CurrentLanguage);
        LanguageSetting.OnLanguageChanged += UpdateIndex;
    }

    private void OnDisable()
    {
        LanguageSetting.OnLanguageChanged -= UpdateIndex;
    }

    public void NextLanguage()
    {
        index = (index + 1) % languages.Count;

        LanguageSetting.ChangeLanguageSetting(languages[index]);
    }

    private void UpdateIndex(Languages currentLanguage)
    {
        index = 0;

        for (int i = 0; i < languages.Count; i++)
        {
            if ((int)currentLanguage == i)
            {
                index = i;
                break;
            }
        }
    }
}
