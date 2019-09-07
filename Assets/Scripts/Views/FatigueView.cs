using CardGame.Common;
using CardGame.Common.Notifications;
using CardGame.GameActions;
using DG.Tweening;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Sirenix.OdinInspector;
using System.Collections;
using TMPro;
using UnityEngine;

namespace CardGame.Views
{
    public class FatigueView : MonoBehaviour, IOnEventCallback
    {
        [Required] [SerializeField] private TextMeshProUGUI fatigueText = null;

        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
            if (PhotonNetwork.IsMasterClient)
            {
                this.AddObserver(OnPrepareFatigue, NotificationHelper.PrepareNotification<FatigueAction>());
            }
        }

        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
            if (PhotonNetwork.IsMasterClient)
            {
                this.RemoveObserver(OnPrepareFatigue, NotificationHelper.PrepareNotification<FatigueAction>());
            }
        }

        public void OnEvent(EventData photonEvent)
        {
            if (photonEvent.Code != EventCodes.OnFatigue) { return; }

            var fatigueAmount = (int)photonEvent.CustomData;

            StartCoroutine(DisplayFatigue(fatigueAmount));
        }

        private void OnPrepareFatigue(object sender, object args)
        {
            var fatigueAction = args as FatigueAction;
            fatigueAction.PerformPhase.Viewer = FatigueViewer;
        }

        private IEnumerator FatigueViewer(IContainer game, GameAction action)
        {
            yield return true;

            var fatigueAction = action as FatigueAction;

            yield return StartCoroutine(DisplayFatigue(fatigueAction.Player.Fatigue));
        }

        private IEnumerator DisplayFatigue(int fatigueAmount)
        {
            fatigueText.text = $"Fatigue\n{fatigueAmount}";

            var sequence = DOTween.Sequence()
                .Append(transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack))
                .Append(transform.DOScale(new Vector3(0f, 1f, 0f), 0.5f).SetEase(Ease.InBack));

            while (sequence.IsActive()) { yield return null; }
        }
    }
}
