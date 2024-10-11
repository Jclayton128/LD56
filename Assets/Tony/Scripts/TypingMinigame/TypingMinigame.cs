using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BeeGame.TypingGame
{

    public class TypingMinigame : MonoBehaviour
    {

        [SerializeField] private Honeycomb honeycomb;
        [SerializeField] private Slider slider;
        [SerializeField] private Slider wordCountSlider;
        [SerializeField] private TextAsset wordListAsset;

        [SerializeField] float _startingSliderValue = 1f;
        [SerializeField] float _bonusUponCorrectKey = 0.05f;
        [SerializeField] float _malusUponWrongKey = 0.05f;
        [SerializeField] float _timeMultiplier = 0.6f;

        bool _hasPressedAKeyThisRun = false;

        private const int MaxWords = 10;

        private List<string> wordList;
        private int currentWordIndex;
        private string currentWord;
        private int currentLetterIndex;
        private int numWordsCompleted;

        private void Start()
        {
            if (GameController.Instance != null)
            {
                GameController.Instance.GameModeChanged += HandleGameModeChanged;
            }
            LoadWordList();
            StartMinigame();
        }

        private void HandleGameModeChanged(GameController.GameModes newGameMode)
        {
            if (newGameMode == GameController.GameModes.Recruiting)
            {
                enabled = true;
                Invoke(nameof(Delay_StartMinigame), 0.01f);
            }
            else enabled = false;
        }

        private void Delay_StartMinigame()
        {
            StartMinigame();
        }

        private void LoadWordList()
        {
            wordList = new List<string>();
            foreach (var word in wordListAsset.text.Split('\n'))
            {
                var trimmed = word.Trim();
                if (string.IsNullOrEmpty(trimmed)) continue;
                wordList.Add(trimmed.ToUpper());
            }
            if (wordList.Count == 0)
            {
                Debug.LogError("Word list is empty.");
            }
        }

        private void ShuffleWordList()
        {
            if (wordList == null || wordList.Count < 2) return;
            for (int i = 0; i < wordList.Count - 1; i++)
            {
                int j = UnityEngine.Random.Range(i, wordList.Count);
                var tmp = wordList[i];
                wordList[i] = wordList[j];
                wordList[j] = tmp;
            }
        }

        public void StartMinigame()
        {
            slider.value = _startingSliderValue; //0;
            wordCountSlider.value = 0;
            numWordsCompleted = 0;
            currentWordIndex = 0;
            ShuffleWordList();
            PlayNextWord();
            _hasPressedAKeyThisRun = false;
        }

        private void PlayNextWord()
        {
            if (currentWordIndex == wordList.Count)
            {
                ShuffleWordList();
                currentWordIndex = 0;
            }
            PlayWord(wordList[currentWordIndex]);
            //AUDIO This is called whenever the player has successfully played a word. Maybe a happy 'success' sound?
            currentWordIndex++;
        }

        private void PlayWord(string word)
        {
            currentWord = word;
            honeycomb.SetupWord(word);
            currentLetterIndex = 0;
        }

        private void Update()
        {

            if (currentWord == null) return;
            var requiredCharacter = currentWord[currentLetterIndex];
            var requiredKeyCode = KeyCode.A + requiredCharacter - 'A';
            if (Input.GetKeyDown(requiredKeyCode))
            {
                _hasPressedAKeyThisRun = true;
                slider.value += _bonusUponCorrectKey;
                honeycomb.TypeLetterIndex(currentLetterIndex);
                currentLetterIndex++;
                //TODO have extra spectator bees show up to provide visual feedback that the player is doing well

                if (currentLetterIndex == currentWord.Length)
                {
                    numWordsCompleted++;
                    wordCountSlider.value = numWordsCompleted;
                    PlayNextWord();
                }
            }
            else if (Input.anyKeyDown)
            {
                _hasPressedAKeyThisRun = true;
                honeycomb.ShowMistakeLetterIndex(currentLetterIndex);
                slider.value -=
                    _malusUponWrongKey * UpgradeController.Instance.TypingMultiplier;
                //AUDIO this is called when the player has made a typing error. Maybe a sad/angry buzz sound?
            }
            else if (_hasPressedAKeyThisRun)
            {
                slider.value -= Time.deltaTime * _timeMultiplier * UpgradeController.Instance.TypingMultiplier;
            }

            if (slider.value <= 0 || numWordsCompleted >= MaxWords)
            {
                //minigame ends
                //TODO output number of successful words to inform number of bees in next pollen hunt
                if (GameController.Instance != null 
                    && PollenRunController.Instance != null)
                {
                    PollenRunController.Instance.FollowerToSpawnOnNextPollenRun = numWordsCompleted;
                    GameController.Instance.SetGameMode(GameController.GameModes.Flying);
                }
            }
        }

        


    }
}
