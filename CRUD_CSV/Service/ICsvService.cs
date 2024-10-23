namespace CRUD_CSV.Service
{
    public interface ICsvService
    {
        void Create(string data);
        void Read();
        void Update(int id, string newData);
        void Delete(int id);
    }
}
