using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeMatcher.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DeleteMeetingCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Slots_Meetings_MeetingId",
                schema: "time-matcher",
                table: "Slots");

            migrationBuilder.AddForeignKey(
                name: "FK_Slots_Meetings_MeetingId",
                schema: "time-matcher",
                table: "Slots",
                column: "MeetingId",
                principalSchema: "time-matcher",
                principalTable: "Meetings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Slots_Meetings_MeetingId",
                schema: "time-matcher",
                table: "Slots");

            migrationBuilder.AddForeignKey(
                name: "FK_Slots_Meetings_MeetingId",
                schema: "time-matcher",
                table: "Slots",
                column: "MeetingId",
                principalSchema: "time-matcher",
                principalTable: "Meetings",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
