

using jwtapp.Data;
using Microsoft.EntityFrameworkCore;

public static class DataMigration {

  public static void MigrateDb(this WebApplication app)
  {
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AuthContext>();
    dbContext.Database.Migrate();
  }
  
}
