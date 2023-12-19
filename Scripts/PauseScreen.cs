using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{
    public Text currentMunitionText;
    public Text currentCartridgesText;
    public Text currentHealthPacks;
    public GameObject gunModel;

    private void OnEnable()
    {
        if (PlayerController.armed)
        {
            gunModel.SetActive(true);
            currentMunitionText.text = $"Balas: {PlayerController.currentMunition.ToString("D2")}";
        }
        else
        {
            gunModel.SetActive(false);
            currentMunitionText.enabled = false;
        }
        currentCartridgesText.text = $"Cartuchos: {PlayerController.currentCartridges.ToString("D2")}";
        currentHealthPacks.text = $"Kits: {PlayerController.currentHealthPacks.ToString("D2")}";
    }

    private void Start()
    {
        if (PlayerController.armed)
        {
            gunModel.SetActive(true);
            currentMunitionText.text = $"Balas: {PlayerController.currentMunition.ToString("D2")}";
        }
        else
        {
            gunModel.SetActive(false);
            currentMunitionText.enabled = false;
        }
        currentCartridgesText.text = $"Cartuchos: {PlayerController.currentCartridges.ToString("D2")}";
        currentHealthPacks.text = $"Kits: {PlayerController.currentHealthPacks.ToString("D2")}";
    }

    private void Update()
    {
        if (PlayerController.armed)
        {
            gunModel.SetActive(true);
            currentMunitionText.text = $"Balas: {PlayerController.currentMunition.ToString("D2")}";
        }
        else
        {
            gunModel.SetActive(false);
            currentMunitionText.enabled = false;
        }
        currentCartridgesText.text = $"Cartuchos: {PlayerController.currentCartridges.ToString("D2")}";
        currentHealthPacks.text = $"Kits: {PlayerController.currentHealthPacks.ToString("D2")}";
    }
}
