using System;
using Microsoft.EntityFrameworkCore.Migrations;
namespace HRFlow.Data.Migrations
{
    public partial class AddAuditAndRequestLogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(name:"AuditLogs",columns:table=>new { Id=table.Column<int>(nullable:false).Annotation("SqlServer:Identity","1, 1"),CreatedDate=table.Column<DateTime>(nullable:false),UserId=table.Column<string>(maxLength:450,nullable:true),EmployeeId=table.Column<int>(nullable:true),UserName=table.Column<string>(maxLength:256,nullable:true),Role=table.Column<string>(maxLength:100,nullable:true),Module=table.Column<int>(nullable:false),Action=table.Column<int>(nullable:false),EntityId=table.Column<int>(nullable:true),Description=table.Column<string>(maxLength:1000,nullable:false),IpAddress=table.Column<string>(maxLength:45,nullable:true)},constraints:table=>table.PrimaryKey("PK_AuditLogs",x=>x.Id));
            migrationBuilder.CreateTable(name:"RequestLogs",columns:table=>new { Id=table.Column<int>(nullable:false).Annotation("SqlServer:Identity","1, 1"),CreatedDate=table.Column<DateTime>(nullable:false),UserId=table.Column<string>(maxLength:450,nullable:true),UserName=table.Column<string>(maxLength:256,nullable:true),Role=table.Column<string>(maxLength:100,nullable:true),IpAddress=table.Column<string>(maxLength:45,nullable:true),RequestPath=table.Column<string>(maxLength:1000,nullable:false),HttpMethod=table.Column<string>(maxLength:10,nullable:false),StatusCode=table.Column<int>(nullable:false),DurationMs=table.Column<long>(nullable:false),UserAgent=table.Column<string>(maxLength:1000,nullable:true),Browser=table.Column<string>(maxLength:100,nullable:true),OperatingSystem=table.Column<string>(maxLength:100,nullable:true)},constraints:table=>table.PrimaryKey("PK_RequestLogs",x=>x.Id));
        }
        protected override void Down(MigrationBuilder migrationBuilder){migrationBuilder.DropTable("AuditLogs");migrationBuilder.DropTable("RequestLogs");}
    }
}
