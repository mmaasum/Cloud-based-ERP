namespace Cloud_based_ERP.Services
{
    

    public class MockLogisticsService : ILogisticsService
    {
        public async Task NotifyOrderAsync(int orderId)
        {
            await Task.Delay(2000);
            Console.WriteLine($"Order {orderId} sent to logistics.");
        }
    }

}
