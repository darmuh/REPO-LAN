using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using Steamworks;
using System.Collections;
using UnityEngine;


namespace REPOLAN
{

    [HarmonyPatch(typeof(MenuPageMain), "ButtonEventJoinGame")]
    internal class MainMenuPatch
    {

        internal static bool Prefix()
        {
            MenuStuff.JoinPrompt.OpenPage(true);
            return false;

        }
    }

    [HarmonyPatch(typeof(MenuPageMain), "ButtonEventSinglePlayer")]
    internal class MainMenuPatch2
    {
        internal static bool Prefix()
        {
            NetworkStart.LocalLobby = true;
            MenuPageMain.instance.ButtonEventHostGame();
            return false;

        }
    }

    [HarmonyPatch(typeof(NetworkConnect), "Start")]
    internal class NetworkStart
    {
        internal static bool LocalLobby = false;
        internal static bool Prefix(NetworkConnect __instance)
        {
            if (!LocalLobby)
                return true;

            PhotonNetwork.NickName = SteamClient.Name;
            PhotonNetwork.AutomaticallySyncScene = false;
            PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "";
            Object.Instantiate(__instance.punVoiceClient, Vector3.zero, Quaternion.identity);
            PhotonNetwork.Disconnect();
            __instance.StartCoroutine(CreateLobbyFix(__instance));
            LocalLobby = false;
            return false;
        }

        private static IEnumerator CreateLobbyFix(NetworkConnect __instance)
        {
            while (PhotonNetwork.NetworkingClient.State != ClientState.Disconnected && PhotonNetwork.NetworkingClient.State != 0)
            {
                yield return null;
            }
            
            RunManager.instance.ResetProgress();     
            Plugin.Log.LogMessage("Created lobby on Network Connect. (LANFIX)");
            __instance.RoomName = SteamClient.Name + " LAN";
            PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime = "60239a4e-9ccd-4f51-81f0-cb6c08096adc";

            PhotonNetwork.ConnectUsingSettings();
        }
    }
    //ButtonEventSinglePlayer - MenuPageMain
    //MenuActionSingleplayerGame - SemiFunc

        /*
        [HarmonyPatch(typeof(RunManager), "ChangeLevel")]
        internal static class HostPatch
        {
            internal static void Postfix(RunManager __instance)
            {
                if (__instance.levelCurrent != __instance.levelLobbyMenu || __instance.levelCurrent != __instance.levelLobby)
                    return;


                string lobbyIDpath = Path.Combine(@"%userprofile%\appdata\locallow\semiwork\Repo", "lobbystuff") + "\\lobbyID.txt";
                lobbyIDpath = Environment.ExpandEnvironmentVariables(lobbyIDpath);
                File.WriteAllText(lobbyIDpath, $"{SteamManager.instance.currentLobby.Id.Value}");
                Plugin.Log.LogMessage($"Updating lobbyID to {SteamManager.instance.currentLobby.Id.Value}");
            }
        }*/
}