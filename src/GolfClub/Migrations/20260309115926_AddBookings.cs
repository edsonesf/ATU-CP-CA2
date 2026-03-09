using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GolfClub.Migrations
{
    /// <inheritdoc />
    public partial class AddBookings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TeeTimeBookings",
                columns: table => new
                {
                    BookingId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TimeSlot = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    BookedByMemberId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeeTimeBookings", x => x.BookingId);
                    table.ForeignKey(
                        name: "FK_TeeTimeBookings_Members_BookedByMemberId",
                        column: x => x.BookedByMemberId,
                        principalTable: "Members",
                        principalColumn: "MemberId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingPlayers",
                columns: table => new
                {
                    BookingPlayerId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BookingId = table.Column<int>(type: "INTEGER", nullable: false),
                    MemberId = table.Column<int>(type: "INTEGER", nullable: false),
                    HandicapAtBooking = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingPlayers", x => x.BookingPlayerId);
                    table.ForeignKey(
                        name: "FK_BookingPlayers_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "MemberId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingPlayers_TeeTimeBookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "TeeTimeBookings",
                        principalColumn: "BookingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingPlayers_BookingId_MemberId",
                table: "BookingPlayers",
                columns: new[] { "BookingId", "MemberId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookingPlayers_MemberId",
                table: "BookingPlayers",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_TeeTimeBookings_BookedByMemberId",
                table: "TeeTimeBookings",
                column: "BookedByMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_TeeTimeBookings_Date_TimeSlot",
                table: "TeeTimeBookings",
                columns: new[] { "Date", "TimeSlot" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingPlayers");

            migrationBuilder.DropTable(
                name: "TeeTimeBookings");
        }
    }
}
