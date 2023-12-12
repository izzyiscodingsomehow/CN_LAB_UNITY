using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Realtime;
public class RoomBehaviors : MonoBehaviour   
{
    public TMP_Text roomNameText;

    private RoomInfo info;

    public void SetButtonDetails(RoomInfo inputInfo)
    {
        info = inputInfo;
        roomNameText.text = inputInfo.Name;
    }
}