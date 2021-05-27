using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Hud : MonoBehaviour
{
    public GameObject connectedPlayer;
    private Player atributes;
    private PlayerActions actions;

    public Image healthFill;
    public Image scoreFill;
    public Image shotCooldownFill;
    public Image shieldCooldownFill;
    public Image blinkCooldownFill;
    public Image reviveCooldownFill;

    public Image[] shieldChargeIcon = new Image[3];
    public Image[] buffDurationFill = new Image[3];

    public Text healthText;
    public Text scoreText;
    public Text shotCooldownText;
    public Text shieldCooldownText;
    public Text blinkCooldownText;
    public Text reviveCooldownText;
    public Text shotCostText;
    public Text shieldCostText;
    public Text blinkCostText;
    public Text reviveCostText;
    public Text timerText;

    private void Start()
    {
        GameManager.currentTime = 0;
        GameManager.timerPaused = false;
        GameManager.scriptHud = GetComponent<Hud>();

        connectedPlayer = GameManager.PlayerCharacter;

        if (connectedPlayer != null)
        {
            atributes = connectedPlayer.GetComponent<Player>();
            actions = connectedPlayer.GetComponent<PlayerActions>();
        }

    }

    private void Update()
    {
        if (connectedPlayer != null)
        {
            healthFill.fillAmount = (float)atributes.hp / (float)atributes.hpMax;
            scoreFill.fillAmount = (float)atributes.score / (float)atributes.scoreMax;
            healthText.text = atributes.hp + "/" + atributes.hpMax;
            scoreText.text = atributes.score.ToString();

            if (atributes.buffActive[2]) shotCostText.text = "0";
            else shotCostText.text = actions.shotCost.ToString();
            shieldCostText.text = actions.restoreCost.ToString();
            blinkCostText.text = actions.blinkCost.ToString();
            reviveCostText.text = actions.reviveCost.ToString();

            if (atributes.buffActive[2]) shotCooldownFill.fillAmount = GetFillValueFromTimer(actions.shotBoostCooldown, actions.shotCooldownTimer);
            else shotCooldownFill.fillAmount = GetFillValueFromTimer(actions.shotBaseCooldown, actions.shotCooldownTimer);

            shieldCooldownFill.fillAmount = GetFillValueFromTimer(actions.restoreCooldown, actions.restoreCooldownTimer);
            blinkCooldownFill.fillAmount = GetFillValueFromTimer(actions.blinkCooldown, actions.blinkCooldownTimer);
            reviveCooldownFill.fillAmount = GetFillValueFromTimer(actions.reviveCooldown, actions.reviveCooldownTimer);

            if (atributes.buffActive[2]) shotCooldownText.text = GetSecondsFromTimer(actions.shotBoostCooldown, actions.shotCooldownTimer);
            else shotCooldownText.text = GetSecondsFromTimer(actions.shotBaseCooldown, actions.shotCooldownTimer);

            shieldCooldownText.text = GetSecondsFromTimer(actions.restoreCooldown, actions.restoreCooldownTimer);
            blinkCooldownText.text = GetSecondsFromTimer(actions.blinkCooldown, actions.blinkCooldownTimer);
            reviveCooldownText.text = GetSecondsFromTimer(actions.reviveCooldown, actions.reviveCooldownTimer);

            for (int i = 0; i < atributes.buffActive.Length; i++)
            {
                if (atributes.buffActive[i])
                {
                    buffDurationFill[i].transform.parent.gameObject.SetActive(true);
                    buffDurationFill[i].fillAmount = GetFillValueFromTimer(atributes.buffDuration[i], atributes.buffDurationTimer[i]);
                }
                else
                    buffDurationFill[i].transform.parent.gameObject.SetActive(false);
            }

            if (GameManager.timerPaused == false) GameManager.currentTime += Time.deltaTime;
            timerText.text = Mathf.Round(GameManager.currentTime).ToString();

        }
    }

    private float GetFillValueFromTimer(float cooldownValue, float cooldownTimer)
    {
        if (Time.time > cooldownTimer) return 0;
        else return (cooldownTimer - Time.time) / cooldownValue;
    }
    private string GetSecondsFromTimer(float cooldownValue, float cooldownTimer)
    {
        if (Time.time > cooldownTimer) return null;
        else return (1 +(int)(cooldownTimer - Time.time)).ToString();
    }
}
