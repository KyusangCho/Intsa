using Intsa.Shared.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;

namespace Intsa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private IHostingEnvironment hostingEnv;

        public FileUploadController(IHostingEnvironment env)
        {
            this.hostingEnv = env;
        }

        [HttpPost("[action]")]
        public void Save(IList<IFormFile> UploadFiles)
        {
            long size = 0;
            try
            {
                //foreach (var file in UploadFiles)
                //{
                var fileName = Path.GetFileName(UploadFiles[0].FileName);

                IFormFile file = UploadFiles[0];
                string name = file.FileName;
                string fullName = Path.Combine(Path.GetTempPath(), name);
                string fileKey = string.Concat(Path.GetFileNameWithoutExtension(fullName), "_", 
                                               DateTime.Now.ToString("yyMMddHHmmddsss"), 
                                               Path.GetExtension(fullName)); 
                    //var filename = ContentDispositionHeaderValue
                    //        .Parse(file.ContentDisposition)
                    //        .FileName
                    //        .Trim('"');
                    //filename = hostingEnv.ContentRootPath + $@"\{filename}";

                if (UploadFiles != null)
                {
                    if (System.IO.File.Exists(fullName))
                    {
                        System.IO.File.Delete(fullName);
                    }

                    if (!System.IO.File.Exists(fullName))
                    {
                        using (FileStream fsSource = new FileStream(Path.Combine(Path.GetTempPath(), fileName), FileMode.Create))
                        {
                            UploadFiles[0].CopyTo(fsSource);
                            fsSource.Close();
                        }
                        AmazonS3.Main(fileKey, Path.Combine(Path.GetTempPath(), fileName));       // 아마존 S3에 저장 
                        System.IO.File.Delete(fullName);
                    }
                }
                    
                    //using (FileStream fs = System.IO.File.Create(filename))
                    //    {
                    //        file.CopyTo(fs);
                    //        fs.Flush();
                    //    }
                    //}
                //}
            }
            catch (Exception e)
            {
                Response.Clear();
                Response.StatusCode = 204;
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File failed to upload";
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = e.Message;
            }
        }

        [HttpPost("[action]")]
        public void Remove(IList<IFormFile> UploadFiles)
        {
            try
            {
                var filename = hostingEnv.ContentRootPath + $@"\{UploadFiles[0].FileName}";
                if (System.IO.File.Exists(filename))
                {
                    System.IO.File.Delete(filename);
                }
            }
            catch (Exception e)
            {
                Response.Clear();
                Response.StatusCode = 200;
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File removed successfully";
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = e.Message;
            }
        }
    }
}
