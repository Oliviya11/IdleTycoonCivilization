using Assets.Scripts.Data;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.Services.SaveLoad
{
    public static class SaveLoadSystem
    {
        private static string path = Application.persistentDataPath + "/save.json";

        public static void SaveGame(PlayerProgress data)
        {
            if (data == null) return;

            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(path, json);
        }

        public static PlayerProgress LoadGame()
        {
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                return JsonUtility.FromJson<PlayerProgress>(json);
            }
            return new PlayerProgress();
        }

        public static bool SaveExists()
        {
            return File.Exists(path);
        }
    }

}
