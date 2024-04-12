using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//for image

public class ProgressBar : BaseUI
{
    [SerializeField] private GameObject hasProgressGameObject;
    [SerializeField] private Image barImage;

    private IHasProgress hasProgress;

    private void Start()
    {
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        if(hasProgress == null)
        {
            Debug.LogError("This GameObject doesnt have IHasProgress!");
            return;
        }
        hasProgress.OnProgressChanged += UpdateBar;
        Hide();
        barImage.fillAmount = 0;
    }

    private void UpdateBar(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        if (e.progressNormalized > 0 && e.progressNormalized < 1)
            Show();
        else Hide();
        barImage.fillAmount = e.progressNormalized;
    }

    
}
