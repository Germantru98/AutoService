namespace AutoService.DAL.Entities
{
    public class ContactData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public bool isActive { get; set; }
    }
}