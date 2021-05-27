using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyDeliever : MonoBehaviour
{
    public int energyDelivered;
    public int energyNecessary = 100;
    public int energyMissing;
    public GameObject delieverVFX;

    private void Start()
    {
        GameManager.scriptCanvas.deliverer = GetComponent<EnergyDeliever>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && energyDelivered < energyNecessary)
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (delieverVFX != null)
            {
                GameObject effect = Instantiate(delieverVFX);
                effect.transform.position = transform.position;

            }

            energyMissing = energyNecessary - energyDelivered;

            energyDelivered = Mathf.Clamp(energyDelivered + player.score, 0, energyNecessary);

            player.ScoreChange(-energyMissing);


            if (energyDelivered >= energyNecessary)
            {
                FindObjectOfType<Arena>().SummonBoss();
                GetComponent<Animator>().Play("fill");
            }
        }
    }

}
