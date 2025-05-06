namespace AttendanceManagement.Input_Models
{
    public class MessageNotFoundException : Exception
    {
        public MessageNotFoundException() { }
        public MessageNotFoundException(string message) : base(message) { }
        public MessageNotFoundException(string message, Exception inner) : base(message, inner) { }
    }
}