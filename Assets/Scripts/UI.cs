using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    public TMP_Text speed, boidSpeed;

    public Slider speedSlider;

    public void ChangeSpeed()
    {
        boidSpeed.text = speedSlider.value.ToString();
        Boid.maxSpeed = speedSlider.value;
    }
    public void Pause()
    {
        Time.timeScale = 0;
        speed.text = Time.timeScale.ToString();
    }

    public void Unpause()
    {
        Time.timeScale = 1;
        speed.text = Time.timeScale.ToString();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void SpeedUp() {
        if(Time.timeScale < 10)
        {
            Time.timeScale++;
            speed.text = Time.timeScale.ToString();
        }
    }

    public void SpeedDown()
    {
        if (Time.timeScale > 1)
        {
            Time.timeScale--;
            speed.text = Time.timeScale.ToString();
        }
    }
}
