using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    public static Managers Instance;

    public UIManager ui;
    public InputManager input;
    public ObjectManager obj;

    private void Awake()
    {
        Instance = this;
    }
}
