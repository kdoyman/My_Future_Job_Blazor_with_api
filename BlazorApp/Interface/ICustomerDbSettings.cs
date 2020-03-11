namespace BlazorApp.Interface
{
   public interface ICustomerDbSettings
    {
        string CollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
