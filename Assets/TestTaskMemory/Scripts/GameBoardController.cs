using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Misuno.TestTaskMemory
{
    public class GameBoardController : MonoBehaviour
    {
        public CardController cardPrefab;

        public float animationDelay;

        // Use this for initialization
        private List<Transform> slots = new List<Transform>();
        private List<CardController> cards = new List<CardController>();

        private Coroutine touchCoriutine;

        public List<Transform> Slots
        {
            get
            {
                return slots;
            }
        }

        private CardController openCard;

        private void Awake()
        {
            World.board = this;

            for (var i = 0; i < transform.childCount; ++i)
            {
                Transform row = transform.GetChild(i);
                for (var j = 0; j < row.childCount; ++j)
                {
                    slots.Add(row.GetChild(j));
                }
            }
        }

        private void Start()
        {
            GenerateCards();
        }

        void GenerateCards()
        {
            // Clean up.
            if (cards.Count > 0)
            {
                cards.ForEach(card => Destroy(card.gameObject));
                cards.Clear();
            }

            var generatedCards = new List<CardController>();

            // Spawn cards.
            Slots.ForEach(slot => generatedCards.Add(Instantiate(cardPrefab, slot, false)));
            cards.AddRange(generatedCards);

            // Set values for pairs.
            int valueForCard = 0;
            while (generatedCards.Count > 1)
            {
                int index = Random.Range(0, generatedCards.Count);
                CardController firstCard = generatedCards[index];
                generatedCards.RemoveAt(index);

                index = Random.Range(0, generatedCards.Count);
                CardController secondCard = generatedCards[index];
                generatedCards.RemoveAt(index);

                firstCard.Value = valueForCard;
                secondCard.Value = valueForCard;
                valueForCard++;
            }

            // Last card is blank, ie. -1.
            generatedCards[0].Value = -1;
        }

        public void CardTouched(CardController card)
        {
            if (touchCoriutine != null)
                return;
            
            touchCoriutine = StartCoroutine(CardTouch(card));
        }

        private IEnumerator CardTouch(CardController card)
        {
            if (openCard == null)
            {
                openCard = card;
                yield return card.Open();
            }
            else if (openCard == card)
            {
                yield return card.Close();
            }
            else
            {
                yield return card.Open();

                if (openCard.Value == card.Value)
                {
                    yield return new WaitForSeconds(animationDelay);
                    cards.Remove(openCard);
                    openCard.Disappear();
                    openCard = null;

                    cards.Remove(card);
                    card.Disappear();
                }
                else
                {
                    yield return new WaitForSeconds(animationDelay);
                    openCard.Close();
                    openCard = null;
                    yield return card.Close();
                }
            }

            touchCoriutine = null;

            if (cards.Count == 1)
            {
                GenerateCards();
            }
        }
    }
}