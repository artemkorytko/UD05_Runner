using System;
using System.Collections;
using UnityEngine;

namespace Runner
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Level levelPrefab;
        [SerializeField] private float delay = 1f;
        private Level _level;

        private Coroutine _coroutine;
        private InterstitialAdExample _interstitialAdExample;
        private AnalyticsManager _analyticsManager;

        // private int sumTwoDigit;
        //
        // private int myProp;
        // private MyClass myClass;
        //
        // public int MyProp1
        // {
        //     get => myProp;
        //     set => myProp = value;
        // }
        //
        // public int MyProp2
        // {
        //     get { return myProp; }
        //
        //     set { myProp = value; }
        // }
        //
        // public void DoSum(int x, int y) => myProp = x + y;
        //
        // public void DoSum1(int x, int y)
        // {
        //     myProp = x + y;
        // }
        //
        // public int DoSumRet1(int x, int y) => x + y;
        //
        // public int DoSumRet2(int x, int y)
        // {
        //     return x + y;
        // }
        //
        // //ref
        // //out
        // public MyClass GetSumOfTwoDigit(MyClass x, MyClass y)
        // {
        //     return new MyClass() {MyInt = x.MyInt + y.MyInt};
        // }
        //
        // public void DoSumAndReturnEnterDigit(int x, int y, out MyClass sum)
        // {
        //     sum = new MyClass();
        //     sum.MyText = "dgfgh";
        //     sum.MyInt = x + y;
        // }
        //
        // public void DoSumAndReturnEnterDigit1(int x, int y, ref MyClass sum)
        // {
        //     sum.MyInt = x + y;
        // }

        private void Awake()
        {
            // // var x = new MyClass() {MyInt = 10};
            // // var y = new MyClass() {MyInt = 20};
            // // Debug.Log(GetSumOfTwoDigit(x, y).MyInt);
            // // Debug.Log(x.MyInt);
            //
            // int x = 10;
            // int y = 20;
            // // int sum;
            //
            // MyClass sum1 = new MyClass();
            // DoSumAndReturnEnterDigit1(x, y, ref myClass);
            // DoSumAndReturnEnterDigit(x, y, out myClass);
            // //Debug.Log(sum);


            _interstitialAdExample = FindObjectOfType<InterstitialAdExample>();
            _analyticsManager = FindObjectOfType<AnalyticsManager>();
            _level = Instantiate(levelPrefab, transform);
        }

        private void Start()
        {

            try
            {
                MyClassBase classBase = new MyClassBase();
                classBase.Init();
            }
            catch (Exception e)
            {
               Debug.LogError(e);
            }
           
            // StartLevel();
            Do(() =>
            {
                _level.GenerateLevel();
                _level.Player.OnWin += OnWin;
                _level.Player.OnDead += OnDead;
            });

            Do(StartLevel);
        }

        private void Do(Action callback)
        {
            callback?.Invoke();
        }

        private void StartLevel()
        {
            _level.GenerateLevel();
            _level.Player.OnWin += OnWin;
            _level.Player.OnDead += OnDead;
        }

        private void OnDead()
        {
            StartCoroutine(FailWithDelay());
        }

        private void OnWin()
        {
            StartCoroutine(WinWithDelay());
        }

        private IEnumerator WinWithDelay()
        {
            yield return ClearDelay();
            StartLevel();
            _interstitialAdExample.ShowAd();
        }

        private IEnumerator FailWithDelay()
        {
            yield return ClearDelay();
            StartLevel();
            _interstitialAdExample.ShowAd();
        }

        private IEnumerator ClearDelay()
        {
            _level.Player.OnWin -= OnWin;
            _level.Player.OnDead -= OnDead;
            yield return new WaitForSeconds(delay);
        }

        public void StartRun()
        {
            _level.Player.IsActive = true;
            _analyticsManager.SendLevelStart(1);
        }
    }

    // public class MyClass
    // {
    //     public int MyInt;
    //     public string MyText;
    // }
}