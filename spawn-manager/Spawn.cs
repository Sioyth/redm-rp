using System;
using System.IO;
using CitizenFX.Core;
using Newtonsoft.Json;
using CitizenFX.Core.Native;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpawnManager
{

    public class Spawn : ClientScript
    {
        //private readonly MySqlDatabase _db;
        private static bool _spawnLock = false;

        public Spawn()
        {
            Tick += SpawnTick;
        }

        private static void FreezePlayer(int id, bool freeze)
        {
            int ped = API.GetPlayerPed(id);

            //True when player was visible and frozen
            var visibility = freeze && API.IsEntityVisible(ped);

            API.SetPlayerControl(id, 0, 0, 0);
            API.SetEntityVisible(ped, visibility);
            API.SetEntityCollision(ped, !freeze, true);
            API.FreezeEntityPosition(ped, freeze);
            API.SetPlayerInvincible(ped, freeze);

            if (API.IsPedFatallyInjured(ped))
            {
                API.ClearPedTasksImmediately(ped, 0, 0);
            }
        }
        private async void SpawnPlayer()
        {
            Debug.WriteLine("DOOOOOM!");
            //string json = File.ReadAllText("spawnpoints.json");
            //List<Vector3> spawnPoints = JsonConvert.DeserializeObject<List<Vector3>>(json);
            Vector3 spawnPoint = new Vector3(-262.849f, 793.404f, 118.087f); 
            if (_spawnLock) return;

            _spawnLock = true;

            API.DoScreenFadeOut(500);

            while (!API.IsScreenFadingOut())
            {
                await Delay(1);
            }

            int ped = API.GetPlayerPed(-1);
            FreezePlayer(API.PlayerId(), true);

            // temporary
            int hash = API.GetHashKey("U_M_M_NbxBoatTicketSeller_01");
            if(await LoadModel(hash)) Debug.WriteLine("Loaded sucessfull");

            API.SetPlayerModel(API.PlayerId(), hash, 0);
            API.SetModelAsNoLongerNeeded((uint)hash);
            API.N_0x283978a15512b2fe(ped, 1);

            API.RequestCollisionAtCoord(spawnPoint.X, spawnPoint.Y, spawnPoint.Z);
            API.SetEntityCoordsNoOffset(ped, spawnPoint.X, spawnPoint.Y, spawnPoint.Z, false, false, false);
            API.NetworkResurrectLocalPlayer((int)spawnPoint.X, (int)spawnPoint.Y, (int)spawnPoint.Z, 250, 1, 1, 0, 0);
            API.ClearPedTasksImmediately(ped, 0, 0);
            API.RemoveAllPedWeapons(ped, false, false);
            API.ClearPlayerWantedLevel(API.PlayerId());

            int time = API.GetGameTimer();
            while (!API.HasCollisionLoadedAroundEntity(ped) && (API.GetGameTimer() - time) < 5000)
            {
                Debug.WriteLine("Loop 1");
                await Delay(1);
            }

            API.ShutdownLoadingScreen();
            

            if(API.IsScreenFadedOut()) API.DoScreenFadeIn(500);
            while (!API.IsScreenFadingIn())
            {
                Debug.WriteLine("Loop 2");
                await Delay(1);
            }

            FreezePlayer(API.PlayerId(), false);
            _spawnLock = false;
        }

        private async Task<bool> LoadModel(int hash)
        {
            uint h = (uint)hash;
            if (!API.IsModelValid(h)) return false;

            API.RequestModel(h, false);
            while (!API.HasModelLoaded(h))
            {
                //API.RequestModel(hash, false);
                await Delay(1);
            }

            return true;
        }

        bool forceRespawn = true;
        int diedAt = 0;
        private async Task SpawnTick()
        {
            await Delay(50);

            int ped = API.GetPlayerPed(-1);
            if (API.IsEntityDead(ped)) diedAt = API.GetGameTimer();
            else diedAt = 0;

            if (forceRespawn)
            {
                Debug.WriteLine("SPAWNTICK");
                SpawnPlayer();
                forceRespawn = false;
            }
            //if playerPed and playerPed ~= -1 then
            //-- check if we want to autospawn
            //if autoSpawnEnabled then
            //    if NetworkIsPlayerActive(PlayerId()) then
            //        if (diedAt and(math.abs(GetTimeDifference(GetGameTimer(), diedAt)) > 2000)) or respawnForced then
            //            if autoSpawnCallback then
            //                autoSpawnCallback()
            //            else
            //    --spawnPlayer()
            //                Citizen.Trace("Testing!")
            //            end

            //            respawnForced = false
            //        end
            //    end
            //end

            //if IsEntityDead(playerPed) then
            //    if not diedAt then
            //        diedAt = GetGameTimer()
            //    end
            //else
            //    diedAt = nil
            //end
        }

    }
}
