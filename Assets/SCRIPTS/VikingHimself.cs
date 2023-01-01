using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VikingHimself : MonoBehaviour
{
    [SerializeField] private GameObject mygold;
    [SerializeField] private GameObject iamviking;

    private V_PriestController _v_priescofile;
    private VikingsController vikifile;


    private void Awake()
    {
        _v_priescofile = FindObjectOfType<V_PriestController>();
        _v_priescofile.BumPoBashke += Ischez; // прихоит из священника, по триггеру в канделябре

        vikifile = FindObjectOfType<VikingsController>();
        vikifile.GotGold += ShowGold;
    }

    void Start()
    {
        mygold.SetActive(false);
    }

    //============== временная функция пропажи викинга =======================
    private void Ischez(VikingHimself bashka)
    {
        if (bashka == this)
        {
            iamviking.SetActive(false);
        }
    }

    private void ShowGold(VikingHimself oneviking)
    {
        if (oneviking == this)
        {
            mygold.SetActive(true);
            //mygold.GetComponent<SpriteRenderer>().sortingOrder = 105;
        }
    }

}