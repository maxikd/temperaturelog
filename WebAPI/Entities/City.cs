using System;

namespace WebAPI.Entities
{
    public class City
    {
        public City()
        {
            Id = Guid.NewGuid().ToString();
        }
        public City(string id)
        {
            Id = id;
        }

        public string Id { get; private set; }
        public string Name { get; set; }
        public string FederativeUnity { get; set; }
        public string PostalCode { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool Deleted { get; set; }
    }
}
