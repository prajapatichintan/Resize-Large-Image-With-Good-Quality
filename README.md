#   Resize Large Image With Good Quality
Introduction:-
Now days we need to store images on cloud and then show on many places like if create E-commerce web site then image galleries can be an effective method of communicating with visitor. But if we upload large size images then its increase the cost of storage and take time to load on browser so A nice compromise to this situation is to present images as thumbnails.This article show how to Resize large image with good quality.




Follow these simple steps:-

Step 1:-
         Select Image which you want to Resize and upload on server.




Step 2:-

    Go to Controller section and add following assemblies



  Create function of “ResizeImage()” in controller 



using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;   
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
Return myfile; 
}

Step 3:-

For we use large images for resizing so we need following settings on web.config file.

3.1




3.2




3.3

Some time we facing issue of the garbage collection and memory when we upload large size of images so we used the following things for remove garbage collection and other space.

 GC.Collect();
 .Dispose();


 Step 4:-

Currently I was store resized image in folder so first create one folder in our solution where we need to store resized images. 


Note:- In this article we store resized images on our server but if we need to store resized images on cloud then please refer  my next article “ Upload Multiple Images In Azure Blob Storage ”. 


Step 5:-

Create Action in Controller where we call “ResizeImage()” function.

public ActionResult UploadImageThumb(HttpPostedFileBase fileToUpload)
       {
           string myfile= ResizeImage(fileToUpload);
           return RedirectToAction("Index", new { imgName = myfile });
       }
        

 public ActionResult Index(string imgName)
       {
           
            ViewBag.ImgName = imgName;
            return View();
        }









