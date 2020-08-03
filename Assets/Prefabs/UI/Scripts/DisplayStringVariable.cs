using ScriptableObjectArchitecture;
using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DisplayStringVariable : MonoBehaviour
{
    [SerializeField] private StringReference variable = default;
    private Text text;
    private event Action messageChanged;

    private void Awake()
    {
        text = GetComponent<Text>();
        messageChanged = UpdateText;
    }

    private void OnEnable()
    {
        variable.AddListener(messageChanged);
    }

    private void OnDisable()
    {
        variable.RemoveListener(messageChanged);
    }

    private void UpdateText()
    {
        if (text != null)
        {
            text.text = variable.Value;
        }
    }

}