using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Course.Fundamentals.Localization
{
    [AddComponentMenu("Course/Localization/Locale Switcher")]
    public sealed class LocaleSwitcher : MonoBehaviour
    {
        private bool _busy;

        public void SelectLocale(string code)
        {
            if (!_busy) StartCoroutine(SelectRoutine(code));
        }

        [ContextMenu("Next Locale")] // right-click the component header in Play Mode
        public void NextLocale()
        {
            if (!_busy) StartCoroutine(NextRoutine());
        }

        private IEnumerator SelectRoutine(string code)
        {
            _busy = true;
            yield return LocalizationSettings.InitializationOperation;

            // string -> LocaleIdentifier converts implicitly.
            Locale locale = LocalizationSettings.AvailableLocales.GetLocale(code);
            if (locale != null) LocalizationSettings.SelectedLocale = locale;
            else Debug.LogWarning($"Locale '{code}' is not in the project's available locales.", this);
            _busy = false;
        }

        private IEnumerator NextRoutine()
        {
            _busy = true;
            yield return LocalizationSettings.InitializationOperation;

            var locales = LocalizationSettings.AvailableLocales.Locales;

            if (locales.Count > 0)
            {
                var current = locales.IndexOf(LocalizationSettings.SelectedLocale);
                LocalizationSettings.SelectedLocale = locales[(current + 1) % locales.Count];
                Debug.Log($"Locale is now {LocalizationSettings.SelectedLocale.Identifier.Code}", this);
            }

            _busy = false;
        }
    }
}