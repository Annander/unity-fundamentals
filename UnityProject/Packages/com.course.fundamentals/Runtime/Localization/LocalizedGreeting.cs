using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Course.Fundamentals.Localization
{
    [AddComponentMenu("Course/Localization/Localized Greeting")]
    public sealed class LocalizedGreeting : MonoBehaviour
    {
        [SerializeField] private string tableName = "UI";
        [SerializeField] private string entryKey = "greeting";

        [SerializeField] private LocalizedString greeting = new ("UI", "greeting");

        public string CurrentText { get; private set; } = string.Empty;

        private void OnEnable()
        {
            // Fires once the string is loaded, and again whenever the locale changes.
            greeting.StringChanged += OnGreetingChanged;
        }

        private void OnDisable()
        {
            greeting.StringChanged -= OnGreetingChanged;
        }

        private IEnumerator Start()
        {
            // Option A: a one-off lookup. Everything is asynchronous (tables may be in Addressables bundles), so wait for initialization and then for the lookup.
            yield return LocalizationSettings.InitializationOperation;

            var lookup = LocalizationSettings.StringDatabase.GetLocalizedStringAsync(tableName, entryKey);
            yield return lookup;

            if (lookup.Status == AsyncOperationStatus.Succeeded)
                Debug.Log($"[Option A] {tableName}/{entryKey} = \"{lookup.Result}\"", this);
            else
                Debug.LogWarning($"[Option A] lookup failed for {tableName}/{entryKey}", this);
        }

        private void OnGreetingChanged(string value)
        {
            CurrentText = value; // feed this to your UI (TextMeshPro, UI Toolkit Label, ...)
            Debug.Log($"[Option B] greeting = \"{value}\"", this);
        }
    }
}