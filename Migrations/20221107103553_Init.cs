using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace partner_aluro.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    ChildId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    kolejnosc = table.Column<int>(type: "int", nullable: true),
                    NazwaPlikuIkony = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Aktywny = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "ProfileDzialalnosci",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NazwaProfilu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rabat = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileDzialalnosci", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubCategory",
                columns: table => new
                {
                    SubCategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Aktywny = table.Column<bool>(type: "bit", nullable: true),
                    CatID = table.Column<int>(type: "int", nullable: false),
                    SubCatId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategory", x => x.SubCategoryId);
                    table.ForeignKey(
                        name: "FK_SubCategory_Category_SubCatId",
                        column: x => x.SubCatId,
                        principalTable: "Category",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    SubCategoryId = table.Column<int>(type: "int", nullable: true),
                    ProductImagesId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataDodania = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NazwaPlikuObrazka = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CenaProduktu = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Pakowanie = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Materiał = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ilosc = table.Column<int>(type: "int", nullable: true),
                    CenaProduktuDetal = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WagaProduktu = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SzerokoscProduktu = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WysokoscProduktu = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GlebokoscProduktu = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Bestseller = table.Column<bool>(type: "bit", nullable: false),
                    Ukryty = table.Column<bool>(type: "bit", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Products_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_SubCategory_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "SubCategory",
                        principalColumn: "SubCategoryId");
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    CartId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    ImageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    kolejnosc = table.Column<int>(type: "int", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    Tytul = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    ImageName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    ProductImagesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.ImageId);
                    table.ForeignKey(
                        name: "FK_Images_Products_ProductImagesId",
                        column: x => x.ProductImagesId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Adress1rozliczeniowy",
                columns: table => new
                {
                    Adres1rozliczeniowyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Miasto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kraj = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ulica = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NrNieruchomosci = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NrLokalu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KodPocztowy = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
                    Telefon = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    Vat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Wojewodztwo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Powiat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gmina = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusNip = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataZakonczeniaDzialalnosci = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Regon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Adres1UserID = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adress1rozliczeniowy", x => x.Adres1rozliczeniowyId);
                });

            migrationBuilder.CreateTable(
                name: "Adress2dostawy",
                columns: table => new
                {
                    Adres2dostawyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Imie = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nazwisko = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ulica = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Miasto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kraj = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KodPocztowy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Adres2UserID = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adress2dostawy", x => x.Adres2dostawyId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdProfilDzialalnosci = table.Column<int>(type: "int", nullable: true),
                    Imie = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Nazwisko = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    NazwaFirmy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataZałożenia = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Aktywny = table.Column<bool>(type: "bit", nullable: true),
                    Adress1rozliczeniowyId = table.Column<int>(type: "int", nullable: true),
                    Adress2dostawyId = table.Column<int>(type: "int", nullable: true),
                    NotatkaOsobista = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdUser = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Adress1rozliczeniowy_Adress1rozliczeniowyId",
                        column: x => x.Adress1rozliczeniowyId,
                        principalTable: "Adress1rozliczeniowy",
                        principalColumn: "Adres1rozliczeniowyId");
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Adress2dostawy_Adress2dostawyId",
                        column: x => x.Adress2dostawyId,
                        principalTable: "Adress2dostawy",
                        principalColumn: "Adres2dostawyId");
                    table.ForeignKey(
                        name: "FK_AspNetUsers_ProfileDzialalnosci_IdProfilDzialalnosci",
                        column: x => x.IdProfilDzialalnosci,
                        principalTable: "ProfileDzialalnosci",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUsers_ProfileDzialalnosci_IdUser",
                        column: x => x.IdUser,
                        principalTable: "ProfileDzialalnosci",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderTotal = table.Column<int>(type: "int", nullable: false),
                    OrderPlaced = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MessageToOrder = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RabatZamowienia = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StanZamowienia = table.Column<int>(type: "int", nullable: false),
                    Komentarz = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SposobDostawy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetodaPlatnosci = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    adresRozliczeniowyAdres1rozliczeniowyId = table.Column<int>(type: "int", nullable: false),
                    AdressDostawyAdres2dostawyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Adress1rozliczeniowy_adresRozliczeniowyAdres1rozliczeniowyId",
                        column: x => x.adresRozliczeniowyAdres1rozliczeniowyId,
                        principalTable: "Adress1rozliczeniowy",
                        principalColumn: "Adres1rozliczeniowyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Adress2dostawy_AdressDostawyAdres2dostawyId",
                        column: x => x.AdressDostawyAdres2dostawyId,
                        principalTable: "Adress2dostawy",
                        principalColumn: "Adres2dostawyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Cena = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Adress1rozliczeniowy_UserID",
                table: "Adress1rozliczeniowy",
                column: "UserID",
                unique: true,
                filter: "[UserID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Adress2dostawy_UserID",
                table: "Adress2dostawy",
                column: "UserID",
                unique: true,
                filter: "[UserID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Adress1rozliczeniowyId",
                table: "AspNetUsers",
                column: "Adress1rozliczeniowyId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Adress2dostawyId",
                table: "AspNetUsers",
                column: "Adress2dostawyId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_IdProfilDzialalnosci",
                table: "AspNetUsers",
                column: "IdProfilDzialalnosci");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_IdUser",
                table: "AspNetUsers",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductId",
                table: "CartItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_ProductImagesId",
                table: "Images",
                column: "ProductImagesId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_adresRozliczeniowyAdres1rozliczeniowyId",
                table: "Orders",
                column: "adresRozliczeniowyAdres1rozliczeniowyId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AdressDostawyAdres2dostawyId",
                table: "Orders",
                column: "AdressDostawyAdres2dostawyId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserID",
                table: "Orders",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SubCategoryId",
                table: "Products",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategory_SubCatId",
                table: "SubCategory",
                column: "SubCatId");

            migrationBuilder.AddForeignKey(
                name: "FK_Adress1rozliczeniowy_AspNetUsers_UserID",
                table: "Adress1rozliczeniowy",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Adress2dostawy_AspNetUsers_UserID",
                table: "Adress2dostawy",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adress1rozliczeniowy_AspNetUsers_UserID",
                table: "Adress1rozliczeniowy");

            migrationBuilder.DropForeignKey(
                name: "FK_Adress2dostawy_AspNetUsers_UserID",
                table: "Adress2dostawy");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "SubCategory");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Adress1rozliczeniowy");

            migrationBuilder.DropTable(
                name: "Adress2dostawy");

            migrationBuilder.DropTable(
                name: "ProfileDzialalnosci");
        }
    }
}
