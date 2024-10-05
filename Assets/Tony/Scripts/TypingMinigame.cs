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
        [SerializeField] private TextAsset wordListAsset;

        private List<string> wordList;
        private int currentWordIndex = 0;
        private string currentWord;
        private int currentLetterIndex = 0;

        private void Start()
        {
            GameController.Instance.GameModeChanged += HandleGameModeChanged;
            LoadWordList();
            StartMinigame();
        }

        private void HandleGameModeChanged(GameController.GameModes newGameMode)
        {
            if (newGameMode == GameController.GameModes.Recruiting)
            {
                enabled = true;
            }
            else enabled = false;
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
            slider.value = 0.5f; //0;
            ShuffleWordList();
            PlayNextWord();
        }

        private void PlayNextWord()
        {
            if (currentWordIndex == wordList.Count)
            {
                ShuffleWordList();
                currentWordIndex = 0;
            }
            PlayWord(wordList[currentWordIndex]);
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
                slider.value += 0.05f;
                honeycomb.TypeLetterIndex(currentLetterIndex);
                currentLetterIndex++;
                //TODO have extra spectator bees show up to provide visual feedback that the player is doing well

                if (currentLetterIndex == currentWord.Length)
                {
                    PlayNextWord();
                }
            }
            else if (Input.anyKeyDown)
            {
                honeycomb.ShowMistakeLetterIndex(currentLetterIndex);
                slider.value -= 0.1f;
            }
            else
            {
                slider.value -= Time.deltaTime * 0.1f;
            }

            if (slider.value <= 0)
            {
                //minigame ends
                //TODO output number of successful words to inform number of bees in next pollen hunt
                GameController.Instance.SetGameMode(GameController.GameModes.Flying);
            }
        }

        


    }
}
