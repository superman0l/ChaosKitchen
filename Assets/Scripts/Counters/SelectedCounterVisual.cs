using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject visualGameObject;

    private void Start()
    {
        if (!Player.Instance) return;
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if (e.selectedCounter == baseCounter)
        {
            show();
        }
        else
        {
            hide();
        }
    }

    public void show()
    {
        visualGameObject.SetActive(true);
    }

    public void hide()
    {
        visualGameObject.SetActive(false);
    }
}
