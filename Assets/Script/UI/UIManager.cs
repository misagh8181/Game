using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject settingUIElements;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnStartClicked()
    {
        var i = PlayerPrefs.GetInt("level", 0);
        if (i >= 1)
        {
            SceneManager.LoadScene(2);
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }

    public void OnSettingsClicked()
    {
        settingUIElements.SetActive(true);
    }

    public void OnResumeInSettingsClicked()
    {
        settingUIElements.SetActive(false);
    }

    public void OnExitClicked()
    {
        Application.Quit(0);
    }
}