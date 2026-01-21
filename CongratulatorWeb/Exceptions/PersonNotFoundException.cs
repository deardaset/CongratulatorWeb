namespace CongratulatorWeb.Exceptions
{
    public class PersonNotFoundException : CongratulatorException
    {
        public PersonNotFoundException(string message) : base(message) 
        {
            StatusCode = 404;
        }        
    }
}
