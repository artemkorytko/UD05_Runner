using System;
using System.Collections;
using Unity.VisualScripting;
// using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Runner
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Level levelPrefab;

        // считает сколько типов уровня в awake
        [SerializeField] public GameConfigsContainer container;
        private int howmanylevels;
        public int leveltype = 0;

        //поле для задержек в корутинах:
        [SerializeField] private float delay = 3f;

        private Level _level;
        private PlayerController _playercontrollerfile;

        public int howMuchCoins = 0; //считаем монетки ЗА ВСЮ ИГРУ
        
        

        public event Action LevelChanged; //событие для передачи на панель
        public event Action CoinsEncreqased; //событие для передачи на панель

        private void Awake()
        {
            _level = Instantiate(levelPrefab, transform); // #######################[ внутрь текущего трансформа ??? ]


            // считает сколько типов уровня 
            // container = FindObjectOfType<GameConfigsContainer>();
            howmanylevels = container.howmanyLevelConfigs; //3 пришло
        }

        // пока на старте потом по кнопке
        private void Start()
        {
            StartLevel(); //см. ниже, вынесли их чтобы не повторять. ПОТОМ ИЗ КНОПКИ
        }


        //----------------------пытаюсь тут всунуть увеличение количества монеток. ОНО У МЕНЯ ОБЩЕЕ, НА ВСЕ УРОВНИ------
        public void PlusPlus(CoinComponent somewordwasCoin)
        {
            howMuchCoins++;
            CoinsEncreqased?.Invoke();
        }


        // выносим это в отдельный метод ибо левел генерится много раз

        private void StartLevel()
        {
            _level.GenerateLevel(); // обратились к функции внутри Level вроде как

            // надо запустить игрока 
            _level.Player.IsActive = true;

            // подписки на события из плеера в PlayerController, ибо он там пересекает и стукается
            _level.Player.Dobezal += OnWin; // у АК тут опять одинаковые слова
            _level.Player.OnDie += OnDead;

            // подписка для счетчика монет
            _playercontrollerfile = FindObjectOfType<PlayerController>();
            _playercontrollerfile.GetCoin += PlusPlus;

            // Начать звук бега: прям сразу ищет компонент и в нем запустили функцию О_о
            AudioManager audioManager = FindObjectOfType<AudioManager>();
            audioManager.InitSound();

            LevelChanged?.Invoke();
        }

        //------------ методы по событиям --------------------------------------------------------------------------
        private void OnDead()
        {
            // надо сделать задержку для того чтобы посмотрели анимацию -> корутины
            StartCoroutine(FailWithdelay());

            // добвалено !
            leveltype = 0;
        }


        private void OnWin()
        {
            // надо сделать задержку для того чтобы посмотрели анимацию > корутины
            StartCoroutine(WinWithdelay());

            //--- добвалено с конфигами ------------------------------------------------
            // если он подряд два выграл - то играет последний уровень, пока не врежется
            //--------------------------------------------------------------------------
            if (leveltype < howmanylevels - 1) // -1 ибо считает штуки конфигов в массиве
            {
                leveltype++;
            }
        }


        //------- КОРУТИНЫ для задержек - она остановит программу и будет ждать пока отработиает Иелдоператор ------- 
        private IEnumerator WinWithdelay()
        {
            yield return ClearDelay();
        }


        private IEnumerator FailWithdelay()
        {
            yield return ClearDelay();
        }
        //-----------------------------------------------------------------------------------------------------------


        //--------------вынесли отписки и паузу-----------------------------
        // из-за того, что и на победу и на стук об стену - вызываем одинаковые
        // отписки от уровня и генерацию нового - выносим это в отдельную корутинку
        private IEnumerator ClearDelay()
        {
            // отписаться
            _level.Player.Dobezal -= OnWin;
            _level.Player.OnDie -= OnDead;

            // код отсновит выполнение в этом методе, пока не выполнится это условие
            yield return new WaitForSeconds(delay);

            // и запустить новый уровень
            StartLevel();

            FindObjectOfType<GamePanel>().PrintLevelText();
        }
    }
}