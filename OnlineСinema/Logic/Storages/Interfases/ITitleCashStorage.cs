namespace OnlineСinema.Logic.Storages.Interfases
{
    public interface ITitleCashStorage
    {
        public Task<string> ConvertToMp4(Guid id, string path, bool userGpu);

        public Task<string> ConvertToMp4InBackground(Guid id, string path, bool userGpu);

        public string? GetFilePathFromCash(Guid id);

        public Task<string> GetOrAdd(Guid id, string path, bool userGpu);
    }
}
