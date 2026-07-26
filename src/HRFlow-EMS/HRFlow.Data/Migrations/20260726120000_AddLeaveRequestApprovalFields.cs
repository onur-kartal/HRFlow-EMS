using System;
using HRFlow.Data.Context;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRFlow.Data.Migrations
{
    [DbContext(typeof(HRFlowDbContext))]
    [Migration("20260726120000_AddLeaveRequestApprovalFields")]
    public partial class AddLeaveRequestApprovalFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApprovedBy",
                table: "LeaveRequests",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedDate",
                table: "LeaveRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalDays",
                table: "LeaveRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(
                "UPDATE LeaveRequests SET TotalDays = DATEDIFF(DAY, StartDate, EndDate) + 1 WHERE TotalDays = 0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                table: "LeaveRequests");

            migrationBuilder.DropColumn(
                name: "ApprovedDate",
                table: "LeaveRequests");

            migrationBuilder.DropColumn(
                name: "TotalDays",
                table: "LeaveRequests");
        }
    }
}
