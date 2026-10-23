using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Course.Demos
{
    [AddComponentMenu("Course/Demos/Scene Load Demo")]
    public sealed class SceneLoadDemo : MonoBehaviour
    {
        [Tooltip("Content scene for L (LoadSceneAsync, Additive) and U (UnloadSceneAsync). Must be in the scene list.")]
        [SerializeField] private string additiveScene = "1.1_Architecture";

        [Tooltip("Mark this GameObject DontDestroyOnLoad, so it survives single-mode scene loads. Only works on root objects.")]
        [SerializeField] private bool persistAcrossScenes = true;

        [Tooltip("Boot behaviour: on Start, load 'Start Scene' in Single mode (replaces the boot scene).")]
        [SerializeField] private bool loadOnStart;

        [Tooltip("The scene loaded on Start when 'Load On Start' is ticked.")]
        [SerializeField] private string startScene = "1.1_Architecture";

        private void Awake()
        {
            if (persistAcrossScenes) DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            if (loadOnStart && !string.IsNullOrEmpty(startScene))
            {
                Debug.Log($"Boot: loading '{startScene}' (single).", this);
                SceneManager.LoadSceneAsync(startScene, LoadSceneMode.Single);
            }
        }

        private void Update()
        {
            var keyboard = Keyboard.current;
            if (keyboard == null) return;

            var scene = SceneManager.GetSceneByName(additiveScene);

            if (keyboard.lKey.wasPressedThisFrame)
            {
                if (scene.isLoaded) { Debug.Log($"'{additiveScene}' is already loaded.", this); return; }
                Debug.Log($"Loading '{additiveScene}' additively.", this);
                SceneManager.LoadSceneAsync(additiveScene, LoadSceneMode.Additive);
            }
            else if (keyboard.uKey.wasPressedThisFrame)
            {
                if (!scene.isLoaded) { Debug.Log($"'{additiveScene}' is not loaded.", this); return; }
                Debug.Log($"Unloading '{additiveScene}'.", this);
                SceneManager.UnloadSceneAsync(scene);
            }
        }
    }
}