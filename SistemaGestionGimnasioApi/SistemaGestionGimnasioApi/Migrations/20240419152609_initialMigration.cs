using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaGestionGimnasioApi.Migrations
{
    public partial class initialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    IdActivity = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ActivityName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ActivityDescription = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.IdActivity);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Email = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserType = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AutorizationToReserve = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Email);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TrainerActivities",
                columns: table => new
                {
                    IdTrainerActivity = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TrainerEmail = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdActivity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainerActivities", x => x.IdTrainerActivity);
                    table.ForeignKey(
                        name: "FK_TrainerActivities_Activities_IdActivity",
                        column: x => x.IdActivity,
                        principalTable: "Activities",
                        principalColumn: "IdActivity",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrainerActivities_Users_TrainerEmail",
                        column: x => x.TrainerEmail,
                        principalTable: "Users",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "GymClasses",
                columns: table => new
                {
                    IdGymClass = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdTrainerActivity = table.Column<int>(type: "int", nullable: false),
                    DateTimeClass = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GymClasses", x => x.IdGymClass);
                    table.ForeignKey(
                        name: "FK_GymClasses_TrainerActivities_IdTrainerActivity",
                        column: x => x.IdTrainerActivity,
                        principalTable: "TrainerActivities",
                        principalColumn: "IdTrainerActivity",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Reserves",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdGymClass = table.Column<int>(type: "int", nullable: false),
                    ClientEmail = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reserves", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reserves_GymClasses_IdGymClass",
                        column: x => x.IdGymClass,
                        principalTable: "GymClasses",
                        principalColumn: "IdGymClass",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reserves_Users_ClientEmail",
                        column: x => x.ClientEmail,
                        principalTable: "Users",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Activities",
                columns: new[] { "IdActivity", "ActivityDescription", "ActivityName" },
                values: new object[] { -8, "Actividad deportiva que involucra dos equipos compitiendo por marcar goles en la portería contraria.", "Fútbol" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Email", "IsDeleted", "LastName", "Name", "Password", "UserType" },
                values: new object[] { "belenlinera@gmail.com", true, "Linera", "Belen", "123456", "Admin" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Email", "AutorizationToReserve", "IsDeleted", "LastName", "Name", "Password", "UserType" },
                values: new object[] { "juanperez@gmail.com", true, true, "Perez", "Juan", "123456", "Client" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Email", "IsDeleted", "LastName", "Name", "Password", "UserType" },
                values: new object[] { "pedrolopez@gmail.com", false, "Lopez", "Pedro", "123456", "Trainer" });

            migrationBuilder.InsertData(
                table: "TrainerActivities",
                columns: new[] { "IdTrainerActivity", "IdActivity", "TrainerEmail" },
                values: new object[] { -6, -8, "pedrolopez@gmail.com" });

            migrationBuilder.InsertData(
                table: "GymClasses",
                columns: new[] { "IdGymClass", "Capacity", "DateTimeClass", "IdTrainerActivity" },
                values: new object[] { -4, 20, new DateTime(2024, 4, 20, 16, 0, 0, 0, DateTimeKind.Unspecified), -6 });

            migrationBuilder.CreateIndex(
                name: "IX_GymClasses_IdTrainerActivity",
                table: "GymClasses",
                column: "IdTrainerActivity");

            migrationBuilder.CreateIndex(
                name: "IX_Reserves_ClientEmail",
                table: "Reserves",
                column: "ClientEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Reserves_IdGymClass",
                table: "Reserves",
                column: "IdGymClass");

            migrationBuilder.CreateIndex(
                name: "IX_TrainerActivities_IdActivity",
                table: "TrainerActivities",
                column: "IdActivity");

            migrationBuilder.CreateIndex(
                name: "IX_TrainerActivities_TrainerEmail",
                table: "TrainerActivities",
                column: "TrainerEmail");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reserves");

            migrationBuilder.DropTable(
                name: "GymClasses");

            migrationBuilder.DropTable(
                name: "TrainerActivities");

            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
