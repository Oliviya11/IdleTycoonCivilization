using Assets.Scripts.Data;

namespace Assets.Scripts.Services.PersistentProgress
{
    internal class PersistentProgressService : IPersistentProgressService
    {
        public PlayerProgress Progress { get; set; }
    }
}
