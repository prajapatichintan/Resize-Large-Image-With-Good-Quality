Resize Large Image With Good Quality
Introduction:-
Now days we need to store images on cloud and then show on many places like if create E-commerce web site then image galleries can be an effective method of communicating with visitor. But if we upload large size images then its increase the cost of storage and take time to load on browser so A nice compromise to this situation is to present images as thumbnails.This article show how to Resize large image with good quality.

Follow these simple steps:-

Step 1:-
         Select Image which you want to Resize and upload on server.
         Index.cshtml
         
`<div class="jumbotron">
    <h1>Demo Project</h1>
    <p class="lead">Add Watermark Text and Image to Photo  using C#</p>
</div>
<div>
    @using (Html.BeginForm("UploadImageThumb", "Home", FormMethod.Post, new { @class = "form-horizontal", role = "form", enctype = "multipart/form-data" }))
    {
        <label>Select image to upload:</label>
        <input type="file" name="fileToUpload" id="fileToUpload">
        <input type="submit" value="Upload Image" name="submit">

    }
</div>
@if (ViewBag.ImgName != null)
{
    <div>
        <img src="~/resizeImageStore/@ViewBag.ImgName" />
    </div>

}`

Step 2:-

    Go to Controller section and add following assemblies
`using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;   

  Create function of “ResizeImage()” in controller 

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
`
Step 3:-

For we use large images for resizing so we need following settings on web.config file.

3.1
` <system.web>
    <authentication mode="None" />
    <compilation debug="true" targetFramework="4.5" />
    <httpRuntime requestValidationMode="2.0" enableVersionHeader="false" targetFramework="4.5" maxRequestLength="15360000" fcnMode="Single" executionTimeout="900000" />
  </system.web>`
   

 Step 4:-

Currently I was store resized image in folder so first create one folder in our solution where we need to store resized images. 

Step 5:-

Create Action in Controller where we call “ResizeImage()” function.

`public ActionResult UploadImageThumb(HttpPostedFileBase fileToUpload)
       {
           string myfile= ResizeImage(fileToUpload);
           return RedirectToAction("Index", new { imgName = myfile });
       }
        

 public ActionResult Index(string imgName)
       {
           
            ViewBag.ImgName = imgName;
            return View();
        }`



