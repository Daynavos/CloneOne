using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameStateMan : MonoBehaviour
{
    public enum MapState
    {
        Cube,
        Yoyo,
        Bear
    }
    
    public bool CUBEobjCollected = false;
    public bool YOYOobjCollected = false;
    public bool BEARobjCollected = false;
    
    public Button cubeButton;
    public Button yoyoButton;
    public Button bearButton;

    public GameObject cubePlanet;
    public GameObject yoyoPlanet;
    public GameObject bearPlanet;
    
    public MapState currentMAPstate = MapState.Cube;

    public GameObject map;
    void Start()
    {
        currentMAPstate = MapState.Cube;
        updateMap();
    }

    public void goToMap()
    {
        map.SetActive(true);
        cubePlanet.SetActive(false);
        yoyoPlanet.SetActive(false);
        bearPlanet.SetActive(false);
        updateMap();
        Time.timeScale = 0;
    }

    public void goToCube()
    {
        map.SetActive(false);
        Time.timeScale = 1;
        cubePlanet.SetActive(true);
        currentMAPstate = MapState.Yoyo;
    }
    
    public void goToYoyo()
    {
        map.SetActive(false);
        Time.timeScale = 1;
        yoyoPlanet.SetActive(true);
        currentMAPstate = MapState.Bear;
    }
    
    public void goToBear()
    {
        map.SetActive(false);
        Time.timeScale = 1;
        bearPlanet.SetActive(true);
    }

    void updateMap()
    {
        switch (currentMAPstate)
        {
            case MapState.Cube:
                cubeButton.interactable = true;
                yoyoButton.interactable = false;
                bearButton.interactable = false;
                break;
            case MapState.Yoyo:
                yoyoButton.interactable = true;
                bearButton.interactable = false;
                cubeButton.interactable = false;
                break;
            case MapState.Bear:
                bearButton.interactable = true;
                yoyoButton.interactable = false;
                cubeButton.interactable = false;
                break;
        }
    }
}
