using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeeGame.TypingGame
{

    [Serializable]
    public class LetterSet
    {
        [SerializeField] private List<Letter> letters;

        public List<Letter> Letters => letters;
    }

    public class Honeycomb : MonoBehaviour
    {

        [SerializeField] private List<LetterSet> letterSets;
        [SerializeField] private RectTransform bee;

        public List<LetterSet> LetterSets => letterSets;

        private const int MinWordLength = 2;
        private const int MaxWordLength = 8;
        private WaitForSeconds LetterAppearanceDelay = new WaitForSeconds(0.05f);
        private const float BeeSpeed = 250f;

        private LetterSet currentLetterSet;
        private Letter beeTargetLetter;

        public void HideAllLetters()
        {
            foreach (var letterSet in letterSets)
            {
                foreach (var letter in letterSet.Letters)
                {
                    letter.Hide();
                }
            }
            beeTargetLetter = null;
        }

        public void SetupWord(string word)
        {
            if (!(MinWordLength <= word.Length && word.Length <= MaxWordLength))
            {
                Debug.LogError($"Invalid length: '{word}'. Must be {MinWordLength}-{MaxWordLength} letters.");
                return;
            }
            HideAllLetters();
            StartCoroutine(SetupLettersInWord(word));
        }

        private IEnumerator SetupLettersInWord(string word)
        {
            currentLetterSet = LetterSets[word.Length];
            for (int i = 0; i < word.Length; i++)
            { 
                var letter = currentLetterSet.Letters[i];
                letter.ShowUntyped(word[i]);
                yield return LetterAppearanceDelay;
            }
        }

        public void TypeLetterIndex(int index)
        {
            currentLetterSet.Letters[index].ShowTyped();
            beeTargetLetter = currentLetterSet.Letters[index];
        }

        public void ShowMistakeLetterIndex(int index)
        {
            currentLetterSet.Letters[index].ShowMistake();
        }

        private void Update()
        {
            var targetPosition = (beeTargetLetter != null)
                ? beeTargetLetter.GetComponent<RectTransform>().anchoredPosition
                : Vector2.zero;
            var direction = (targetPosition - bee.anchoredPosition).normalized;
            bee.anchoredPosition += BeeSpeed * direction * Time.deltaTime;
        }

    }

}
