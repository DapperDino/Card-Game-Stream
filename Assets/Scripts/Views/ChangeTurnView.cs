﻿using CardGame.Common;
using CardGame.Common.Notifications;
using CardGame.Common.StateMachines;
using CardGame.GameActions;
using CardGame.GameStates;
using CardGame.Systems;
using DG.Tweening;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace CardGame.Views
{
    public class ChangeTurnView : MonoBehaviour, IOnEventCallback
    {
        [Required] [SerializeField] private Transform yourTurnBanner = null;
        [Required] [SerializeField] private ChangeTurnButtonView buttonView = null;

        private IContainer game = null;

        private void Awake()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                game = GetComponentInParent<GameViewSystem>().Container;
            }
        }

        private void Start()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                NotifyFirstTurnStarting();
            }
        }

        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
            if (PhotonNetwork.IsMasterClient)
            {
                this.AddObserver(OnPrepareChangeTurn, NotificationHelper.PrepareNotification<ChangeTurnAction>());
            }
        }

        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
            if (PhotonNetwork.IsMasterClient)
            {
                this.RemoveObserver(OnPrepareChangeTurn, NotificationHelper.PrepareNotification<ChangeTurnAction>());
            }
        }

        private void OnPrepareChangeTurn(object sender, object args)
        {
            var changeTurnAction = args as ChangeTurnAction;
            changeTurnAction.PerformPhase.Viewer = ChangeTurnViewer;
        }

        private void NotifyFirstTurnStarting()
        {
            byte currentPlayerIndex = game.GetMatch().CurrentPlayerIndex;
            var raiseEventoptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            var sendOptions = new SendOptions { Reliability = true };
            PhotonNetwork.RaiseEvent(EventCodes.OnFirstTurnStart, currentPlayerIndex, raiseEventoptions, sendOptions);
        }

        public void OnEvent(EventData photonEvent)
        {
            switch (photonEvent.Code)
            {
                case EventCodes.OnFirstTurnStart:
                    var firstTurnPlayerIndex = (byte)photonEvent.CustomData;
                    float targetRotation = GetTargetButtonRotation(firstTurnPlayerIndex);
                    buttonView.RotationHandle.eulerAngles = new Vector3(targetRotation, 0f, 0f);
                    return;

                case EventCodes.OnChangeTurnRequest:
                    var requestedPlayerIndex = (byte)photonEvent.CustomData;
                    if (CanChangeTurn(requestedPlayerIndex))
                    {
                        var matchSystem = game.GetAspect<MatchSystem>();
                        matchSystem.ChangeTurn();
                    }
                    return;
                case EventCodes.OnTurnChange:
                    var targetPlayerIndex = (byte)photonEvent.CustomData;
                    StartCoroutine(ChangeTurnDisplay(targetPlayerIndex));
                    return;
            }
        }

        public void ChangeTurnButtonPressed()
        {
            var myTurnIndex = (byte)PhotonNetwork.LocalPlayer.CustomProperties["TurnIndex"];
            var raiseEventoptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient };
            var sendOptions = new SendOptions { Reliability = true };
            PhotonNetwork.RaiseEvent(EventCodes.OnChangeTurnRequest, myTurnIndex, raiseEventoptions, sendOptions);
        }

        private bool CanChangeTurn(byte requestedPlayerIndex)
        {
            var stateMachine = game.GetAspect<StateMachine>();

            if (!(stateMachine.CurrentState is PlayerIdleState)) { return false; }

            var currentPlayerIndex = game.GetMatch().CurrentPlayerIndex;

            if (requestedPlayerIndex != currentPlayerIndex) { return false; }

            return true;
        }

        private IEnumerator ChangeTurnViewer(IContainer game, GameAction action)
        {
            var dataSystem = game.GetAspect<DataSystem>();
            var changeTurnAction = action as ChangeTurnAction;
            var targetPlayerIndex = dataSystem.Match.Players[changeTurnAction.TargetPlayerIndex].Index;

            yield return StartCoroutine(ChangeTurnDisplay(targetPlayerIndex));
        }

        private IEnumerator ChangeTurnDisplay(byte targetPlayerIndex)
        {
            var banner = ShowBanner(targetPlayerIndex);
            var button = FlipButton(targetPlayerIndex);

            bool isAnimating;
            do
            {
                var isBannerOn = banner.MoveNext();
                var isButtonOn = button.MoveNext();
                isAnimating = isBannerOn || isButtonOn;
                yield return null;
            }
            while (isAnimating);
        }

        private IEnumerator ShowBanner(byte targetPlayerIndex)
        {
            var myTurnIndex = (byte)PhotonNetwork.LocalPlayer.CustomProperties["TurnIndex"];
            if (myTurnIndex != targetPlayerIndex) { yield break; }

            var sequence = DOTween.Sequence()
                .Append(yourTurnBanner.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack))
                .AppendInterval(1f)
                .Append(yourTurnBanner.DOScale(new Vector3(0f, 1f, 0f), 0.5f).SetEase(Ease.InBack));

            while (sequence.IsActive()) { yield return null; }
        }

        private IEnumerator FlipButton(byte targetPlayerIndex)
        {
            var targetRotation = GetTargetButtonRotation(targetPlayerIndex);
            float rotation = targetRotation == 0f ? 180f : 0f;

            var tweener = DOTween.To(() => rotation, x => rotation = x, targetRotation, 1f)
                .SetEase(Ease.OutBack)
                .OnUpdate(() => buttonView.RotationHandle.transform.eulerAngles = new Vector3(rotation, 0f, 0f));

            while (tweener.IsActive()) { yield return null; }
        }

        private float GetTargetButtonRotation(byte targetPlayerIndex)
        {
            var myTurnIndex = (byte)PhotonNetwork.LocalPlayer.CustomProperties["TurnIndex"];
            return targetPlayerIndex == myTurnIndex ? 0 : 180;
        }
    }
}
