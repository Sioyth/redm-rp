using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace Trinita.Core.Native
{
    public class Player : Ped
    {
        public Player(int id) => _id = id;
        public Player() => _id = API.PlayerPedId();

        public async void Spawn(Vector3 spawnLocation, string model = "player_one")
        {
            int hash = API.GetHashKey(model);
            if (await LoadModel((uint)hash)) Debug.WriteLine("Loaded sucessfull");
            else Debug.WriteLine("[ERROR] Couldn't load model");

            API.SetPlayerModel(API.PlayerId(), hash, 0);
            API.SetModelAsNoLongerNeeded((uint)hash);

            API.N_0x283978a15512b2fe(_id, 1);
            API.RequestCollisionAtCoord(spawnLocation.X, spawnLocation.Y, spawnLocation.Z);
            API.SetEntityCoordsNoOffset(_id, spawnLocation.X, spawnLocation.Y, spawnLocation.Z, false, false, false);
            API.ClearPedTasksImmediately(_id, 0, 0);
            API.RemoveAllPedWeapons(_id, false, false);
            API.ClearPlayerWantedLevel(API.PlayerId());

            int time = API.GetGameTimer();
            while (!API.HasCollisionLoadedAroundEntity(_id) && (API.GetGameTimer() - time) < 5000)
            {
                await BaseScript.Delay(1);
            }
        }

        new public void Freeze(bool freeze)
        {
            API.SetPlayerControl(API.PlayerId(), 1, 0, 0);
            if (!freeze)
            {
                if (!API.IsEntityVisible(_id)) API.SetEntityVisible(_id, true);
                if (!API.IsPedInAnyVehicle(_id, true)) API.SetEntityCollision(_id, true, true);
                API.FreezeEntityPosition(_id, false);
                API.SetPlayerInvincible(API.PlayerId(), false);
            }
            else
            {
                if (API.IsEntityVisible(_id)) API.SetEntityVisible(_id, false);
                API.SetEntityCollision(_id, false, true);
                API.FreezeEntityPosition(_id, true);
                API.SetPlayerInvincible(API.PlayerId(), true);
                if (API.IsPedFatallyInjured(_id)) API.ClearPedTasksImmediately(_id, 0, 0);
            }
        }
    }
}
