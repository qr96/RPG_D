using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    UserData myUserData;

    public void SetMyUserData(UserData userData)
    {
        myUserData = userData;
    }

    public UserData GetMyUserData()
    {
        return myUserData;
    }
}
