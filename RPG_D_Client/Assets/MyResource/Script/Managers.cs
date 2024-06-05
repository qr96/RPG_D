using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    public static Managers Instance;

    public UIManager _ui;
    public InputManager _input;
    public ObjectManager _obj;
    public MapManager _map;
    public DataManager _data;

    public static UIManager ui { get { return Instance._ui; } }
    public static InputManager input { get { return Instance._input; } }
    public static ObjectManager obj { get { return Instance._obj; } }
    public static MapManager map { get { return Instance._map; } }
    public static DataManager data { get { return Instance._data; } }

    private void Awake()
    {
        Instance = this;
    }
}
