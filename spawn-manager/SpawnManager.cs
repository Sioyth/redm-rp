using CitizenFX.Core;
using CitizenFX.Core.Native;
using System.Threading.Tasks;

namespace TrinitaRP
{

    public class SpawnManager : ClientScript
    {
        //private readonly MySqlDatabase _db;
        private static bool _spawnLock = false;

        public SpawnManager()
        {
            Tick += SpawnThread;
        }

        private static void FreezePlayer(int id, bool freeze)
        {
            int ped = API.PlayerPedId();

            //True when player was visible and frozen
            var visibility = freeze && API.IsEntityVisible(ped);

            //API.SetPlayerControl(id, 1, 0, 0);
            //API.SetEntityVisible(ped, visibility);
            //API.SetEntityCollision(ped, !freeze, true);
            //API.FreezeEntityPosition(ped, freeze);
            //API.SetPlayerInvincible(ped, freeze);

            //if (!API.IsPedFatallyInjured(ped))
            //{
            //    API.ClearPedTasksImmediately(ped, 0, 0);
            //}
            
            API.SetPlayerControl(id, 1, 0, 0);
            if (!freeze)
            {
                if (!API.IsEntityVisible(ped))
                    API.SetEntityVisible(ped, true);

                if (!API.IsPedInAnyVehicle(ped, true))
                    API.SetEntityCollision(ped, true, true);

                API.FreezeEntityPosition(ped, false);
                //SetCharNeverTargetted(ped, false)
                API.SetPlayerInvincible(API.PlayerId(), false);
            }
            else
            {
                if (API.IsEntityVisible(ped))
                    API.SetEntityVisible(ped, false);

                API.SetEntityCollision(ped, false, true);
                API.FreezeEntityPosition(ped, true);
                //SetCharNeverTargetted(ped, true)
                API.SetPlayerInvincible(API.PlayerId(), true);

                if (API.IsPedFatallyInjured(ped))
                    API.ClearPedTasksImmediately(ped, 0, 0);
            }
        }
        private async void SpawnPlayer()
        {
            // TODO: Read from database
            //string json = File.ReadAllText("spawnpoints.json");
            //List<Vector3> spawnPoints = JsonConvert.DeserializeObject<List<Vector3>>(json);
            Vector3 spawnPoint = new Vector3(-262.849f, 793.404f, 118.087f); 
            if (_spawnLock) return;

            _spawnLock = true;

            //API.DoScreenFadeOut(500);

            //while (!API.IsScreenFadingOut())
            //{
            //    Debug.WriteLine("ANOTHER LOOOP");
            //    await Delay(1);
            //}

            FreezePlayer(API.PlayerId(), true);

            int hash = API.GetHashKey("RCSP_GUNSLINGERDUEL4_MALES_01");
            if(await LoadModel(hash)) Debug.WriteLine("Loaded sucessfull");

            API.SetPlayerModel(API.PlayerId(), hash, 0);
            API.SetModelAsNoLongerNeeded((uint)hash);

            int ped = API.PlayerPedId();
            API.N_0x283978a15512b2fe(ped, 1);
            API.RequestCollisionAtCoord(spawnPoint.X, spawnPoint.Y, spawnPoint.Z);
            API.SetEntityCoordsNoOffset(ped, spawnPoint.X, spawnPoint.Y, spawnPoint.Z, false, false, false);
            API.ClearPedTasksImmediately(ped, 0, 0);
            API.RemoveAllPedWeapons(ped, false, false);
            API.ClearPlayerWantedLevel(API.PlayerId());

            int time = API.GetGameTimer();
            while (!API.HasCollisionLoadedAroundEntity(ped) && (API.GetGameTimer() - time) < 5000)
            {
                await Delay(1);
            }

            API.ShutdownLoadingScreen();
            

           // if(API.IsScreenFadedOut()) 
                API.DoScreenFadeIn(500);
            //while (!API.IsScreenFadingIn())
            //{
            //    Debug.WriteLine("Loop 2");
            //    await Delay(1);
            //}
            
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
                API.RequestModel(h, false);
                await Delay(1);
            }

            return true;
        }

        //TODO: Change name, respawn when you die.
        bool forceRespawn = true;
        int diedAt = 0;
        private async Task SpawnThread()
        {
            await Delay(50);

            int ped = API.PlayerPedId();
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
