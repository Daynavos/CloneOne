using System;
using UnityEngine;

public class LeftClickState : MonoBehaviour
{
    public enum leftClickState
    {
        bullet,
        beams,
        none
    }
    
    public leftClickState currentLCstate = leftClickState.none;

    void Start()
    {
        currentLCstate = leftClickState.none;
        Debug.Log(currentLCstate);
    }
    public void bulletsTime()
    {
        currentLCstate = leftClickState.bullet;
        Debug.Log(currentLCstate);
    }
    
    public void beamsTime()
    {
        currentLCstate = leftClickState.beams;
        Debug.Log(currentLCstate);
    }

    public void noneTime()
    {
        currentLCstate = leftClickState.none;
        Debug.Log(currentLCstate);
    }
}
