namespace WebQ
{
    public interface IDiskCache
    {
        void Save(string data, string fileName);
        string Read(string fileName);
    }
}
