using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using Watermark;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace ResizeLargeImageWithGoodQuality.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string imgName)
        {
            ViewBag.ImgName = imgName;
            return View();
        }

        public ActionResult UploadImageThumb(HttpPostedFileBase fileToUpload)
        {
            string myfile = ResizeImage(fileToUpload);
            return RedirectToAction("Index", new { imgName = myfile });
        }

        public string ResizeImage(HttpPostedFileBase fileToUpload)
        {
            string name = Path.GetFileNameWithoutExtension(fileToUpload.FileName);
            var ext = Path.GetExtension(fileToUpload.FileName);
            string myfile = name + ext;

            try
            {
                using (Image image = Image.FromStream(fileToUpload.InputStream, true, false))
                {

                    var path = Path.Combine(Server.MapPath("~/resizeImageStore"), myfile);
                    try
                    {
                        //Size can be change according to your requirement 
                        float thumbWidth = 270F;
                        float thumbHeight = 180F;
                        //calculate  image  size
                        if (image.Width > image.Height)
                        {
                            thumbHeight = ((float)image.Height / image.Width) * thumbWidth;
                        }
                        else
                        {
                            thumbWidth = ((float)image.Width / image.Height) * thumbHeight;
                        }

                        int actualthumbWidth = Convert.ToInt32(Math.Floor(thumbWidth));
                        int actualthumbHeight = Convert.ToInt32(Math.Floor(thumbHeight));
                        var thumbnailBitmap = new Bitmap(actualthumbWidth, actualthumbHeight);
                        var thumbnailGraph = Graphics.FromImage(thumbnailBitmap);
                        thumbnailGraph.CompositingQuality = CompositingQuality.HighQuality;
                        thumbnailGraph.SmoothingMode = SmoothingMode.HighQuality;
                        thumbnailGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        var imageRectangle = new Rectangle(0, 0, actualthumbWidth, actualthumbHeight);
                        thumbnailGraph.DrawImage(image, imageRectangle);
                        var ms = new MemoryStream();
                        thumbnailBitmap.Save(path, ImageFormat.Jpeg);
                        ms.Position = 0;
                        GC.Collect();
                        thumbnailGraph.Dispose();
                        thumbnailBitmap.Dispose();
                        image.Dispose();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return myfile;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}