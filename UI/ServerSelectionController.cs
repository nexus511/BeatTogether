﻿using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using BeatSaberMarkupLanguage.Components.Settings;
using BeatSaberMarkupLanguage.Parser;
using BeatTogether.Models.Interfaces;
using BeatTogether.Providers;
using IPA.Utilities;
using MasterServer;
using TMPro;
using UnityEngine;

namespace BeatTogether.UI
{
    internal class ServerSelectionController : MonoBehaviour
    {
        private static readonly string STATUS_TEXT_UNKNOWN = "Status: <color=\"yellow\">UNKNOWN";
        private static readonly string STATUS_TEXT_OFFLINE = "Status: <color=\"red\">OFFLINE";
        private static readonly string STATUS_TEXT_ONLINE = "Status: <color=\"green\">ONLINE";
        private static readonly string STATUS_TEXT_MAINTENANCE = "Status: <color=\"yellow\">MAINTENANCE UPCOMING";

        private static MethodInfo _unauthenticateWithMasterServerMethodInfo = typeof(UserMessageHandler)
            .GetMethod("UnauthenticateWithMasterServer", BindingFlags.Instance | BindingFlags.NonPublic);

        private MultiplayerModeSelectionViewController _multiplayerView;

        private ListSetting _serverSelection;

        public void Init(ListSetting serverSelection, MultiplayerModeSelectionViewController multiplayerView)
        {
            _multiplayerView = multiplayerView;
            _serverSelection = serverSelection;

            var changed = new BSMLAction(this, typeof(ServerSelectionController).GetMethod("OnServerChanged"));
            serverSelection.onChange = changed;
            UpdateUI(multiplayerView, BeatTogetherCore.instance.SelectedServer);

            var provider = BeatTogetherCore.instance.ServerDetailProvider as ServerDetailProvider;
            provider.ServerListChanged += OnServerListChanged;
            provider.UpdateServerSelectionInt += OnServerSelectionInt;
            GameEventDispatcher.Instance.MultiplayerViewEntered += OnMultiplayerViewEntered;
        }

        public void OnDestroy()
        {
            var provider = BeatTogetherCore.instance.ServerDetailProvider as ServerDetailProvider;
            provider.UpdateServerSelectionInt -= OnServerSelectionInt;
            provider.ServerListChanged -= OnServerListChanged;
            GameEventDispatcher.Instance.MultiplayerViewEntered -= OnMultiplayerViewEntered;
        }

        public void OnServerChanged(object selection)
        {
            var details = selection as IServerDetails;
            var detailsProvider = BeatTogetherCore.instance.ServerDetailProvider as ServerDetailProvider;
            detailsProvider?.OnSetSelectedServer(this, details);

            // Keep this code, as it informs MPEX of the change
            // (by invoking the getters):
            NotifyMpex();

            DisconnectServer();
            UpdateUI(_multiplayerView, details);
        }

        #region Private Methods

        private void NotifyMpex()
        {
            var networkConfig = GetNetworkConfig();
            var endPoint = networkConfig.masterServerEndPoint;
            var statusUrl = networkConfig.masterServerStatusUrl;
            Plugin.Logger.Debug(
                "Master server selection changed " +
                $"(EndPoint={endPoint}, StatusUrl={statusUrl})"
            );
        }

        private void DisconnectServer()
        {
            var provider = BeatTogetherCore.instance.InstanceProvider;
            var handler = provider?.UserMessageHandler;
            if (handler == null)
                return;
            _unauthenticateWithMasterServerMethodInfo.Invoke(handler, new object[] { });
        }

        private void UpdateUI(MultiplayerModeSelectionViewController multiplayerView, IServerDetails details)
        {
            if (multiplayerView == null)
                return;

            var transform = _multiplayerView.transform;
            var quickPlayButton = transform.Find("Buttons/QuickPlayButton").gameObject;
            quickPlayButton.SetActive(details.IsOfficial);

            var statusProvider = BeatTogetherCore.instance.StatusProvider;
            var status = statusProvider?.GetServerStatus(details);
            multiplayerView.SetData(status);

            var textMesh = GetMaintenanceMessageText();
            if (textMesh.gameObject.activeSelf)
            {
                textMesh.richText = false;
                return;
            }

            textMesh.richText = true;
            if (status == null)
            {
                textMesh.SetText(STATUS_TEXT_UNKNOWN);
                textMesh.gameObject.SetActive(true);
                return;
            }

            switch (status.status)
            {
                case MasterServerAvailabilityData.AvailabilityStatus.Offline:
                    textMesh.SetText(STATUS_TEXT_OFFLINE);
                    break;
                case MasterServerAvailabilityData.AvailabilityStatus.MaintenanceUpcoming:
                    // should not be reached, as method returns, if basegame already handled the message
                    textMesh.SetText(STATUS_TEXT_MAINTENANCE);
                    break;
                case MasterServerAvailabilityData.AvailabilityStatus.Online:
                    textMesh.SetText(STATUS_TEXT_ONLINE);
                    break;
            }
            textMesh.gameObject.SetActive(true);
        }

        private INetworkConfig GetNetworkConfig() =>
            ReflectionUtil.GetField<INetworkConfig, MultiplayerModeSelectionViewController>(_multiplayerView, "_networkConfig");

        private TextMeshProUGUI GetMaintenanceMessageText() =>
            ReflectionUtil.GetField<TextMeshProUGUI, MultiplayerModeSelectionViewController>(_multiplayerView, "_maintenanceMessageText");

        private void OnMultiplayerViewEntered(object sender, MultiplayerModeSelectionViewController multiplayerView)
        {
            var selection = BeatTogetherCore.instance.SelectedServer;
            UpdateUI(multiplayerView, selection);
        }

        private void OnServerSelectionInt(object sender, IServerDetails server)
        {
            UpdateUI(_multiplayerView, server);
        }

        private void OnServerListChanged(object sender, List<IServerDetails> serverList)
        {
            if (_serverSelection == null)
                return;

            _serverSelection.values = serverList.ToList<object>();
            var selection = BeatTogetherCore.instance.SelectedServer;
            UpdateUI(_multiplayerView, selection);
        }
        #endregion
    }
}
