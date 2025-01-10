namespace Migration.DataModels
{
    internal class ApartmentDataModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Address { get; set; }
        public int RoomNumber { get; set; }
    }
}