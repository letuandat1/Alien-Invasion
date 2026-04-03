using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void Menu(string Menu)
    {
        SceneManager.LoadScene("Menu"); // tên scene option
    }
    public void option(string option)
    {
        SceneManager.LoadScene("Options"); // tên scene option
    }
    public void scene1(string scene1)
    {
        SceneManager.LoadScene("DinhDocLap"); // tên scene option
    }
}