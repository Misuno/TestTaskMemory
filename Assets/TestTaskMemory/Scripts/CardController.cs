using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace Misuno.TestTaskMemory
{
    public class CardController : MonoBehaviour, IPointerClickHandler
    {
        public float animationSpeed = 1f;
        public float disappearDuration;

        public GameObject cardFace;

        private Text valueText;
        private int value;

        private Coroutine activeCoroutine;

        public int Value
        {
            get { return value; }

            set
            {
                this.value = value;

                if (valueText == null)
                {
                    valueText = GetComponentInChildren<Text>(true);
                }

                valueText.text = value == -1 ? "" : value.ToString();
            }
        }

        #region IPointerClickHandler implementation

        public void OnPointerClick(PointerEventData eventData)
        {
            if (World.board == null)
                return;

            World.board.CardTouched(this);
        }

        #endregion

        public Coroutine Open()
        {
            return RotateCard(true);
        }

        public Coroutine Close()
        {
            return RotateCard(false); 
        }

        private Coroutine RotateCard(bool setOpen)
        {
            if (activeCoroutine != null)
            {
                StopCoroutine(activeCoroutine);
            }
            return activeCoroutine = StartCoroutine(Rotate(animationSpeed, () => this.cardFace.SetActive(setOpen)));
        }

        private IEnumerator Rotate(float speed, Action finishAction)
        {
            // Scale from current value down to 0.
            while (!Mathf.Approximately(transform.localScale.x, 0f))
            {
                RotationStep(-speed);
                yield return null;
            }

            finishAction();

            // Afterwards scale back to 1.
            while (!Mathf.Approximately(transform.localScale.x, 1f))
            {
                RotationStep(speed);
                yield return null;
            }

        }

        void RotationStep(float speed)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Clamp01(scale.x + speed * Time.deltaTime);
            transform.localScale = scale;
        }

        public void Disappear()
        {
            var myImages = GetComponentsInChildren<MaskableGraphic>();
            foreach (var myImage in myImages)
            {
                myImage.raycastTarget = false;
                DOTween.ToAlpha(() => myImage.color, a => myImage.color = a, 0, disappearDuration);
            }
            Destroy(gameObject, disappearDuration);
        }
    }
}
