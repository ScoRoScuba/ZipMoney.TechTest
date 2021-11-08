namespace ZipPay.API.Model
{
    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public double MonthlySalary { get; set; }
        public double MonthlyExpenses { get; set; }

    }
}
