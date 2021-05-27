using CitizenFX.Core;
using Trinita.Core.Native;
using CitizenFX.Core.Native;
using System.Threading.Tasks;

namespace TrinitaRP
{
    public class SpawnManager : ClientScript
    {
        private Player _player;
        private static bool _spawnLock;

        public SpawnManager()
        {
            _spawnLock = false;
            _player = new Player();
            Tick += SpawnThread;
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

            _player.Freeze(true);
            _player.Spawn(spawnPoint);

            API.ShutdownLoadingScreen();
            

           // if(API.IsScreenFadedOut()) 
                API.DoScreenFadeIn(500);
            //while (!API.IsScreenFadingIn())
            //{
            //    Debug.WriteLine("Loop 2");
            //    await Delay(1);
            //}

            _player.Freeze(false);
            _spawnLock = false;
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


       