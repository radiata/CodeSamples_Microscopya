using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Translatable Text Block", menuName = "Custom Menus/Multi Language/Translatables/Translatable Text")]
public class TranslatableText_SO : ScriptableObject
{
    [SerializeField] private string translationID;

    [SerializeField] private List<Translation<string>> translations = new List<Translation<string>>();

    public string GetTranslation(Languages language)
    {
        var x = translations[(int)language];
        if (x.Language == language)
        {
            return x.TranslationContent;
        }

        for (int i = 0; i < translations.Count; i++)
        {
            if (translations[i].Language == language)
            {
                return x.TranslationContent;
            }
        }

        DebugWrapper.Log("Translation Content not found.", null);
        return translationID;
    }

    private void Reset()
    {
        translations = new List<Translation<string>>();

        foreach (int index in Enum.GetValues(typeof(Languages)))
        {
            Translation<string> newTranslation = new Translation<string>((Languages)index, string.Empty);

            if (newTranslation.Language != Languages.Uninitialized)
            {
                translations.Add(newTranslation);
            }
        }
    }

    private void OnValidate()
    {
        translations = translations.OrderBy(textTranslation => textTranslation.Language).ToList();

        //ValidateTranslationEntries();
    }

    private void ValidateTranslationEntries()
    {
        Array languageList = Enum.GetValues(typeof(Languages));
        int[] languageOccurrences = new int[languageList.Length];

        for(int i = 0; i < languageOccurrences.Length; i++)
        {
            languageOccurrences[i] = 0;
        }

        for (int i = 0; i < translations.Count; i++)
        {
            //add one for each language occurrence
        }

        //if language occurrence for any language is > 1
        DebugWrapper.LogWarning($"{translationID} - contains multiple definitions for a language.\nThere should be only one entry per language.", null);

        //if language occurrence for any language is == 0
        DebugWrapper.LogWarning($"{translationID} - contains no definition for a language.\nThere should be one entry per language.", null);
    }
}