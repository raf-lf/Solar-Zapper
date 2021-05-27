using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [Header("Components")]
    public Animator overlay;
    public AudioSource musicSource;

    [Header("Boss HUD")]
    public Animator bossHud;
    public Image bossHpFill;
    public Text bossName;
    public Damageable bossScript;

    [Header("Summon HUD")]
    public Image summonFill;
    public Text summonText;
    public EnergyDeliever deliverer;

    private void Start()
    {
        GameManager.scriptCanvas = GetComponent<CanvasManager>();
        GameManager.Overlay = overlay;
        GameManager.musicSource = musicSource;
        GameManager.BossHud = bossHud;
    }

    private void Update()
    {
        UpdateInfo();
    }

    public void UpdateInfo()
    {
        if (bossScript != null) bossHpFill.fillAmount = (float)bossScript.hp / (float)bossScript.hpMax;
        if (deliverer != null)
        {
            summonText.text = deliverer.energyDelivered + "/" + deliverer.energyNecessary;
            summonFill.fillAmount = (float)deliverer.energyDelivered / (float)deliverer.energyNecessary;
        }

    }
}
