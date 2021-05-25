
using CitizenFX.Core;
using CitizenFX.Core.Native;
using System.Threading.Tasks;

namespace Trinita.Core.Native
{
    public class Entity
    {
        public static async Task<bool> LoadModel(string model)
        {
            uint hash = (uint)API.GetHashKey(model);
            if (!Function.Call<bool>(Hash.IS_MODEL_VALID, hash)) return false;

            Function.Call(Hash.REQUEST_MODEL, hash);
            while (!Function.Call<bool>(Hash.HAS_MODEL_LOADED, hash))
            {
                await BaseScript.Delay(200);
            }

            return true;
        }

        public static async Task<bool> LoadModel(uint hash)
        {
            if (!Function.Call<bool>(Hash.IS_MODEL_VALID, hash)) return false;

            Function.Call(Hash.REQUEST_MODEL, hash);
            while (!Function.Call<bool>(Hash.HAS_MODEL_LOADED, hash))
            {
                await BaseScript.Delay(200);
            }

            return true;
        }
    }
}
