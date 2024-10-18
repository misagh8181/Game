using UnityEngine;
using UnityEngine.UI;

public class UIEnemy : MonoBehaviour
{
    private Canvas _canvas;
    public Slider Slider { get; private set; }

    private void Awake()
    {
        _canvas = GetComponentInChildren<Canvas>();
        _canvas.worldCamera = Camera.main;

        Slider = _canvas.GetComponentInChildren<Slider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Slider.minValue = 0;
        UpdateSliderColor();
    }

    public void UpdateSliderColor()
    {
        var healthPercent = Mathf.InverseLerp(Slider.minValue, Slider.maxValue, Slider.value);
        var newColor = Color.Lerp(Color.red, Color.green, healthPercent);
        Slider.GetComponentInChildren<Image>().color = newColor;
    }

    public void StartHealth(int maxHealth)
    {
        Slider.value = Slider.maxValue = maxHealth;
        UpdateSliderColor();
    }

    public void SetHealth(int health)
    {
        Slider.value = health;
        UpdateSliderColor();
    }
}