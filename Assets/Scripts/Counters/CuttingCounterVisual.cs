using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private CuttingCounter cuttingCounter;
    private const string cut = "Cut";

    private void Awake()
    {
        animator = GetComponent<Animator>();
        cuttingCounter.OnProgressChanged += CuttingAnim;
    }

    private void CuttingAnim(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        if (e.progressNormalized > 0 && e.progressNormalized < 1)
            animator.SetTrigger(cut);
    }
}
