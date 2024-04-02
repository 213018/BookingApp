using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingApplication.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BookingLists",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingLists_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookReservation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReservationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BookingListId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Number_of_nights = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookReservation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookReservation_BookingLists_BookingListId",
                        column: x => x.BookingListId,
                        principalTable: "BookingLists",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BookReservation_Reservations_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingLists_UserId",
                table: "BookingLists",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookReservation_BookingListId",
                table: "BookReservation",
                column: "BookingListId");

            migrationBuilder.CreateIndex(
                name: "IX_BookReservation_ReservationId",
                table: "BookReservation",
                column: "ReservationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookReservation");

            migrationBuilder.DropTable(
                name: "BookingLists");
        }
    }
}
