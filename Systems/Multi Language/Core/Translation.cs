using UnityEngine;

[System.Serializable]
public struct Translation<T>
{
    [SerializeField] public Languages Language;
    [TextArea]
    [SerializeField] public T TranslationContent;

    public Translation(Languages language, T translationContent)
    {
        this.Language = language;
        this.TranslationContent = translationContent;
    }
}