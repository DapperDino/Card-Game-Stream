using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame.Views
{
    public class HandView : MonoBehaviour
    {
        [Required] [SerializeField] private Transform activeHandle = null;
        [Required] [SerializeField] private Transform inactiveHandle = null;

        private const float Overlap = 0.4f;

        private List<Transform> cards = new List<Transform>();
        private Transform mainCameraTransform = null;

        private void Start() => mainCameraTransform = Camera.main.transform;

        public IEnumerator AddCard(Transform card, bool showPreview, bool isOverdraw)
        {
            if (showPreview)
            {
                var preview = ShowPreview(card);
                while (preview.MoveNext()) { yield return null; }
            }

            if (isOverdraw)
            {
                var discard = OverdrawCard(card);
                while (discard.MoveNext()) { yield return null; }
            }
            else
            {
                cards.Add(card);
                var layout = LayOutCards();
                while (layout.MoveNext()) { yield return null; }
            }
        }

        private IEnumerator ShowPreview(Transform card)
        {
            var cardView = card.GetComponent<CardView>();

            var sequence = DOTween.Sequence()
                .Append(card.DORotate(activeHandle.rotation.eulerAngles, 1f))
                .Join(card.DOMove(activeHandle.position, 1f).SetEase(Ease.OutBack)
                .OnUpdate(() =>
                {
                    if (!cardView.IsFaceUp)
                    {
                        var toCard = (mainCameraTransform.position - card.position).normalized;
                        if (Vector3.Dot(card.up, toCard) > 0)
                        {
                            cardView.Flip(true);
                        }
                    }
                }))
                .AppendInterval(1f);

            while (sequence.IsActive()) { yield return null; }
        }

        private IEnumerator OverdrawCard(Transform card)
        {
            var tweener = card.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack);

            while (tweener.IsActive()) { yield return null; }

            Destroy(card.gameObject);
        }

        private IEnumerator LayOutCards()
        {
            float width = cards.Count * Overlap;
            float xPos = -(width / 2);

            Tweener tweener = null;
            for (int i = 0; i < cards.Count; i++)
            {
                var canvas = cards[i].GetComponentInChildren<Canvas>();
                canvas.sortingOrder = i;

                var position = inactiveHandle.position + new Vector3(xPos, 0f, 0f);
                cards[i].DORotate(inactiveHandle.rotation.eulerAngles, 0.25f);
                tweener = cards[i].DOMove(position, 0.25f);
                xPos += Overlap;
            }

            while (tweener.IsActive()) { yield return null; }
        }
    }
}
