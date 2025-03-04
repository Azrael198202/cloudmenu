using System.Security.Cryptography.X509Certificates;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;

#nullable disable

namespace cloudmenu_api.RequestModels
{

    /*****
    *       ファイルアップロード
    *****/
    public class ReqFileGet
    {
        public string imgPath { get; set; }
    }
    public class ReqFile
    {
        public string bucketName { get; set; }
        public IFormFile model { get; set; }
    }
}
