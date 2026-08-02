using HRFlow.Business.DTOs.Payroll;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Globalization;

namespace HRFlow.Web.Services
{
    public class PayrollPdfService : IPayrollPdfService
    {
        public byte[] Generate(EmployeePayrollDetailDto payroll)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(36);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header()
                        .Text("BORDRO")
                        .FontSize(20)
                        .Bold()
                        .FontColor(Colors.Blue.Darken2);

                    page.Content().Column(column =>
                    {
                        column.Spacing(12);

                        column.Item().Text($"Dönem: {payroll.PeriodName}").Bold();
                        column.Item().Text($"Çalışan: {payroll.FullName}");
                        column.Item().Text($"Departman: {payroll.DepartmentName}");
                        column.Item().Text($"Pozisyon: {payroll.PositionName}");

                        column.Item().PaddingTop(8).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            AddRow(table, "Temel Maaş", FormatCurrency(payroll.BaseSalary));
                            AddRow(table, "Mesai Tutarı", FormatCurrency(payroll.OvertimeAmount));
                            AddRow(table, "Prim", FormatCurrency(payroll.Bonus));
                            AddRow(table, "Kesinti", FormatCurrency(payroll.Deduction));
                            AddRow(table, "Net Maaş", FormatCurrency(payroll.NetSalary), true);
                            AddRow(table, "Ödeme Tarihi", payroll.PaymentDate?.ToString("dd.MM.yyyy") ?? "-");
                            AddRow(table, "Durum", payroll.Status.ToString());
                        });
                    });

                    page.Footer()
                        .AlignCenter()
                        .Text("HRFlow EMS");
                });
            }).GeneratePdf();
        }

        private static void AddRow(
            TableDescriptor table,
            string label,
            string value,
            bool emphasize = false)
        {
            var labelCell = table.Cell()
                .BorderBottom(1)
                .BorderColor(Colors.Grey.Lighten2)
                .PaddingVertical(6)
                .Text(label);

            var valueCell = table.Cell()
                .BorderBottom(1)
                .BorderColor(Colors.Grey.Lighten2)
                .PaddingVertical(6)
                .AlignRight()
                .Text(value);

            if (emphasize)
            {
                labelCell.Bold();
                valueCell.Bold();
            }
        }

        private static string FormatCurrency(decimal value)
        {
            return value.ToString("C2", new CultureInfo("tr-TR"));
        }
    }
}
