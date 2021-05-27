using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace Trinita.Core.Native
{
    public class Player
    {
        private int _id;
        private Ped _ped;

        public Ped Ped { get => _ped; set => _ped = value; }

        public Player() 
        {
            _id = API.PlayerId();
            _ped = new Ped(API.PlayerPedId());
        }

        public async void Spawn(Vector3 spawnLocation, string model = "player_one")
        {
            int hash = API.GetHashKey(model);
            if (await _ped.LoadModel((uint)hash)) Debug.WriteLine("Loaded sucessfull");
            else Debug.WriteLine("[ERROR] Couldn't load model");

            API.SetPlayerModel(API.PlayerId(), hash, 0);
            API.SetModelAsNoLongerNeeded((uint)hash);

            API.N_0x283978a15512b2fe(_ped.ID, 1);
            API.RequestCollisionAtCoord(spawnLocation.X, spawnLocation.Y, spawnLocation.Z);
            API.SetEntityCoordsNoOffset(_ped.ID, spawnLocation.X, spawnLocation.Y, spawnLocation.Z, false, false, false);
            API.ClearPedTasksImmediately(_ped.ID, 0, 0);
            API.RemoveAllPedWeapons(_ped.ID, false, false);
            API.ClearPlayerWantedLevel(API.PlayerId());

            int time = API.GetGameTimer();
            while (!API.HasCollisionLoadedAroundEntity(_ped.ID) && (API.GetGameTimer() - time) < 5000)
            {
                await BaseScript.Delay(1);
            }
        }

        public void Teleport(Vector3 pos)
        {
            API.SetEntityCoordsNoOffset(_ped.ID, pos.X, pos.Y, pos.Z, false, false, false);
        }

        public void SetInvincible(bool invincible)
        {
            API.SetPlayerInvincible(_id, invincible);
            Debug.WriteLine("[Invincible]", $"{(invincible ? "On" : "Off")}");
        }

        public void Freeze(bool freeze)
        {
            API.SetPlayerControl(API.PlayerId(), 1, 0, 0);
            if (!freeze)
            {
                if (!API.IsEntityVisible(_ped.ID)) API.SetEntityVisible(_ped.ID, true);
                if (!API.IsPedInAnyVehicle(_ped.ID, true)) API.SetEntityCollision(_ped.ID, true, true);
                API.FreezeEntityPosition(_ped.ID, false);
                API.SetPlayerInvincible(API.PlayerId(), false);
            }
            else
            {
                if (API.IsEntityVisible(_ped.ID)) API.SetEntityVisible(_ped.ID, false);
                API.SetEntityCollision(_ped.ID, false, true);
                API.FreezeEntityPosition(_ped.ID, true);
                API.SetPlayerInvincible(API.PlayerId(), true);
                if (API.IsPedFatallyInjured(_ped.ID)) API.ClearPedTasksImmediately(_ped.ID, 0, 0);
            }
        }
    }
}
