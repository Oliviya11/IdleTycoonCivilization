using Assets.Scripts.Data;

namespace Assets.Scripts.Services.PersistentProgress
{
    public interface ISavedProgressReader
    {
        void LoadProgress(PlayerProgress progress);
    }
}
