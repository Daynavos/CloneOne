using UnityEngine;
using UnityEngine.SceneManagement;

public class beam : MonoBehaviour
{

    public GameObject gameManObj;
    private GameStateMan gameManScript;

    public GameObject flightObj;
    private FlightController flightScript;
    

    void Start()
    {
        gameManScript = gameManObj.GetComponent<GameStateMan>();
        flightScript =  flightObj.GetComponent<FlightController>();
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            SceneManager.LoadScene("Dayna");
        }
        if (collision.gameObject.CompareTag("Bear") && flightScript.beaming)
        {
            Destroy(collision.gameObject);
            gameManScript.goToMap();
        }
        
    }
}
