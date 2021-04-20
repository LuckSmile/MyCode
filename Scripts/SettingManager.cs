using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    public bool Sound => sound;
    private bool sound = false;
    public void SetSound()
    {
        sound = !sound;
    }
}
