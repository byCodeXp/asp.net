using System;

namespace Api.Models.Entities
{
    public class FishDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public double Weight { get; set; }
    }
}
