using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BancoAula.Api.Migrations
{
    /// <inheritdoc />
    public partial class NovoEmprestimo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataSolicitacao",
                table: "Emprestimos");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Emprestimos");

            migrationBuilder.DropColumn(
                name: "TaxaJuros",
                table: "Emprestimos");

            migrationBuilder.RenameColumn(
                name: "Prazo",
                table: "Emprestimos",
                newName: "Parcelas");

            migrationBuilder.RenameColumn(
                name: "DataVencimento",
                table: "Emprestimos",
                newName: "DataContrato");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Parcelas",
                table: "Emprestimos",
                newName: "Prazo");

            migrationBuilder.RenameColumn(
                name: "DataContrato",
                table: "Emprestimos",
                newName: "DataVencimento");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataSolicitacao",
                table: "Emprestimos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Emprestimos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "TaxaJuros",
                table: "Emprestimos",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }
    }
}
