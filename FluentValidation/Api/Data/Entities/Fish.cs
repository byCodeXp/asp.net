using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Data.Entities
{
    public class Fish
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public double Weight { get; set; }
    }
}
