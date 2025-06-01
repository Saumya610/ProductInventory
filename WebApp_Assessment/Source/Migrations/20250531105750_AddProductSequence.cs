using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp_Assessment.Migrations
{
    /// <inheritdoc />
    public partial class AddProductSequence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
            @"CREATE SEQUENCE ProductSequence
              START WITH 1
              INCREMENT BY 1
              MINVALUE 1
              MAXVALUE 999999
              NO CYCLE;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP SEQUENCE ProductSequence");
        }
    }
}
