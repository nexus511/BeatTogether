using System.Reflection;
using BeatSaberMarkupLanguage.Components.Settings;
using BeatSaberMarkupLanguage.Parser;
using BeatTogether.Models;
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

        public void Init(ListSetting listSetting, MultiplayerModeSelectionViewController multiplayerView)
        {
            _multiplayerView = multiplayerView;
            var changed = new BSMLAction(this, typeof(ServerSelectionController).GetMethod("OnServerChanged"));
            listSetting.onChange = changed;
            UpdateUI(multiplayerView, BeatTogetherCore.instance.SelectedServer);
            GameEventDispatcher.Instance.MultiplayerViewEntered += OnMultiplayerViewEntered;
        }

        public void OnDestroy()
            => GameEventDispatcher.Instance.MultiplayerViewEntered -= OnMultiplayerViewEntered;

        public void OnServerChanged(object selection)
        {
            var details = selection as IServerDetails;
            var detailsProvider = BeatTogetherCore.instance.ServerDetailProvider;
            detailsProvider?.SetSelectedServer(details);

            // Keep this code, as it informs MPEX of the change
            // (by invoking the getters):
            var networkConfig = GetNetworkConfig();
            var endPoint = networkConfig.masterServerEndPoint;
            var statusUrl = networkConfig.masterServerStatusUrl;
            Plugin.Logger.Debug(
                "Master server selection changed " +
                $"(EndPoint={endPoint}, StatusUrl={statusUrl})"
            );

            DisconnectServer();
            UpdateUI(_multiplayerView, details);
        }

        #region Private Methods

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

        #endregion
    }
}
