using Api.Data;
using Api.Data.Models;

namespace Api
{
    public class Seed
    {
        private readonly DataContext _context;

        public Seed(DataContext context)
        {
            _context = context;
        }

        public void Invoke()
        {
            var pets = new[]
            {
                new Pet { Name = "Dog" },
                new Pet { Name = "Bird" },
                new Pet { Name = "Cat" },
            };

            _context.Pets.AddRange(pets);
            _context.SaveChanges();
        }
    }
}
