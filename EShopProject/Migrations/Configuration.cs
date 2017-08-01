namespace EShopProject.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<EShopProject.DBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(EShopProject.DBContext context)
        {

            context.AttributeGroup.AddOrUpdate(x => x.Id,
        new DataAccess.Entity.AttributeGroup() { Id = 1, Name = "Boyutlar", Deleted = false },
        new DataAccess.Entity.AttributeGroup() { Id = 2, Name = "Gövde", Deleted = false },
        new DataAccess.Entity.AttributeGroup() { Id = 3, Name = "Oturma Boyutları", Deleted = false },
        new DataAccess.Entity.AttributeGroup() { Id = 4, Name = "Lamba Bilgileri", Deleted = false });


            context.Attribute.AddOrUpdate(x => x.Id,
        new DataAccess.Entity.Attribute() { Id = 1, AttributeGroupId = 1, Name = "Yükseklik", Description = "Yükseklik Değeri", AttributeValueType = DataAccess.Entity.AttributeValueType.TamSayi, Deleted = false },
        new DataAccess.Entity.Attribute() { Id = 2, AttributeGroupId = 1, Name = "Genişlik", Description = "Genişlik Değeri", AttributeValueType = DataAccess.Entity.AttributeValueType.TamSayi, Deleted = false },
        new DataAccess.Entity.Attribute() { Id = 3, AttributeGroupId = 1, Name = "Derinlik", Description = "Derinlik Değeri", AttributeValueType = DataAccess.Entity.AttributeValueType.TamSayi, Deleted = false },
        new DataAccess.Entity.Attribute() { Id = 4, AttributeGroupId = 2, Name = "İskeleti", Description = "İskelet Malzemesi", AttributeValueType = DataAccess.Entity.AttributeValueType.Metin, Deleted = false },
        new DataAccess.Entity.Attribute() { Id = 5, AttributeGroupId = 2, Name = "Ayakları", Description = "Ayak Malzemesi", AttributeValueType = DataAccess.Entity.AttributeValueType.Metin, Deleted = false },
        new DataAccess.Entity.Attribute() { Id = 6, AttributeGroupId = null, Name = "Kapak Sayısı", Description = "Kapak Sayısı", AttributeValueType = DataAccess.Entity.AttributeValueType.TamSayi, Deleted = false },
        new DataAccess.Entity.Attribute() { Id = 7, AttributeGroupId = null, Name = "Oturma Sayısı", Description = "Oturma Sayısı", AttributeValueType = DataAccess.Entity.AttributeValueType.TamSayi, Deleted = false },
        new DataAccess.Entity.Attribute() { Id = 8, AttributeGroupId = 4, Name = "Lamba Sayısı", Description = "Lamba Sayısı", AttributeValueType = DataAccess.Entity.AttributeValueType.TamSayi, Deleted = false },
        new DataAccess.Entity.Attribute() { Id = 9, AttributeGroupId = 4, Name = "Lamba Türü", Description = "Lamba Türü", AttributeValueType = DataAccess.Entity.AttributeValueType.Metin, Deleted = false });

            context.User.AddOrUpdate(x => x.Id,
    new DataAccess.Entity.User() { Id = 1, EmailAddress = "erencancstan@gmail.com", Password = "asd1" , MyAuthorization = DataAccess.Entity.MyAuthorization.Admin, CreateDate = DateTime.Now, Deleted = false});

            context.Customer.AddOrUpdate(x => x.Id,
    new DataAccess.Entity.Customer() { Id = 1, FirstName = "Erencan", LastName = "Cüstan", Gender = DataAccess.Entity.Gender.Erkek, BirthOfDate = DateTime.Now, Deleted = false });

            context.Address.AddOrUpdate(x => x.Id,
    new DataAccess.Entity.Address() { Id = 1, Name = "Ev Adresi", PostAddress = "Talatpaşa mah. oralardar ", City="istanbul",Region="Marmara",PostalCode="34400",Country="Türkiye", Deleted = false });

        }
    }
}
