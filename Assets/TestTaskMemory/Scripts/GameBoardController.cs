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
            var generatedCards = new List<CardController>();

            // Spawn cards.
            Slots.ForEach(slot => generatedCards.Add(Instantiate(cardPrefab, slot, false)));

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
            card.Open();


            if (openCard == null)
            {
                openCard = card;
            }
            else if (openCard.Value == card.Value)
            {
                yield return new WaitForSeconds(animationDelay);
                openCard = null;
                // TODO put disappear logic.
            }
            else
            {
                yield return new WaitForSeconds(animationDelay);
                openCard.Close();
                card.Close();
                openCard = null;
            }

            touchCoriutine = null;
        }
    }
}