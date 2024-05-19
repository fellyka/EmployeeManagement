using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Models
{
    public class EmployeeDbContext : IdentityDbContext /*Contains all the DbSet properties needed to manage the identity tables in the underlying data store*/
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options): base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /* 
             *This is another way of seeding data but for this application, we chose to seed from a static class
              and use that class below to avoid that our class to be cluttered. 
            Alternatively, we could just populate the database directly
              
                 modelBuilder.Entity<Employee>().HasData(
                      new Employee
                         { 
                             Id = 1, 
                             Name="Felly KANYIKI", 
                             Department=Dept.It, Email="fellyka@sollers.co.za"
                         }
                     );  */
            base.OnModelCreating(modelBuilder);//This line is needed to facilitate the Migration when using identity bcoz keys are mapped here
             

            //--We rather chose to move thsese seeding codes in the ModelBuilderExtension static class to keep tge EmploueeDbContext class clean --
            ModelBuilderExtension.Seed(modelBuilder);
        }

        public DbSet<Employee>? Employees { get; set;}
    }
}

/*
  How to Keep Domain Models and Database Scheme in sync
  If you had a new property in your Model, add a new migration (Add-Migration MigrationName) and update the database (update-database) to keep the
  Models and the database in the sync.

  The Snapshot.cs file contains the snapshot of the current Models.
  It is created when the first migration is created and updated at every subsequent migration.
  EF Core migration API uses this file to determine what has changed when adding a next migration

  How to remove a migration?
  remove-migration will remove the latest added migration (as long as this migration hasn't been applied to the database) and update the Snapshot.cs class 
  to be in sync with the models.
   -What if the migration has been already applied to the database? => First, use update-database NameOfTheLatestMigrationYouWouldWantToGoBackTo to undo the database changes 
   by the migration(Consult the  __efmigrationshistory table to confirm which migation you'd want to go back to) and then apply remove-migration
 
 */