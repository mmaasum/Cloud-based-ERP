namespace Cloud_based_ERP.Services
{
    public interface ILogisticsService
    {
        Task NotifyOrderAsync(int orderId);
    }
}
