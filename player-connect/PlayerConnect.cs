using System;
using MySqlLib;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace TrinitaRP
{
    public class PlayerConnect : ServerScript
    {
        private readonly MySqlDatabase _db;

        public PlayerConnect()
        {
            _db = new MySqlDatabase();
            EventHandlers["onResourceStart"] += new Action<string>(OnResourceStart);
            EventHandlers["playerConnecting"] += new Action<Player, string, dynamic, dynamic>(OnPlayerConnecting);
            //EventHandlers["playerDropped"] += new Action<Player, string>(OnPlayerDropped);
        }


        private void OnResourceStart(string resourceName)
        {
            if (API.GetCurrentResourceName() != resourceName) return;

           var task =_db.Connect();
           if (task.Result == false) Debug.WriteLine("[ERROR] Connection to database as failed!");
            _db.Disconnect();
        }

        private async void OnPlayerConnecting([FromSource] Player player, string playerName, dynamic setKickReason, dynamic deferrals)
        {
            deferrals.defer();
            await Delay(0);
            
            string steamid = player.Identifiers["steam"];
            string license = player.Identifiers["license"];
            Debug.WriteLine("STEAMID: " + steamid);
            Debug.WriteLine("LICENSE: " + license);

            if (string.IsNullOrEmpty(steamid)) deferrals.done($"You have been kicked (Reason: Steam is not running!)");

            deferrals.done();
            CheckID(steamid, playerName);
        }

        public async void CheckID(string id, string name)
        {
            await _db.Connect();
            if (await _db.RowCount("players", "ID", id) == 0) _db.InsertData("players", new string[] { "ID", "SteamName" }, new string[] { id, name });
            _db.Disconnect();
        }

        private void OnPlayerDropped(Player player, string reason)
        {
            throw new NotImplementedException();
        }
    }
}
