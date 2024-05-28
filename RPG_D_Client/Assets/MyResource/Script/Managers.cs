using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    public static Managers Instance;

    public UIManager ui;
    public InputManager input;

    private void Awake()
    {
        Instance = this;
    }
}
