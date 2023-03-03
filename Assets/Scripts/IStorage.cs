public interface IStorage
{
    SavedGameState GetState();
    void Save();
}
