using MenuLib;
using Steamworks;
using System.IO;
using System;
using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace REPOLAN
{
    internal class MenuStuff
    {
        internal static REPOSimplePage JoinPrompt = new("Join Game", JoinPromptSetup);

        private static void JoinPromptSetup(REPOSimplePage popup)
        {
            popup.SetText("Join Online or LAN?");
            popup.AddElementToPage(new REPOButton("Online", () => SteamManager.instance.OpenSteamOverlayToLobby()), new Vector2( 200f, 200f));
            popup.AddElementToPage(new REPOButton("LAN", JoinLAN), new Vector2(200f, 250f));
        }

        private static void JoinLAN()
        {
            MainMenuOpen.instance.NetworkConnect();
            RunManager.instance.ResetProgress();

            //PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "usw";
            //PhotonNetwork.PhotonServerSettings.AppSettings.UseNameServer = true;
            //PhotonNetwork.PhotonServerSettings.AppSettings.Protocol = ExitGames.Client.Photon.ConnectionProtocol.Udp;
            PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime = "60239a4e-9ccd-4f51-81f0-cb6c08096adc";
            NetworkConnect.instance.RoomName = SteamClient.Name + " LAN";
            PhotonNetwork.ConnectUsingSettings();

            GameManager.instance.localTest = false;
            RunManager.instance.ResetProgress();
            RunManager.instance.waitToChangeScene = true;
            RunManager.instance.lobbyJoin = true;
            RunManager.instance.ChangeLevel(_completedLevel: true, _levelFailed: false, RunManager.ChangeLevelType.LobbyMenu);
            //SteamManager.instance.joinLobby = false;
        }

        // --- Old code --- //

        //internal static REPOButton HostLanMenuButton = new("HOST LAN", HostLan);
        internal static REPOButton JoinLanMenuButton = new("JOIN SELF", JoinLan);
        internal static TMP_InputField inputField = null!;

        internal static void Init()
        {
            //MenuAPI.AddElementToMainMenu(HostLanMenuButton, new Vector2(145f, 225f));
            MenuAPI.AddElementToMainMenu(JoinLanMenuButton, new Vector2(145f, 190f));  
        }

        internal static void SetupPrompt(REPOPopupPage popup)
        {
            popup.SetLocalPosition(new Vector2(200, 50));
            popup.SetBackgroundDimming(true);
            //popup.panelSize = new Vector2(300, 300);
            popup.AddElementToPage(new REPOButton("Yes", ConfirmJoin), new Vector2(200f, 70f));
            popup.AddElementToPage(new REPOButton("No", () => popup.ClosePage(true)), new Vector2(250f, 70f));
            
        }

        internal static void ConfirmJoin()
        {
            string lobbyIDpath = Path.Combine(@"%userprofile%\appdata\locallow\semiwork\Repo", "lobbystuff");
            lobbyIDpath = Environment.ExpandEnvironmentVariables(lobbyIDpath);
            if (!Directory.Exists(lobbyIDpath))
                Directory.CreateDirectory(lobbyIDpath);

            if (!File.Exists(lobbyIDpath + "\\lobbyID.txt"))
                Plugin.Log.LogWarning("LobbyID could not be obtained!");
            else
            {
                string lobbyIDText = File.ReadAllText(lobbyIDpath + "\\lobbyID.txt");
                Plugin.Log.LogMessage($"Attempting to join lobby at {lobbyIDText}");
                ulong lobbyID = ulong.Parse(lobbyIDText);
                SteamMatchmaking.JoinLobbyAsync(lobbyID);
            }
        }

        internal static void JoinLan()
        {
            ConfirmJoin();
        }
    }
}
