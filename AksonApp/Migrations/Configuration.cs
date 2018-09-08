namespace AksonApp.Migrations
{
    using AksonApp.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AksonApp.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "AksonApp.Models.ApplicationDbContext";
        }

        protected override void Seed(AksonApp.Models.ApplicationDbContext context)
        {
            using(Models.ApplicationDbContext db = new Models.ApplicationDbContext())
            {
                string[] service = { "Brake fluid flush overdue",
                                     "Front brake pads due",
                                     "Rear brake pads due",
                                     "Micro filter due",
                                     "Statutory emmissions due",
                                     "Vehicle check service due",
                                     "Service overdue" };

                bool checkIfExits = db.ServiceTypes.Any();
                if (!checkIfExits)
                {
                    for (int i = 0; i <= service.Length - 1; i++)
                    {
                        ServiceTypes serviceTypes = new ServiceTypes()
                        {
                            Name = service[i]
                        };
                        db.ServiceTypes.Add(serviceTypes);
                        db.SaveChanges();
                    }      
                }
            }
        }
    }
}
