namespace dotnet_core_MVC.Models
{
    public class UserMessage
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
        public DateTime SentAt { get; set; }
    }
}
