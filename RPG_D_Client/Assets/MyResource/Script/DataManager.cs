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

    public int GetEquipLevel(int equipType)
    {
        if (equipType == 0)
            return myUserData.weaponLevel;
        else if (equipType == 1)
            return myUserData.armorLevel;
        else if (equipType == 2)
            return myUserData.shoesLevel;

        return 0;
    }
}
