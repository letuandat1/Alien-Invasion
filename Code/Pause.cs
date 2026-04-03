using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject pausepanel;

    // Update is called once per frame
    void Update()
    {

    }
    public void pause()
    {
        Time.timeScale = 0f; // Stop the game time
        pausepanel.SetActive(true); // Show the pause panel
    }
    public void Play()
    {
        Time.timeScale = 1f; // Resume the game time
        pausepanel.SetActive(false); // Hide the pause panel
    }
    public void ShowUIpanel()
    {
        pausepanel.SetActive(true); // Show the pause panel
    }
    public void HideUIpanel()
    {
        pausepanel.SetActive(false); // Hide the pause panel
    }
}
