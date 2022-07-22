using ExcelUploadReadDataSaveExampleCore.Models;
using ExcelUploadReadDataSaveExampleCore.Service;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ExcelUploadReadDataSaveExampleCore.Controllers
{
    public class StudentsController : Controller
    {
        IStudentService _studentService = null;
        List<Student> _students = new List<Student>();

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public JsonResult SaveStudents(List<Student> students)
        {
            _students = _studentService.SaveStudents(students);
            return Json(_students);  
        }

        public string GenerateAndDownloadExcel(int studentId, string name)
        {
            List<Student> students = _studentService.GetStudents();
            var datatable = CommonMethods.ConvertListToDataTable(students);
            datatable.Columns.Remove("StudentId");

            byte[] fileContents = null;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage pck = new ExcelPackage())
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Students");
                ws.Cells["A1"].Value = "School Name";
                ws.Cells["A1"].Style.Font.Bold = true;
                ws.Cells["A1"].Style.Font.Size = 16;
                ws.Cells["A1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells["A2"].Value = "Student List";
                ws.Cells["A2"].Style.Font.Bold = true;
                ws.Cells["A2"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["A2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells["A3"].LoadFromDataTable(datatable, true);
                ws.Cells["A3:C3"].Style.Font.Bold = true;
                ws.Cells["A3:C3"].Style.Font.Size = 12;
                ws.Cells["A3:C3"].Style.Fill.PatternType=ExcelFillStyle.Solid;
                ws.Cells["A3:C3"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.SkyBlue);
                ws.Cells["A3:C3"].Style.VerticalAlignment=ExcelVerticalAlignment.Center;
                ws.Cells["A3:C3"].Style.HorizontalAlignment=ExcelHorizontalAlignment.Center;

                pck.Save();
                fileContents = pck.GetAsByteArray();
            }
            return Convert.ToBase64String(fileContents);
        }
    }
}
