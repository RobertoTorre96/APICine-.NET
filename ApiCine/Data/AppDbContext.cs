using Microsoft.EntityFrameworkCore;

namespace ApiCine.Data {
    public class AppDbContext :DbContext  {

        public AppDbContext (DbContextOptions<AppDbContext> options) : base(options) {   }
    }
}
