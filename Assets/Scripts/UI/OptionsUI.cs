using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : BaseUI
{
    public static OptionsUI Instance { get; private set; }

    [SerializeField] private Button soundButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button keyBindButton;
    [SerializeField] private TextMeshProUGUI soundText;
    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private BaseUI keyBindUI;

    private void Awake()
    {
        Instance = this;

        soundButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        musicButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        closeButton.onClick.AddListener(() =>
        {
            Hide();
        });
        keyBindButton.onClick.AddListener(() =>
        {
            keyBindUI.Show();
        });
        Hide();
    }

    private void Start()
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        soundText.text = "Sound: " + Mathf.Floor(SoundManager.Instance.GetVolume() * 10f);
        musicText.text = "Music: " + Mathf.Floor(MusicManager.Instance.GetVolume() * 10f);
    }

}
