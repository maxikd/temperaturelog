using System;

namespace WebAPI.Entities
{
    public class Temperature
    {
        public Temperature()
        {
            Id = Guid.NewGuid().ToString();
        }
        public Temperature(string id)
        {
            Id = id;
        }

        public string Id { get; private set; }
        public float Value { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool Deleted { get; set; }

        public string CityId { get; set; }
    }
}
