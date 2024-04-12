using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private PlatesCounter platesCounter;
    [SerializeField] private Transform counterTop;
    [SerializeField] private Transform plateVisualPrefab;

    private List<GameObject> plateVisualGameObjectsList;

    private void Awake()
    {
        plateVisualGameObjectsList = new List<GameObject>();
    }

    private void Start()
    {
        if (platesCounter == null) return;
        platesCounter.PlateSpawn += SpawnPlate;
        platesCounter.PlateRemove += DeletePlate;
    }

    private void SpawnPlate(object sender, EventArgs e)
    {
        Transform plateVisualTransform = Instantiate(plateVisualPrefab, counterTop);

        float plateOffsetY = 0.1f;
        plateVisualTransform.localPosition = Vector3.up * plateOffsetY * plateVisualGameObjectsList.Count;

        plateVisualGameObjectsList.Add(plateVisualTransform.gameObject);
    }

    private void DeletePlate(object sender, EventArgs e)
    {
        if (plateVisualGameObjectsList.Count > 0)
        {
            GameObject plateGameObject = plateVisualGameObjectsList[plateVisualGameObjectsList.Count - 1];
            plateVisualGameObjectsList.Remove(plateGameObject);
            Destroy(plateGameObject);
        }
    }

    public void SpawnPlate()
    {
        Transform plateVisualTransform = Instantiate(plateVisualPrefab, counterTop);

        float plateOffsetY = 0.1f;
        plateVisualTransform.localPosition = Vector3.up * plateOffsetY * plateVisualGameObjectsList.Count;

        plateVisualGameObjectsList.Add(plateVisualTransform.gameObject);
    }

    public void DeletePlate()
    {
        if (plateVisualGameObjectsList.Count > 0)
        {
            GameObject plateGameObject = plateVisualGameObjectsList[plateVisualGameObjectsList.Count - 1];
            plateVisualGameObjectsList.Remove(plateGameObject);
            Destroy(plateGameObject);
        }
    }
}
