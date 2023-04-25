using Domain.Entity;

namespace Domain.Gameplay
{
    public interface IGameplayPresenter
    {
        public void StartGame();
        public void OnGameStarted(Tower tower);
        public void Lose();
        public void OnEnemySpawned(Enemy enemy);
    }
}