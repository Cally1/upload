using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace webUpload_layui.Controllers
{
    public class MutipleUploadController : Controller
    {
        // GET: MutipleUpload
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ProcessRequest(HttpPostedFileBase Filedata)
        {
            object ddd = Request.Files.Count;
            if (Filedata == null)
            {
                Filedata = Request.Files[0];
            }
            // 没有文件上传，直接返回
            if (Filedata == null || string.IsNullOrEmpty(Filedata.FileName) || Filedata.ContentLength == 0)
            {
                return HttpNotFound();
            }
            //获取文件完整文件名(包含绝对路径)
            //文件存放路径格式：/files/upload/{日期}/{md5}.{后缀名}
            //例如：/files/upload/20130913/43CA215D947F8C1F1DDFCED383C4D706.jpg
            string fileMD5 = DateTime.Now.Millisecond.ToString(); //CommonFuncs.GetStreamMD5(Filedata.InputStream);
            string FileEextension = Path.GetExtension(Filedata.FileName);
            string uploadDate = DateTime.Now.ToString("yyyyMMdd");
            string imgType = Request["imgType"];
            string virtualPath = "/";
            //if (imgType == "normal")

            //{
            virtualPath = string.Format("~/files/upload/{0}/{1}{2}", uploadDate, fileMD5, FileEextension);
            //}
            //else
            //{
            //    virtualPath = string.Format("~/files/upload2/{0}/{1}{2}", uploadDate, fileMD5, FileEextension);
            //}
            string fullFileName = this.Server.MapPath(virtualPath);
            //创建文件夹，保存文件
            string path = Path.GetDirectoryName(fullFileName);
            Directory.CreateDirectory(path);
            if (!System.IO.File.Exists(fullFileName))
            {
                Filedata.SaveAs(fullFileName);
            }
            var data = new { imgtype = imgType, imgpath = virtualPath.Remove(0, 1) };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}