using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRFlow.Data.Migrations
{
    /// <inheritdoc />
    public partial class ActivateExistingEmployeesForPayroll : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE [Employees] SET [IsActive] = 1 WHERE [IsDeleted] = 0 AND [IsActive] = 0;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
