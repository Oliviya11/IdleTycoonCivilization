using Assets.Scripts.Data;

namespace Assets.Scripts.Services.PersistentProgress
{
    public interface IPersistentProgressService : IService
    {
        PlayerProgress Progress { get; set; }
    }
}
