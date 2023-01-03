using System;
using UnityEngine;
// Индивидуальная кучка золота в церкви

namespace Runner
{
    public class GoldPiece: MonoBehaviour
    {
        // [SerializeField] private GameObject goldhaufen;
        // private VikingsController vikifile;
        // private V_PriestController _v_priescofile;
        // private void Awake()
        // {
        //     vikifile = FindObjectOfType<VikingsController>();
        //     
        //     // слушает общий файл, не дошел ли там викинг до золота
        //     vikifile.GotGold += HideGoldinChurch;
        //     
        //     // слушает в священнике, не долбанули ли по башке викингу с золотом
        //     _v_priescofile.BumPoBashke += Golddropped;
        //     
        //     
        // }
        //
        // private void HideGoldinChurch(VikingHimself oneviking)
        // {
        //     if (oneviking == this) // А НИФИГА !!!!!!!!!!!!!!!
        //     {
        //         // викинг взял золото - золото пропало
        //         goldhaufen.SetActive(false);
        //         
        //     }
        // }
        //
        // private void Golddropped(VikingHimself oneviking)
        // {
        //     if (oneviking == this) // А НИФИГА!!!!!!!!!!!!!!!
        //     {
        //         // викинг взял золото - золото пропало
        //         goldhaufen.SetActive(true);
        //         
        //         // ???????????????????????????????????????????????????????????????????????
        //         // НАДО получить вторым параметром (???) позицию дистроя викинга и оставить кучку золота там, где его пришлёпнули!!!!
        //         // goldhaufen.transform.position = diepos;
        //
        //     }
        // }
        //
        // private void OnDestroy()
        // {
        //     vikifile.GotGold -= HideGoldinChurch;
        //     _v_priescofile.BumPoBashke -= Golddropped;
        // }
    }
}