namespace Medical.Models
{
    public class ApplicationUserVM
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string NationalId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string ImageUrl { get; set; }
        public string QrCodeUrl { get; set; }
    }

}
