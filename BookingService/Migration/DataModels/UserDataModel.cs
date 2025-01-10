namespace Migration.DataModels
{
    internal class UserDataModel
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public List<ApartmentDataModel> Apartments { get; set; }
    }
}
