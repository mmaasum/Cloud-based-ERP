using Cloud_based_ERP.Models;

namespace Cloud_based_ERP.Validators
{
    public static class OrderValidator
    {
        public static List<string> Validate(OrderRequest order)
        {
            var errors = new List<string>();

            if (order.Customer == null)
                errors.Add("Customer info is required.");

            if (string.IsNullOrWhiteSpace(order.Customer?.Name))
                errors.Add("Customer name is required.");

            if (!IsValidEmail(order.Customer?.Email))
                errors.Add("Invalid customer email.");

            if (order.Items == null || !order.Items.Any())
                errors.Add("At least one order item is required.");
            else
            {
                foreach (var item in order.Items)
                {
                    if (item.Quantity <= 0) errors.Add("Quantity must be > 0");
                    if (item.UnitPrice <= 0) errors.Add("UnitPrice must be > 0");
                }
            }

            return errors;
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }

}
