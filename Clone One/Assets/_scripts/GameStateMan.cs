using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameStateMan : MonoBehaviour
{
    public enum MapState
    {
        Cube,
        Yoyo,
        Bear,
        done
    }
    
    
    public Button cubeButton;
    public Button yoyoButton;
    public Button bearButton;

    public GameObject cubePlanet;
    public GameObject yoyoPlanet;
    public GameObject bearPlanet;
    
    public MapState currentMAPstate = MapState.Cube;

    public GameObject start;
    public GameObject map;
    public GameObject end;
    void Start()
    {
        currentMAPstate = MapState.Cube;
        updateMap();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowEndScreen();
        }
    }

    public void startButton()
    {
        start.SetActive(false);
        map.SetActive(true);
    }

    public void endGame()
    {
        Application.Quit();
    }

    void ShowEndScreen()
    {
        if (!end.active)
        {
            end.SetActive(true);
        }
        else
        {
            end.SetActive(false);
        }
        
    }

    public void goToMap()
    {
        if (currentMAPstate == MapState.done)
        {
            ShowEndScreen();
        }
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
        currentMAPstate = MapState.done;
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
