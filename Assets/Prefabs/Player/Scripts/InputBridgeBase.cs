﻿using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// base class for the input bridge, inherit to create specific input for kbm or oculus
/// </summary>
public class InputBridgeBase : MonoBehaviour
{
    public static InputBridgeBase instance;

    protected bool canPlayerMove = true;

    public float MoveAxis { get; protected set; }
    public float StrafeAxis { get; protected set; }
    public float PitchAxis { get; protected set; }
    public float YawAxis { get; protected set; }
    public bool Interact { get; protected set; }
    public bool Climb { get; protected set; }
    public float SwimAxis { get; protected set; }
    public bool Jump { get; protected set; }
    public bool Menu { get; protected set; }
    public bool Score { get; protected set; }
    public bool ToggleVoice { get; set; }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += CheckScene;
    }
    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= CheckScene;
    }

    private void CheckScene(Scene oldScene, Scene newScene)
    {
        if (newScene.buildIndex < 2)
        {
            ToggleMovement(true);
        }
    }

    public static void ToggleMovement(bool value)
    {
        instance.canPlayerMove = value;
    }

    private void Start()
    {
        MoveAxis = 0;
        StrafeAxis = 0;
        PitchAxis = 0;
        YawAxis = 0;
        SwimAxis = 0;
        Interact = false;
        Jump = false;
        Climb = false;
        Menu = false;
        Score = false;
        ToggleVoice = false;
    }
}
