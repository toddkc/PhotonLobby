using System.Linq;
using UnityEditor;
using UnityEngine;

public class AllGameEvents : EditorWindow
{
    private string displayString = "";
    private Vector2 scrollPosition = Vector2.zero;

    [MenuItem("Window/GameEvents and Listeners")]
    public static void Init()
    {
        AllGameEvents window = (AllGameEvents)GetWindow(typeof(AllGameEvents));
        window.Show();
    }

    [MenuItem("Window/GameEvents and Listeners")]
    public static void ShowWindow()
    {
        GetWindow(typeof(AllGameEvents));
    }

    private void OnFocus()
    {
        SearchPrefabs();
    }

    private void OnGUI()
    {
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        GUILayout.Label("GameEvents and Listeners \n", EditorStyles.boldLabel);
        GUILayout.Label(displayString);
        GUILayout.EndScrollView();
    }

    private void SearchPrefabs()
    {
        displayString = "";
        string[] _pathsToEvents = AssetDatabase.FindAssets("t:ScriptableObjectArchitecture.GameEvent");
        string[] _pathsToAssets = AssetDatabase.FindAssets("t:GameObject");
        foreach (var path in _pathsToEvents)
        {
            var _eventPath = AssetDatabase.GUIDToAssetPath(path);
            var _event = AssetDatabase.LoadAssetAtPath<ScriptableObjectArchitecture.GameEvent>(_eventPath);
            displayString += $"{_event.name}: \n";
            foreach (var apath in _pathsToAssets)
            {
                var _assetPath = AssetDatabase.GUIDToAssetPath(apath);
                var _go = AssetDatabase.LoadAssetAtPath<GameObject>(_assetPath);
                //if (_go.GetComponents<ScriptableObjectArchitecture.GameEventListener>().Any(evtL => evtL.GetEvent() == _event))
                //{
                //    displayString += $"    -{_go.name} \n";
                //}
                if (_go.GetComponentsInChildren<ScriptableObjectArchitecture.GameEventListener>().Any(evtL => evtL.GetEvent() == _event))
                {
                    displayString += $"    -{_go.name} \n";
                }
            }
        }
        Repaint();
    }
}