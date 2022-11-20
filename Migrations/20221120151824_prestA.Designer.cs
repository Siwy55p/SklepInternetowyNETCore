﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using partner_aluro.Data;

#nullable disable

namespace partner_aluro.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20221120151824_prestA")]
    partial class prestA
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("partner_aluro.Models.Adress1rozliczeniowy", b =>
                {
                    b.Property<int>("Adres1rozliczeniowyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Adres1rozliczeniowyId"), 1L, 1);

                    b.Property<string>("Adres1UserID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DataZakonczeniaDzialalnosci")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gmina")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("KodPocztowy")
                        .IsRequired()
                        .HasMaxLength(7)
                        .HasColumnType("nvarchar(7)");

                    b.Property<string>("Kraj")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Miasto")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NrLokalu")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NrNieruchomosci")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Powiat")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Regon")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StatusNip")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telefon")
                        .IsRequired()
                        .HasMaxLength(9)
                        .HasColumnType("nvarchar(9)");

                    b.Property<string>("Ulica")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Vat")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Wojewodztwo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Adres1rozliczeniowyId");

                    b.HasIndex("UserID")
                        .IsUnique()
                        .HasFilter("[UserID] IS NOT NULL");

                    b.ToTable("Adress1rozliczeniowy");
                });

            modelBuilder.Entity("partner_aluro.Models.Adress2dostawy", b =>
                {
                    b.Property<int>("Adres2dostawyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Adres2dostawyId"), 1L, 1);

                    b.Property<string>("Adres2UserID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Imie")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("KodPocztowy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Kraj")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Miasto")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nazwisko")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telefon")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ulica")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserID")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Adres2dostawyId");

                    b.HasIndex("UserID")
                        .IsUnique()
                        .HasFilter("[UserID] IS NOT NULL");

                    b.ToTable("Adress2dostawy");
                });

            modelBuilder.Entity("partner_aluro.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<int?>("Adress1rozliczeniowyId")
                        .HasColumnType("int");

                    b.Property<int?>("Adress2dostawyId")
                        .HasColumnType("int");

                    b.Property<bool?>("Aktywny")
                        .HasColumnType("bit");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DataZałożenia")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<int?>("IdProfilDzialalnosci")
                        .HasColumnType("int");

                    b.Property<int?>("IdUser")
                        .HasColumnType("int");

                    b.Property<string>("Imie")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NazwaFirmy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nazwisko")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<bool?>("Newsletter")
                        .HasColumnType("bit");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NotatkaOsobista")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool?>("PolitykaPrywatnosci")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("Adress1rozliczeniowyId");

                    b.HasIndex("Adress2dostawyId");

                    b.HasIndex("IdProfilDzialalnosci");

                    b.HasIndex("IdUser");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("partner_aluro.Models.CartItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("CartId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("CartItems");
                });

            modelBuilder.Entity("partner_aluro.Models.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryId"), 1L, 1);

                    b.Property<bool?>("Aktywny")
                        .HasColumnType("bit");

                    b.Property<int?>("ChildId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NazwaPlikuIkony")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ParentId")
                        .HasColumnType("int");

                    b.Property<int?>("kolejnosc")
                        .HasColumnType("int");

                    b.HasKey("CategoryId");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("partner_aluro.Models.ContactPrestashop", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("active")
                        .HasColumnType("int");

                    b.Property<string>("company")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("deleted")
                        .HasColumnType("int");

                    b.Property<string>("email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("id_default_group")
                        .HasColumnType("int");

                    b.Property<int?>("id_gender")
                        .HasColumnType("int");

                    b.Property<int?>("id_id_lang")
                        .HasColumnType("int");

                    b.Property<int?>("id_risk")
                        .HasColumnType("int");

                    b.Property<int?>("id_shop")
                        .HasColumnType("int");

                    b.Property<int?>("id_shop_group")
                        .HasColumnType("int");

                    b.Property<int?>("is_quest")
                        .HasColumnType("int");

                    b.Property<string>("note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("passwd")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("secure_key")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("website")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ContactsPrestashop");
                });

            modelBuilder.Entity("partner_aluro.Models.ImageModel", b =>
                {
                    b.Property<int>("ImageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ImageId"), 1L, 1);

                    b.Property<string>("ImageName")
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("ImageSliderID")
                        .HasColumnType("int");

                    b.Property<string>("Opis")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ProductId")
                        .HasColumnType("int");

                    b.Property<int?>("ProductImagesId")
                        .HasColumnType("int");

                    b.Property<int?>("SliderIds")
                        .HasColumnType("int");

                    b.Property<string>("Tytul")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("fullPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("kolejnosc")
                        .HasColumnType("int");

                    b.Property<string>("path")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ImageId");

                    b.HasIndex("ImageSliderID");

                    b.HasIndex("ProductImagesId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("partner_aluro.Models.Newsletter", b =>
                {
                    b.Property<int>("NewsletterID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NewsletterID"), 1L, 1);

                    b.Property<string>("MessagerBody")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nazwa")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("NewsletterID");

                    b.ToTable("Newsletter");
                });

            modelBuilder.Entity("partner_aluro.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AdressDostawyAdres2dostawyId")
                        .HasColumnType("int");

                    b.Property<string>("Komentarz")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MessageToOrder")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MetodaPlatnosci")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("OrderPlaced")
                        .HasColumnType("datetime2");

                    b.Property<int>("OrderTotal")
                        .HasColumnType("int");

                    b.Property<decimal?>("RabatZamowienia")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("SposobDostawy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StanZamowienia")
                        .HasColumnType("int");

                    b.Property<string>("UserID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("adresRozliczeniowyAdres1rozliczeniowyId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AdressDostawyAdres2dostawyId");

                    b.HasIndex("UserID");

                    b.HasIndex("adresRozliczeniowyAdres1rozliczeniowyId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("partner_aluro.Models.OrderItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Cena")
                        .HasColumnType("int");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("partner_aluro.Models.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductId"), 1L, 1);

                    b.Property<bool>("Bestseller")
                        .HasColumnType("bit");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<decimal>("CenaProduktu")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("CenaProduktuDetal")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("CenaPromocyja")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("DataDodania")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EAN13")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("GlebokoscProduktu")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("GlebokoscWewnetrznaProduktu")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("Ilosc")
                        .HasColumnType("int");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("KrotkiOpis")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Materiał")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("NazwaPlikuObrazka")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Pakowanie")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProductImagesId")
                        .HasColumnType("int");

                    b.Property<bool>("Promocja")
                        .HasColumnType("bit");

                    b.Property<int?>("SubCategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("nvarchar(13)");

                    b.Property<decimal?>("SzerokoscProduktu")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("SzerokoscWewnetrznaProduktu")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("Ukryty")
                        .HasColumnType("bit");

                    b.Property<decimal?>("WagaProduktu")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Wymiar_wewnetrzny")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("WysokoscProduktu")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("WysokoscWewnetrznaProduktu")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("ProductId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("Symbol")
                        .IsUnique();

                    b.ToTable("Products");
                });

            modelBuilder.Entity("partner_aluro.Models.ProductCategory", b =>
                {
                    b.Property<int>("ProductCategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductCategoryId"), 1L, 1);

                    b.Property<int?>("CategoryID")
                        .HasColumnType("int");

                    b.Property<int?>("ProductID")
                        .HasColumnType("int");

                    b.HasKey("ProductCategoryId");

                    b.HasIndex("CategoryID");

                    b.HasIndex("ProductID");

                    b.ToTable("ProductCategory");
                });

            modelBuilder.Entity("partner_aluro.Models.ProfilDzialalnosci", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("IdUser")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NazwaProfilu")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("Rabat")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("ProfileDzialalnosci");
                });

            modelBuilder.Entity("partner_aluro.Models.Slider", b =>
                {
                    b.Property<int>("ImageSliderID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ImageSliderID"), 1L, 1);

                    b.Property<int?>("IdObrazek")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ImageSliderID");

                    b.ToTable("Sliders");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("partner_aluro.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("partner_aluro.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("partner_aluro.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("partner_aluro.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("partner_aluro.Models.Adress1rozliczeniowy", b =>
                {
                    b.HasOne("partner_aluro.Models.ApplicationUser", "ApplicationUser")
                        .WithOne()
                        .HasForeignKey("partner_aluro.Models.Adress1rozliczeniowy", "UserID");

                    b.Navigation("ApplicationUser");
                });

            modelBuilder.Entity("partner_aluro.Models.Adress2dostawy", b =>
                {
                    b.HasOne("partner_aluro.Models.ApplicationUser", "ApplicationUser")
                        .WithOne()
                        .HasForeignKey("partner_aluro.Models.Adress2dostawy", "UserID");

                    b.Navigation("ApplicationUser");
                });

            modelBuilder.Entity("partner_aluro.Models.ApplicationUser", b =>
                {
                    b.HasOne("partner_aluro.Models.Adress1rozliczeniowy", "Adress1rozliczeniowy")
                        .WithMany()
                        .HasForeignKey("Adress1rozliczeniowyId");

                    b.HasOne("partner_aluro.Models.Adress2dostawy", "Adress2dostawy")
                        .WithMany()
                        .HasForeignKey("Adress2dostawyId");

                    b.HasOne("partner_aluro.Models.ProfilDzialalnosci", "ProfilDzialalnosci")
                        .WithMany()
                        .HasForeignKey("IdProfilDzialalnosci");

                    b.HasOne("partner_aluro.Models.ProfilDzialalnosci", null)
                        .WithMany("UserProfilDzialalnosci")
                        .HasForeignKey("IdUser");

                    b.Navigation("Adress1rozliczeniowy");

                    b.Navigation("Adress2dostawy");

                    b.Navigation("ProfilDzialalnosci");
                });

            modelBuilder.Entity("partner_aluro.Models.CartItem", b =>
                {
                    b.HasOne("partner_aluro.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("partner_aluro.Models.ImageModel", b =>
                {
                    b.HasOne("partner_aluro.Models.Slider", null)
                        .WithMany("ObrazkiDostepneWSliderze")
                        .HasForeignKey("ImageSliderID");

                    b.HasOne("partner_aluro.Models.Product", "Product")
                        .WithMany("Product_Images")
                        .HasForeignKey("ProductImagesId");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("partner_aluro.Models.Order", b =>
                {
                    b.HasOne("partner_aluro.Models.Adress2dostawy", "AdressDostawy")
                        .WithMany()
                        .HasForeignKey("AdressDostawyAdres2dostawyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("partner_aluro.Models.ApplicationUser", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("partner_aluro.Models.Adress1rozliczeniowy", "adresRozliczeniowy")
                        .WithMany()
                        .HasForeignKey("adresRozliczeniowyAdres1rozliczeniowyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AdressDostawy");

                    b.Navigation("User");

                    b.Navigation("adresRozliczeniowy");
                });

            modelBuilder.Entity("partner_aluro.Models.OrderItem", b =>
                {
                    b.HasOne("partner_aluro.Models.Order", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("partner_aluro.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("partner_aluro.Models.Product", b =>
                {
                    b.HasOne("partner_aluro.Models.Category", "CategoryNavigation")
                        .WithMany("Produkty")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CategoryNavigation");
                });

            modelBuilder.Entity("partner_aluro.Models.ProductCategory", b =>
                {
                    b.HasOne("partner_aluro.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryID");

                    b.HasOne("partner_aluro.Models.Product", "Product")
                        .WithMany("Kategorie")
                        .HasForeignKey("ProductID");

                    b.Navigation("Category");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("partner_aluro.Models.ApplicationUser", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("partner_aluro.Models.Category", b =>
                {
                    b.Navigation("Produkty");
                });

            modelBuilder.Entity("partner_aluro.Models.Order", b =>
                {
                    b.Navigation("OrderItems");
                });

            modelBuilder.Entity("partner_aluro.Models.Product", b =>
                {
                    b.Navigation("Kategorie");

                    b.Navigation("Product_Images");
                });

            modelBuilder.Entity("partner_aluro.Models.ProfilDzialalnosci", b =>
                {
                    b.Navigation("UserProfilDzialalnosci");
                });

            modelBuilder.Entity("partner_aluro.Models.Slider", b =>
                {
                    b.Navigation("ObrazkiDostepneWSliderze");
                });
#pragma warning restore 612, 618
        }
    }
}
