using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Persistence.Seeder
{
    public class Seed
    {
        public static async Task SeedData(
            DataContext context,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager
        )
        {
            if (await context.Users.AnyAsync())
                return;

            // Seed Roles
            var adminRole = new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" };
            var userRole = new IdentityRole { Name = "User", NormalizedName = "USER" };

            await roleManager.CreateAsync(adminRole);
            await roleManager.CreateAsync(userRole);

            // Seed Users
            var adminUser = new User
            {
                UserName = "adminuser",
                Email = "admin@example.com",
                DisplayName = "Admin User",
                EmailConfirmed = true,
            };
            await userManager.CreateAsync(adminUser, "AdminPassword123");
            await userManager.AddToRoleAsync(adminUser, "Admin");

            var user1 = new User
            {
                UserName = "user1",
                Email = "user1@example.com",
                DisplayName = "User One",
                EmailConfirmed = true,
            };
            await userManager.CreateAsync(user1, "UserPassword123");
            await userManager.AddToRoleAsync(user1, "User");

            var user2 = new User
            {
                UserName = "user2",
                Email = "user2@example.com",
                DisplayName = "User Two",
                EmailConfirmed = true,
            };
            await userManager.CreateAsync(user2, "UserPassword123");
            await userManager.AddToRoleAsync(user2, "User");

            // Seed Equipments
            if (!await context.Equipments.AnyAsync())
            {
                var equipments = new[]
                {
                    new Equipment
                    {
                        Id = 1,
                        Name = "Bulldozer D85",
                        Type = "Heavy",
                        Brand = "Komatsu",
                        Model = "D85",
                        YearOfManufacture = "2019",
                        Specification = "Bulldozer for large earth-moving tasks",
                        Description = "Powerful bulldozer ideal for construction",
                        Status = "Available",
                        Location = "Jakarta",
                        RentalPrice = 2500000m,
                        IsDeleted = false,
                        Unit = 3
                    },
                    new Equipment
                    {
                        Id = 2,
                        Name = "Hydraulic Crane ATF",
                        Type = "Heavy",
                        Brand = "Tadano",
                        Model = "ATF220",
                        YearOfManufacture = "2021",
                        Specification = "All-terrain hydraulic crane",
                        Description = "Efficient crane suitable for difficult terrains",
                        Status = "Available",
                        Location = "Surabaya",
                        RentalPrice = 4500000m,
                        IsDeleted = false,
                        Unit = 2
                    },
                    new Equipment
                    {
                        Id = 3,
                        Name = "Dump Truck Quester",
                        Type = "Transport",
                        Brand = "UD Truck",
                        Model = "Quester CWE",
                        YearOfManufacture = "2018",
                        Specification = "Heavy-duty dump truck for construction materials",
                        Description = "Durable truck with large load capacity",
                        Status = "Available",
                        Location = "Semarang",
                        RentalPrice = 3000000m,
                        IsDeleted = false,
                        Unit = 4
                    },
                    new Equipment
                    {
                        Id = 4,
                        Name = "Excavator PC210",
                        Type = "Heavy",
                        Brand = "Komatsu",
                        Model = "PC210",
                        YearOfManufacture = "2022",
                        Specification = "Large excavator for heavy digging",
                        Description = "Powerful and efficient excavator",
                        Status = "Available",
                        Location = "Medan",
                        RentalPrice = 3500000m,
                        IsDeleted = false,
                        Unit = 2
                    },
                    new Equipment
                    {
                        Id = 5,
                        Name = "Compactor Roller",
                        Type = "Road Equipment",
                        Brand = "Bomag",
                        Model = "BW213",
                        YearOfManufacture = "2020",
                        Specification = "Single drum roller for roadworks",
                        Description = "Robust compactor for smooth surface preparation",
                        Status = "Available",
                        Location = "Bandung",
                        RentalPrice = 1500000m,
                        IsDeleted = false,
                        Unit = 5
                    },
                    new Equipment
                    {
                        Id = 6,
                        Name = "Truck Mixer",
                        Type = "Concrete",
                        Brand = "Scania",
                        Model = "P380",
                        YearOfManufacture = "2021",
                        Specification = "Truck mixer for concrete mixing and transport",
                        Description = "Efficient mixing with strong mixing blades",
                        Status = "Available",
                        Location = "Palembang",
                        RentalPrice = 2700000m,
                        IsDeleted = false,
                        Unit = 4
                    },
                    new Equipment
                    {
                        Id = 7,
                        Name = "Asphalt Paver",
                        Type = "Road Equipment",
                        Brand = "Bomag",
                        Model = "BF 300 P",
                        YearOfManufacture = "2020",
                        Specification = "Paver for asphalt road surfaces",
                        Description = "Reliable paver for smooth and even asphalt layering",
                        Status = "Available",
                        Location = "Yogyakarta",
                        RentalPrice = 2500000m,
                        IsDeleted = false,
                        Unit = 3
                    },
                    new Equipment
                    {
                        Id = 8,
                        Name = "Excavator PC200",
                        Type = "Heavy",
                        Brand = "Komatsu",
                        Model = "PC200",
                        YearOfManufacture = "2018",
                        Specification = "Excavator for digging",
                        Description = "Durable excavator suitable for various tasks",
                        Status = "Available",
                        Location = "Pontianak",
                        RentalPrice = 2500000m,
                        IsDeleted = false,
                        Unit = 6
                    },
                    new Equipment
                    {
                        Id = 9,
                        Name = "Crane Tadano ATF 220G",
                        Type = "Heavy",
                        Brand = "Tadano",
                        Model = "ATF220G-5",
                        YearOfManufacture = "2019",
                        Specification = "All-terrain hydraulic crane",
                        Description = "Crane for high lifting capacity",
                        Status = "Available",
                        Location = "Bali",
                        RentalPrice = 5000000m,
                        IsDeleted = false,
                        Unit = 1
                    },
                    new Equipment
                    {
                        Id = 10,
                        Name = "Crawler Dozer",
                        Type = "Heavy",
                        Brand = "Komatsu",
                        Model = "D65PX-18",
                        YearOfManufacture = "2021",
                        Specification = "Crawler dozer for earth moving",
                        Description = "High power and traction",
                        Status = "Available",
                        Location = "Padang",
                        RentalPrice = 4000000m,
                        IsDeleted = false,
                        Unit = 3
                    },
                    new Equipment
                    {
                        Id = 11,
                        Name = "Forklift",
                        Type = "Material Handling",
                        Brand = "Toyota",
                        Model = "8FGU25",
                        YearOfManufacture = "2018",
                        Specification = "Forklift with 2.5 ton capacity",
                        Description = "Compact and reliable forklift",
                        Status = "Available",
                        Location = "Bandung",
                        RentalPrice = 1300000m,
                        IsDeleted = false,
                        Unit = 6
                    },
                    new Equipment
                    {
                        Id = 12,
                        Name = "Backhoe Loader",
                        Type = "Heavy",
                        Brand = "Komatsu",
                        Model = "PC130",
                        YearOfManufacture = "2021",
                        Specification = "Backhoe loader for versatile applications",
                        Description = "Ideal for small construction projects",
                        Status = "Available",
                        Location = "Jakarta",
                        RentalPrice = 2200000m,
                        IsDeleted = false,
                        Unit = 5
                    },
                    new Equipment
                    {
                        Id = 13,
                        Name = "Mini Excavator",
                        Type = "Heavy",
                        Brand = "Komatsu",
                        Model = "PC50",
                        YearOfManufacture = "2020",
                        Specification = "Compact excavator for small jobs",
                        Description = "Easy to maneuver in tight spaces",
                        Status = "Available",
                        Location = "Surabaya",
                        RentalPrice = 1800000m,
                        IsDeleted = false,
                        Unit = 8
                    },
                    new Equipment
                    {
                        Id = 14,
                        Name = "Crawler Crane",
                        Type = "Heavy",
                        Brand = "Hitachi",
                        Model = "SCX800",
                        YearOfManufacture = "2017",
                        Specification = "Crawler crane with high lifting capacity",
                        Description = "Suitable for large construction projects",
                        Status = "Available",
                        Location = "Balikpapan",
                        RentalPrice = 6000000m,
                        IsDeleted = false,
                        Unit = 2
                    },
                    new Equipment
                    {
                        Id = 15,
                        Name = "Road Roller",
                        Type = "Road Equipment",
                        Brand = "Dynapac",
                        Model = "CA2500D",
                        YearOfManufacture = "2019",
                        Specification = "Road roller for compacting surfaces",
                        Description = "Durable roller for road construction",
                        Status = "Available",
                        Location = "Makassar",
                        RentalPrice = 1900000m,
                        IsDeleted = false,
                        Unit = 5
                    },
                    new Equipment
                    {
                        Id = 16,
                        Name = "Wheel Loader",
                        Type = "Heavy",
                        Brand = "Komatsu",
                        Model = "WA200-8",
                        YearOfManufacture = "2020",
                        Specification = "Wheel loader for material handling",
                        Description = "Efficient and durable wheel loader",
                        Status = "Available",
                        Location = "Medan",
                        RentalPrice = 2800000m,
                        IsDeleted = false,
                        Unit = 4
                    },
                    new Equipment
                    {
                        Id = 17,
                        Name = "Articulated Dump Truck",
                        Type = "Heavy",
                        Brand = "Scania",
                        Model = "A40G",
                        YearOfManufacture = "2022",
                        Specification = "Articulated dump truck for rugged terrains",
                        Description = "Strong and flexible truck for transporting heavy materials",
                        Status = "Available",
                        Location = "Balikpapan",
                        RentalPrice = 3200000m,
                        IsDeleted = false,
                        Unit = 2
                    },
                    new Equipment
                    {
                        Id = 18,
                        Name = "Crane Rough Terrain",
                        Type = "Heavy",
                        Brand = "Tadano",
                        Model = "GR-1000EX",
                        YearOfManufacture = "2019",
                        Specification = "Rough terrain crane with high lifting capacity",
                        Description = "Ideal for challenging construction environments",
                        Status = "Available",
                        Location = "Bandung",
                        RentalPrice = 4700000m,
                        IsDeleted = false,
                        Unit = 3
                    },
                    new Equipment
                    {
                        Id = 19,
                        Name = "Pneumatic Tire Roller",
                        Type = "Road Equipment",
                        Brand = "Bomag",
                        Model = "BW 24 RH",
                        YearOfManufacture = "2021",
                        Specification = "Pneumatic tire roller for surface compaction",
                        Description = "High performance roller for smooth surface preparation",
                        Status = "Available",
                        Location = "Makassar",
                        RentalPrice = 2100000m,
                        IsDeleted = false,
                        Unit = 5
                    },
                    new Equipment
                    {
                        Id = 20,
                        Name = "Crawler Loader",
                        Type = "Heavy",
                        Brand = "Komatsu",
                        Model = "D75PX-15R",
                        YearOfManufacture = "2018",
                        Specification = "Crawler loader with powerful loading capabilities",
                        Description = "Ideal for earth-moving and loading operations",
                        Status = "Available",
                        Location = "Semarang",
                        RentalPrice = 3800000m,
                        IsDeleted = false,
                        Unit = 3
                    }
                };
                await context.Equipments.AddRangeAsync(equipments);
                await context.SaveChangesAsync();
            }

            // Seed MaintenancedRecords
            if (!await context.MaintenancedRecords.AnyAsync())
            {
                var maintenances = new[]
                {
                    new MaintenancedRecord
                    {
                        EquipmentId = 1,
                        MaintenanceDate = DateTime.UtcNow.AddMonths(-3),
                        ServicedPerformed = "Engine Check",
                        ServicedProvider = "ServicePro",
                        Cost = 500m,
                        NextMaintenanceDue = DateTime.UtcNow.AddMonths(3),
                        IsDeleted = false
                    },
                    new MaintenancedRecord
                    {
                        EquipmentId = 2,
                        MaintenanceDate = DateTime.UtcNow.AddMonths(-2),
                        ServicedPerformed = "Hydraulics Overhaul",
                        ServicedProvider = "HeavyFix",
                        Cost = 1200m,
                        NextMaintenanceDue = DateTime.UtcNow.AddMonths(4),
                        IsDeleted = false
                    }
                };
                await context.MaintenancedRecords.AddRangeAsync(maintenances);
            }

            // Seed PerformanceFeedbacks
            if (!await context.PerformanceFeedbacks.AnyAsync())
            {
                var feedbacks = new[]
                {
                    new PerformanceFeedback
                    {
                        EquipmentId = 1,
                        UserId = user1.Id,
                        FeedbackDate = DateTime.UtcNow.AddMonths(-1),
                        Rating = 4,
                        Comment = "Great equipment for the job",
                        IsDeleted = false
                    },
                    new PerformanceFeedback
                    {
                        EquipmentId = 2,
                        UserId = user2.Id,
                        FeedbackDate = DateTime.UtcNow.AddMonths(-2),
                        Rating = 5,
                        Comment = "Worked perfectly in tough conditions",
                        IsDeleted = false
                    }
                };
                await context.PerformanceFeedbacks.AddRangeAsync(feedbacks);
            }

            // Seed RentalHistories
            if (!await context.RentalHistories.AnyAsync())
            {
                var rentalHistories = new[]
                {
                    new RentalHistory
                    {
                        EquipmentId = 1,
                        RenterId = user1.Id,
                        Invoice = "RH-001",
                        RentalStartDate = DateTime.UtcNow.AddMonths(-2),
                        RentalEndDate = DateTime.UtcNow.AddMonths(-1),
                        RentalCost = 4500m,
                        Location = "Site A",
                        IsDeleted = false
                    },
                    new RentalHistory
                    {
                        EquipmentId = 2,
                        RenterId = user2.Id,
                        Invoice = "RH-002",
                        RentalStartDate = DateTime.UtcNow.AddMonths(-3),
                        RentalEndDate = DateTime.UtcNow.AddMonths(-2),
                        RentalCost = 6000m,
                        Location = "Site B",
                        IsDeleted = false
                    }
                };
                await context.RentalHistories.AddRangeAsync(rentalHistories);
            }

            // Seed SpareParts
            if (!await context.SpareParts.AnyAsync())
            {
                var spareParts = new[]
                {
                    new SparePart
                    {
                        EquipmentId = 1,
                        PartName = "Hydraulic Pump",
                        PartNumber = "HP-D85",
                        Manufacturer = "Komatsu",
                        AvailabilityStatus = "In Stock",
                        Price = 5500000m,
                        IsDeleted = false,
                        Stock = 10
                    },
                    new SparePart
                    {
                        EquipmentId = 2,
                        PartName = "Crane Hook",
                        PartNumber = "CH-ATF220",
                        Manufacturer = "Tadano",
                        AvailabilityStatus = "Limited",
                        Price = 4000000m,
                        IsDeleted = false,
                        Stock = 6
                    },
                    new SparePart
                    {
                        EquipmentId = 3,
                        PartName = "Truck Tire",
                        PartNumber = "TT-QSTR",
                        Manufacturer = "Bridgestone",
                        AvailabilityStatus = "In Stock",
                        Price = 3500000m,
                        IsDeleted = false,
                        Stock = 20
                    },
                    new SparePart
                    {
                        EquipmentId = 4,
                        PartName = "Excavator Bucket",
                        PartNumber = "EX-BKT-PC210",
                        Manufacturer = "Komatsu",
                        AvailabilityStatus = "Pre-Order",
                        Price = 7500000m,
                        IsDeleted = false,
                        Stock = 5
                    },
                    new SparePart
                    {
                        EquipmentId = 5,
                        PartName = "Compactor Roller Drum",
                        PartNumber = "CRD-BW213",
                        Manufacturer = "Bomag",
                        AvailabilityStatus = "In Stock",
                        Price = 5000000m,
                        IsDeleted = false,
                        Stock = 8
                    },
                    new SparePart
                    {
                        EquipmentId = 6,
                        PartName = "Mixer Drum",
                        PartNumber = "MX-DR-380",
                        Manufacturer = "Scania",
                        AvailabilityStatus = "In Stock",
                        Price = 9200000m,
                        IsDeleted = false,
                        Stock = 6
                    },
                    new SparePart
                    {
                        EquipmentId = 7,
                        PartName = "Paver Conveyor Chain",
                        PartNumber = "PC-BF300",
                        Manufacturer = "Bomag",
                        AvailabilityStatus = "Limited",
                        Price = 4500000m,
                        IsDeleted = false,
                        Stock = 7
                    },
                    new SparePart
                    {
                        EquipmentId = 8,
                        PartName = "Track Chain",
                        PartNumber = "TC-PC200",
                        Manufacturer = "Komatsu",
                        AvailabilityStatus = "In Stock",
                        Price = 7500000m,
                        IsDeleted = false,
                        Stock = 10
                    },
                    new SparePart
                    {
                        EquipmentId = 9,
                        PartName = "Boom Section",
                        PartNumber = "BS-ATF220G",
                        Manufacturer = "Tadano",
                        AvailabilityStatus = "Pre-Order",
                        Price = 12000000m,
                        IsDeleted = false,
                        Stock = 2
                    },
                    new SparePart
                    {
                        EquipmentId = 10,
                        PartName = "Dozer Blade",
                        PartNumber = "DB-D65PX",
                        Manufacturer = "Komatsu",
                        AvailabilityStatus = "In Stock",
                        Price = 6500000m,
                        IsDeleted = false,
                        Stock = 4
                    },
                    new SparePart
                    {
                        EquipmentId = 11,
                        PartName = "Forklift Hydraulic Cylinder",
                        PartNumber = "FHC-8FGU25",
                        Manufacturer = "Toyota",
                        AvailabilityStatus = "In Stock",
                        Price = 3000000m,
                        IsDeleted = false,
                        Stock = 15
                    },
                    new SparePart
                    {
                        EquipmentId = 12,
                        PartName = "Loader Bucket",
                        PartNumber = "LB-PC130",
                        Manufacturer = "Komatsu",
                        AvailabilityStatus = "In Stock",
                        Price = 7000000m,
                        IsDeleted = false,
                        Stock = 6
                    },
                    new SparePart
                    {
                        EquipmentId = 13,
                        PartName = "Mini Excavator Arm",
                        PartNumber = "MEA-PC50",
                        Manufacturer = "Komatsu",
                        AvailabilityStatus = "Limited",
                        Price = 5000000m,
                        IsDeleted = false,
                        Stock = 4
                    },
                    new SparePart
                    {
                        EquipmentId = 14,
                        PartName = "Crawler Crane Hook",
                        PartNumber = "CCH-SCX800",
                        Manufacturer = "Hitachi",
                        AvailabilityStatus = "Pre-Order",
                        Price = 3000000m,
                        IsDeleted = false,
                        Stock = 3
                    },
                    new SparePart
                    {
                        EquipmentId = 15,
                        PartName = "Roller Tire",
                        PartNumber = "RT-CA2500D",
                        Manufacturer = "Dynapac",
                        AvailabilityStatus = "In Stock",
                        Price = 2200000m,
                        IsDeleted = false,
                        Stock = 12
                    },
                    new SparePart
                    {
                        EquipmentId = 16,
                        PartName = "Loader Bucket",
                        PartNumber = "LD-BKT-200",
                        Manufacturer = "Komatsu",
                        AvailabilityStatus = "In Stock",
                        Price = 7000000m,
                        IsDeleted = false,
                        Stock = 5
                    },
                    new SparePart
                    {
                        EquipmentId = 17,
                        PartName = "Dump Truck Transmission",
                        PartNumber = "DT-TR-40",
                        Manufacturer = "Scania",
                        AvailabilityStatus = "Pre-Order",
                        Price = 10500000m,
                        IsDeleted = false,
                        Stock = 4
                    },
                    new SparePart
                    {
                        EquipmentId = 18,
                        PartName = "Crane Boom Extension",
                        PartNumber = "CR-BM-1000",
                        Manufacturer = "Tadano",
                        AvailabilityStatus = "Limited",
                        Price = 12000000m,
                        IsDeleted = false,
                        Stock = 2
                    },
                    new SparePart
                    {
                        EquipmentId = 19,
                        PartName = "Roller Pneumatic Tire",
                        PartNumber = "RP-TIRE-24",
                        Manufacturer = "Bomag",
                        AvailabilityStatus = "In Stock",
                        Price = 3500000m,
                        IsDeleted = false,
                        Stock = 8
                    },
                    new SparePart
                    {
                        EquipmentId = 20,
                        PartName = "Crawler Loader Chain",
                        PartNumber = "CL-CHAIN-75",
                        Manufacturer = "Komatsu",
                        AvailabilityStatus = "In Stock",
                        Price = 9500000m,
                        IsDeleted = false,
                        Stock = 6
                    }
                };
                await context.SpareParts.AddRangeAsync(spareParts);
            }

            // Save changes to database
            await context.SaveChangesAsync();
        }
    }
}
