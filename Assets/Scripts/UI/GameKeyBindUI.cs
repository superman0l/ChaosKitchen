using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameKeyBindUI : BaseUI
{
    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button grabPutButton;
    [SerializeField] private Button cutButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button saveButton;
    [SerializeField] private TextMeshProUGUI moveUPText;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI grabPutText;
    [SerializeField] private TextMeshProUGUI cutText;
    [SerializeField] private TextMeshProUGUI pauseText;
    [SerializeField] private BaseUI RebindUI;

    private void UpdateVisual()
    {
        moveUPText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        moveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        moveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        moveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        grabPutText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        cutText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlternate);
        pauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
    }

    private void Awake()
    {
        moveUpButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Up); });
        moveDownButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Down); });
        moveLeftButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Left); });
        moveRightButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Right); });
        grabPutButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Interact); });
        cutButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.InteractAlternate); });
        pauseButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Pause); });
        saveButton.onClick.AddListener(() => { Hide(); });
        Hide();
    }

    private void RebindBinding(GameInput.Binding binding)
    {
        RebindUI.Show();
        GameInput.Instance.RebindBinding(binding, () =>
        {
            RebindUI.Hide();
            UpdateVisual();
        });
    }

}
