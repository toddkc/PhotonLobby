using System.Linq;
using UnityEditor;
using UnityEngine;

public class SingleGameEvent : EditorWindow
{
    private ScriptableObjectArchitecture.GameEvent selectedEvent;
    private string listenerNames = "";

    [MenuItem("Window/Game Event Debug")]
    public static void Init()
    {
        SingleGameEvent window = (SingleGameEvent)GetWindow(typeof(SingleGameEvent));
        window.Show();
    }

    [MenuItem("Window/Game Event Debug")]
    public static void ShowWindow()
    {
        GetWindow(typeof(SingleGameEvent));
    }

    private void OnGUI()
    {
        GUILayout.Label("GameEvent Listeners", EditorStyles.boldLabel);
        if (selectedEvent == null)
        {
            GUILayout.Label("no event selected...", EditorStyles.boldLabel);
            return;
        }
        GUILayout.Label(selectedEvent.name, EditorStyles.boldLabel);

        if (EditorApplication.isPlaying)
        {
            GUILayout.Label("Current Listeners:", EditorStyles.boldLabel);
            foreach (var listener in selectedEvent.GetListeners())
            {
                GUILayout.Label(listener.ToString());
            }
        }
        else
        {
            GUILayout.Label("Prefab Listeners:", EditorStyles.boldLabel);
            GUILayout.Label(listenerNames.ToString());
        }
    }

    private void OnSelectionChange()
    {
        selectedEvent = Selection.objects.OfType<ScriptableObjectArchitecture.GameEvent>().FirstOrDefault();
        SearchPrefabs();
        Repaint();
    }

    private void SearchPrefabs()
    {
        listenerNames = "";
        if (selectedEvent == null || EditorApplication.isPlaying) return;

        string[] _pathsToAssets = AssetDatabase.FindAssets("t:GameObject");

        foreach (var path in _pathsToAssets)
        {
            var _assetPath = AssetDatabase.GUIDToAssetPath(path);
            var _go = AssetDatabase.LoadAssetAtPath<GameObject>(_assetPath);
            if (_go.GetComponents<ScriptableObjectArchitecture.GameEventListener>().Any(evtL => evtL.GetEvent() == selectedEvent))
            {
                listenerNames += $"{_go.name} \n";
            }
            if (_go.GetComponentsInChildren<ScriptableObjectArchitecture.GameEventListener>().Any(evtL => evtL.GetEvent() == selectedEvent))
            {
                listenerNames += $"{_go.name} \n";
            }
        }
    }
}