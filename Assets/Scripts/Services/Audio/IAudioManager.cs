namespace Assets.Scripts.Services.Audio
{
    public interface IAudioManager : IService
    {
        void PlayCoinsSound();

        void PlayPopupOpened();

        void MuteBackgroundMusic();

        void UnmuteBackgroundMusic();

        void MuteEffects();

        void UnmuteEffects();
    }
}
