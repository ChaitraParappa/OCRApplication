using IronOcr;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OCRAPIApplication.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            try
            {
                var Ocr = new AutoOcr();
                string extension = Path.GetExtension(file.FileName);
                file.SaveAs(Path.Combine(Server.MapPath("~/App_Data/Files"), file.FileName));
                string filepath = Path.Combine(Server.MapPath("~/App_Data/Files"), file.FileName);

                if (extension == ".jpg" || extension == ".png")
                {

                    var Result = Ocr.Read(filepath);
                   
                    WritingContentsToFile(file, Result.Text);
                    // ViewBag.message = Result.Text;
                }
                if (extension == ".pdf")
                {
                    var Results = Ocr.ReadPdf(filepath);
                    var Barcodes = Results.Barcodes;
                    var Text = Results.Text;

                    WritingContentsToFile(file, Results.Text);
                }
                return View("Index");
            }catch(Exception e)
            {
                return null;
            }
        }

        private void WritingContentsToFile(HttpPostedFileBase file,string result)
        {
			string File;
            string fileName = file.FileName + ".txt";
            file.SaveAs(Path.Combine(Server.MapPath("~/App_Data/Files"), fileName));
            string filePathOfText = Path.Combine(Server.MapPath("~/App_Data/Files"), fileName);
            FileStream fs1 = new FileStream(filePathOfText, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter writer = new StreamWriter(fs1);
            writer.Write(result);
            writer.Close();

        }
    }
}
