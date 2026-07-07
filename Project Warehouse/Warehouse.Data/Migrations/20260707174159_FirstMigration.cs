using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Warehouse.Data.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Customer_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Customer_Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Customer_Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Customer_Id);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Location_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Location_Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Location_Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    User_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_Fullname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    User_Adress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    User_Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    User_Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.User_Id);
                });

            migrationBuilder.CreateTable(
                name: "Vendors",
                columns: table => new
                {
                    Vendor_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Vendor_Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Vendor_Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendors", x => x.Vendor_Id);
                });

            migrationBuilder.CreateTable(
                name: "Shipments",
                columns: table => new
                {
                    Shipment_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Customer_Id = table.Column<int>(type: "int", nullable: false),
                    Shipment_Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Sale_Price = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipments", x => x.Shipment_Id);
                    table.ForeignKey(
                        name: "FK_Shipments_Customers_Customer_Id",
                        column: x => x.Customer_Id,
                        principalTable: "Customers",
                        principalColumn: "Customer_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bins",
                columns: table => new
                {
                    Bin_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Bin_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location_Id = table.Column<int>(type: "int", nullable: false),
                    RealBin = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bins", x => x.Bin_Id);
                    table.ForeignKey(
                        name: "FK_Bins_Locations_Location_Id",
                        column: x => x.Location_Id,
                        principalTable: "Locations",
                        principalColumn: "Location_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    Material_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Material_Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Material_Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Vendor_Id = table.Column<int>(type: "int", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.Material_Id);
                    table.ForeignKey(
                        name: "FK_Materials_Vendors_Vendor_Id",
                        column: x => x.Vendor_Id,
                        principalTable: "Vendors",
                        principalColumn: "Vendor_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Movements",
                columns: table => new
                {
                    Movement_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_Id = table.Column<int>(type: "int", nullable: false),
                    Movement_Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastBinLocation_Id = table.Column<int>(type: "int", nullable: false),
                    NewBinLocation_Id = table.Column<int>(type: "int", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movements", x => x.Movement_Id);
                    table.ForeignKey(
                        name: "FK_Movements_Bins_LastBinLocation_Id",
                        column: x => x.LastBinLocation_Id,
                        principalTable: "Bins",
                        principalColumn: "Bin_Id");
                    table.ForeignKey(
                        name: "FK_Movements_Bins_NewBinLocation_Id",
                        column: x => x.NewBinLocation_Id,
                        principalTable: "Bins",
                        principalColumn: "Bin_Id");
                    table.ForeignKey(
                        name: "FK_Movements_Users_User_Id",
                        column: x => x.User_Id,
                        principalTable: "Users",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LocatedMaterialas",
                columns: table => new
                {
                    LocatedMaterials_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Bin_Id = table.Column<int>(type: "int", nullable: false),
                    Material_Id = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocatedMaterialas", x => x.LocatedMaterials_Id);
                    table.ForeignKey(
                        name: "FK_LocatedMaterialas_Bins_Bin_Id",
                        column: x => x.Bin_Id,
                        principalTable: "Bins",
                        principalColumn: "Bin_Id");
                    table.ForeignKey(
                        name: "FK_LocatedMaterialas_Materials_Material_Id",
                        column: x => x.Material_Id,
                        principalTable: "Materials",
                        principalColumn: "Material_Id");
                });

            migrationBuilder.CreateTable(
                name: "MaterialsByShipments",
                columns: table => new
                {
                    MaterialsByShioment_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Shipment_Id = table.Column<int>(type: "int", nullable: false),
                    Material_Id = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialsByShipments", x => x.MaterialsByShioment_Id);
                    table.ForeignKey(
                        name: "FK_MaterialsByShipments_Materials_Material_Id",
                        column: x => x.Material_Id,
                        principalTable: "Materials",
                        principalColumn: "Material_Id");
                    table.ForeignKey(
                        name: "FK_MaterialsByShipments_Shipments_Shipment_Id",
                        column: x => x.Shipment_Id,
                        principalTable: "Shipments",
                        principalColumn: "Shipment_Id");
                });

            migrationBuilder.CreateTable(
                name: "Models",
                columns: table => new
                {
                    Model_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Model_Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    New_Material_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Models", x => x.Model_Id);
                    table.ForeignKey(
                        name: "FK_Models_Materials_New_Material_Id",
                        column: x => x.New_Material_Id,
                        principalTable: "Materials",
                        principalColumn: "Material_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaterialMovements",
                columns: table => new
                {
                    MaterialMovement_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Movement_Id = table.Column<int>(type: "int", nullable: false),
                    Material_Id = table.Column<int>(type: "int", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialMovements", x => x.MaterialMovement_Id);
                    table.ForeignKey(
                        name: "FK_MaterialMovements_Materials_Material_Id",
                        column: x => x.Material_Id,
                        principalTable: "Materials",
                        principalColumn: "Material_Id");
                    table.ForeignKey(
                        name: "FK_MaterialMovements_Movements_Movement_Id",
                        column: x => x.Movement_Id,
                        principalTable: "Movements",
                        principalColumn: "Movement_Id");
                });

            migrationBuilder.CreateTable(
                name: "MaterialsByModels",
                columns: table => new
                {
                    MaterialByModel_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Model_Id = table.Column<int>(type: "int", nullable: false),
                    Material_Id = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialsByModels", x => x.MaterialByModel_Id);
                    table.ForeignKey(
                        name: "FK_MaterialsByModels_Materials_Material_Id",
                        column: x => x.Material_Id,
                        principalTable: "Materials",
                        principalColumn: "Material_Id");
                    table.ForeignKey(
                        name: "FK_MaterialsByModels_Models_Model_Id",
                        column: x => x.Model_Id,
                        principalTable: "Models",
                        principalColumn: "Model_Id");
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Customer_Id", "Customer_Email", "Customer_Name" },
                values: new object[] { 1, "RouterUniversal@example.com", "Router Universals" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Location_Id", "Location_Name" },
                values: new object[,]
                {
                    { 1, "0100" },
                    { 2, "0101" },
                    { 3, "0102" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "User_Id", "User_Adress", "User_Email", "User_Fullname", "User_Password" },
                values: new object[,]
                {
                    { 1, "Cuauhtemoc #72", "jesus.quintero7478@alumnos.udg.mx", "Jesus Eduardo Quintero", "JesusEduardo" },
                    { 2, "Ramon Corona #53", "jorge.flores0258@alumnos.udg.mx", "Jorge Flores Kuan", "JorgeFlores" }
                });

            migrationBuilder.InsertData(
                table: "Vendors",
                columns: new[] { "Vendor_Id", "Vendor_Email", "Vendor_Name" },
                values: new object[,]
                {
                    { 1, "BossMail@example.com", "Production Line" },
                    { 2, "UniversalCircuits@example.com", "Universal Circuits" },
                    { 3, "UniversalPlastics@example.com", "Universal Plastics" },
                    { 4, "UniversalSteels@example.com", "Universal Steels" }
                });

            migrationBuilder.InsertData(
                table: "Bins",
                columns: new[] { "Bin_Id", "Bin_Name", "Location_Id", "RealBin" },
                values: new object[,]
                {
                    { 1, "100-01A", 1, true },
                    { 2, "100-01B", 1, true },
                    { 3, "100-01C", 1, true },
                    { 4, "100-01D", 1, true },
                    { 5, "100-01E", 1, true },
                    { 6, "100-02A", 1, true },
                    { 7, "100-02B", 1, true },
                    { 8, "100-02C", 1, true },
                    { 9, "100-02D", 1, true },
                    { 10, "100-02E", 1, true },
                    { 11, "", 2, false },
                    { 12, "", 3, false }
                });

            migrationBuilder.InsertData(
                table: "Materials",
                columns: new[] { "Material_Id", "Material_Description", "Material_Name", "Vendor_Id" },
                values: new object[,]
                {
                    { 1, "Circuit type A for router", "Circuit A", 2 },
                    { 2, "Casing for Router", "Casing A", 3 },
                    { 3, "Screw for router", "Screw A", 4 },
                    { 4, "Circuit type B for fast router", "Circuit B", 2 },
                    { 5, "Manufactered Router ready for shipping", "Router", 1 },
                    { 6, "Manufactered Fast Router ready for shipping", "Fast Router", 1 }
                });

            migrationBuilder.InsertData(
                table: "Models",
                columns: new[] { "Model_Id", "Model_Name", "New_Material_Id" },
                values: new object[,]
                {
                    { 1, "Router", 5 },
                    { 2, "Fast Router", 6 }
                });

            migrationBuilder.InsertData(
                table: "MaterialsByModels",
                columns: new[] { "MaterialByModel_Id", "Material_Id", "Model_Id", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, 1, 1 },
                    { 2, 2, 1, 1 },
                    { 3, 3, 1, 4 },
                    { 4, 4, 2, 1 },
                    { 5, 2, 2, 1 },
                    { 6, 3, 2, 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bins_Location_Id",
                table: "Bins",
                column: "Location_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Customer_Email",
                table: "Customers",
                column: "Customer_Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LocatedMaterialas_Bin_Id",
                table: "LocatedMaterialas",
                column: "Bin_Id");

            migrationBuilder.CreateIndex(
                name: "IX_LocatedMaterialas_Material_Id",
                table: "LocatedMaterialas",
                column: "Material_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_Location_Name",
                table: "Locations",
                column: "Location_Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MaterialMovements_Material_Id",
                table: "MaterialMovements",
                column: "Material_Id");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialMovements_Movement_Id",
                table: "MaterialMovements",
                column: "Movement_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_Material_Name",
                table: "Materials",
                column: "Material_Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Materials_Vendor_Id",
                table: "Materials",
                column: "Vendor_Id");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialsByModels_Material_Id",
                table: "MaterialsByModels",
                column: "Material_Id");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialsByModels_Model_Id",
                table: "MaterialsByModels",
                column: "Model_Id");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialsByShipments_Material_Id",
                table: "MaterialsByShipments",
                column: "Material_Id");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialsByShipments_Shipment_Id",
                table: "MaterialsByShipments",
                column: "Shipment_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Models_Model_Name",
                table: "Models",
                column: "Model_Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Models_New_Material_Id",
                table: "Models",
                column: "New_Material_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Movements_LastBinLocation_Id",
                table: "Movements",
                column: "LastBinLocation_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Movements_NewBinLocation_Id",
                table: "Movements",
                column: "NewBinLocation_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Movements_User_Id",
                table: "Movements",
                column: "User_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_Customer_Id",
                table: "Shipments",
                column: "Customer_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_User_Email",
                table: "Users",
                column: "User_Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_Vendor_Email",
                table: "Vendors",
                column: "Vendor_Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_Vendor_Name",
                table: "Vendors",
                column: "Vendor_Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LocatedMaterialas");

            migrationBuilder.DropTable(
                name: "MaterialMovements");

            migrationBuilder.DropTable(
                name: "MaterialsByModels");

            migrationBuilder.DropTable(
                name: "MaterialsByShipments");

            migrationBuilder.DropTable(
                name: "Movements");

            migrationBuilder.DropTable(
                name: "Models");

            migrationBuilder.DropTable(
                name: "Shipments");

            migrationBuilder.DropTable(
                name: "Bins");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Vendors");
        }
    }
}
