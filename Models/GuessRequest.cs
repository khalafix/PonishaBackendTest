namespace Ponisha.Models
{
    public class GuessRequest
    {
        public string Secret { get; set; }           // رمز اصلی (مثلاً "4271")
        public List<string> Guesses { get; set; }     // لیست حدس‌ها (مثلاً ["1234", "5671"])
    }
}
