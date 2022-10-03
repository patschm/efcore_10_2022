using ACME.DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace ACME.DataLayer.Repository.SqlServer;


// TODO 1: Create the ShopDatabaseContext
// TODO 1a: Create a constructor that accepts an argument of type DbContextOptions
// TODO 1b: Create DbSet Properties for
//                - Brands
//                - Prices
//                - Products
//                - ProductGroups
//                - Reviews
//                - Reviewers
//                - Specifications
//                - SpecificationDefinitions
// TODO 2: Override the OnModelCreating
// TODO 2a: In the OnModelCreated set the default database schema (dbo) to Core
// TODO 2b: Configure Brand Properties
//                 - Website: MaxLength=1024;
//                 - Name: MaxLength=255, Required
//                 - Timestamp: RowVersion, ConcurrencyToken
// TODO 2c: Configure Price Properties
//                - ShopName: MaxLength=255, Required
//                 - Timestamp: RowVersion, ConcurrencyToken
// TODO 2d: Configure Product Properties
//                 - Image: MaxLength=1024;
//                 - Name: MaxLength=255, Required
//                 - Timestamp: RowVersion, ConcurrencyToken
// TODO 2d: Configure ProductGroup Properties
//                 - Image: MaxLength=1024;
//                 - Name: MaxLength=255, Required
//                 - Timestamp: RowVersion, ConcurrencyToken
// TODO 2e: Configure Reviewer Properties
//                 - Name: MaxLength=255, Required
//                 - UserName: MaxLength=255, Required
//                 - Email: MaxLength=255, Required
//                 - Timestamp: RowVersion, ConcurrencyToken
// TODO 2f: Configure Review Properties
//                 - ReviewType: Convert to int
//                 - Organization: MaxLength=512
//                 - ReviewUrl: MaxLength=1024
//                 - Timestamp: RowVersion, ConcurrencyToken
// TODO 2g: Configure Specification Properties
//                 - Key: MaxLength=255, Required
//                 - Timestamp: RowVersion, ConcurrencyToken
// TODO 2h: Configure SpecificationDefinition Properties
//                 - Key: MaxLength=255, Required
//                 - Name: MaxLength=255, Required
//                 - Unit: MaxLength=127, Required
//                 - Timestamp: RowVersion, ConcurrencyToken

