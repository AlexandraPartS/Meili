using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class Attr12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Users",
                newName: "phonenumber");

            migrationBuilder.RenameColumn(
                name: "NickName",
                table: "Users",
                newName: "nickname");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Users",
                newName: "isdeleted");

            migrationBuilder.RenameColumn(
                name: "CountryResidence",
                table: "Users",
                newName: "countryresidence");

            migrationBuilder.RenameColumn(
                name: "AvatarRelativePath",
                table: "Users",
                newName: "avatarrelativepath");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Users",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_Users_NickName",
                table: "Users",
                newName: "IX_Users_nickname");

            migrationBuilder.AddCheckConstraint(
                name: "nickname",
                table: "Users",
                sql: "nickname.Length >= 2 AND nickname.Length <= 128");

            migrationBuilder.AddCheckConstraint(
                name: "phonenumber",
                table: "Users",
                sql: "Regex.IsMatch(phonenumber, @\"^\\+(?:[\\s\\-]?[0-9]●?){6,14}[0-9]$\")");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "nickname",
                table: "Users");

            migrationBuilder.DropCheckConstraint(
                name: "phonenumber",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "phonenumber",
                table: "Users",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "nickname",
                table: "Users",
                newName: "NickName");

            migrationBuilder.RenameColumn(
                name: "isdeleted",
                table: "Users",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "countryresidence",
                table: "Users",
                newName: "CountryResidence");

            migrationBuilder.RenameColumn(
                name: "avatarrelativepath",
                table: "Users",
                newName: "AvatarRelativePath");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Users",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Users_nickname",
                table: "Users",
                newName: "IX_Users_NickName");
        }
    }
}
