using OfficeOpenXml.Style;

namespace BradescoPGP.Web
{
    public static class BorderEPPLus
    {
        public static void BorderFull(this Border border, ExcelBorderStyle borderStyle = ExcelBorderStyle.Thin)
        {
            border.Top.Style = borderStyle;
            border.Bottom.Style = borderStyle;
            border.Left.Style = borderStyle;
            border.Right.Style = borderStyle;
        }
    }
}