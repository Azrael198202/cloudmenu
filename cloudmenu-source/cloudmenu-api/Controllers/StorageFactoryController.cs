using System;
using LiteX.Storage.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using cloudmenu_api.RequestModels;
using cloudmenu_api.LwUtils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using cloudmenu_api.ResponseModels;

/// <summary>
/// Storage controller
/// </summary>
[ApiController]
[Route("api/file")]
public class StorageController : Controller
{
    #region Fields

    private readonly ILiteXBlobService _blobService;
    public IConfiguration Configuration { get; }
    private readonly ILogger<StorageController> _logger;

    #endregion

    #region Ctor

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="blobService"></param>
    public StorageController(ILogger<StorageController> logger, ILiteXBlobService blobService, IConfiguration configuration)
    {
        _logger = logger;
        _blobService = blobService;
        Configuration = configuration;
        _logger.LogInformation(1, "NLog injected into StorageController");
    }

    #endregion

    #region Methods

    /// <summary>
    /// アップロード(画像)
    /// 請求form-data
    /// </summary>
    /// <param name="model">Blob file</param>
    /// <returns></returns>
    [HttpPost("uploadImage")]
    public async Task<RpnImgFilePath> UploadBlob([FromForm] ReqFile reqf)
    {
        RpnImgFilePath rpnImgFilePath = new RpnImgFilePath();
        if (reqf.model != null)
        {
            //　アップロード画像名
            string blobName = reqf.model.FileName;

            //  画像リネーム
            string UUID = Guid.NewGuid().ToString();
            string imageFileName = UUID + blobName;

            Stream stream = reqf.model.OpenReadStream();

            string contentType = reqf.model.ContentType;

            //  bucket
            string backetname = reqf.bucketName;
            string imagePath = Configuration["FileSystemStorageConfig:Directory"] + backetname;

            BlobProperties properties = new BlobProperties
            {
                ContentType = contentType
            };

            // アップロード処理
            var isUploaded = await _blobService.UploadBlobAsync(imagePath, imageFileName, stream, properties);

            if (isUploaded)
            {
                rpnImgFilePath = new RpnImgFilePath
                {
                    imgFilePath = backetname + ConstCode.STRING_SLASH + imageFileName
                };
                return rpnImgFilePath;

            }

            return rpnImgFilePath;
        }
        else
        {
            return rpnImgFilePath;
        }
    }
    /// <summary>
    /// 画像取得
    /// </summary>
    /// <param name="blobName">Name of the Blob.</param>
    /// <returns></returns>
    [HttpGet("getImage/{storeNumber}/{imgName}")]
    public async Task<IActionResult> DownloadBlob(string storeNumber, string imgName)
    {
        _logger.LogInformation(1, $"DownloadBlob:getImage/{storeNumber}/{imgName}");
        Stream stream = null;
        // byte[] arr = null;
        try
        {
            if (!String.IsNullOrEmpty(imgName))
            {
                string imagePath = Configuration["FileSystemStorageConfig:Directory"] + storeNumber;

                // get blob
                stream = await _blobService.GetBlobAsync(imagePath, imgName);

                // arr = new byte[stream.Length];
                // stream.Position = 0;
                // stream.Read(arr, 0, (int)stream.Length);

                // var response = File(stream, "application/json", reqg.blobName); // FileStreamResult
                string contentType = imgName.EndsWith(".pdf") ? "application/pdf" : "application/octet-stream";
                return File(stream, contentType);
                // return Ok(Convert.ToBase64String(arr));
            }
            return NotFound("画像取得失敗");
        }
        catch (IOException e)
        {
            Console.WriteLine(e.Message);
            return NotFound("画像取得失敗");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return NotFound("画像取得失敗");
        }
        // finally
        // {
        //     if (stream != null)
        //     {
        //         try
        //         {
        //             // 画像stream close
        //             stream.Close();
        //         }
        //         catch (IOException e)
        //         {
        //             Console.WriteLine(e.Message);
        //         }
        //     }
        // }
    }

    /// <summary>
    /// 画像削除
    /// </summary>
    /// <param name="blobName">Name of the Blob.</param>
    /// <returns></returns>
    [HttpPost("delImage")]
    public async Task<IActionResult> DeleteBlobFile(ReqFileGet reqg)
    {
        try
        {
            // bucketName
            string bucketName = string.Empty;
            // 画像名
            string fileName = string.Empty;
            if (reqg != null)
            {
                // '/' 割り切り
                String[] split = reqg.imgPath.Split(ConstCode.STRING_SLASH);
                if (split.Length >= ConstCode.NUM_2)
                {
                    bucketName = split[ConstCode.NUM_0];
                    fileName = split[ConstCode.NUM_1];
                }
                else
                {
                    fileName = reqg.imgPath;
                }
                string imagePath = Configuration["FileSystemStorageConfig:Directory"] + bucketName;
                var isDeleted = await _blobService.DeleteBlobAsync(imagePath, fileName);

                if (isDeleted)
                {
                    // 削除成功処理
                }
                else
                {
                    // 削除失敗処理
                }
                return Ok(isDeleted);
            }
            else
            {
                return NotFound();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return NotFound("画像削除失敗");
        }
    }
    /// <summary>
    /// アップロード(画像)
    /// 請求form-data
    /// </summary>
    /// <param name="model">Blob file</param>
    /// <returns></returns>
    [HttpPost("SaveSignature")]
    public async Task<RpnMdBase> SaveSignature(ReqSignature reqf)
    {
        RpnMdBase rpnDto = new RpnMdBase();
        string imagePath = Configuration["FileSystemStorageConfig:Directory"] + reqf.storeNumber;
        string imageFileName = $"{reqf.storeNumber}_{reqf.receptionNumber}_signature.jpeg";
        BlobProperties properties = new BlobProperties
        {
            ContentType = "image/jpeg"
        };

        try
        {
            string base64String = reqf.signatureJpgStr.Split(',')[1];
            byte[] bytes = Convert.FromBase64String(base64String);
            MemoryStream stream = new MemoryStream(bytes);
            // アップロード処理
            var isUploaded = await _blobService.UploadBlobAsync(imagePath, imageFileName, stream, properties);
        }
        catch (System.Exception ex)
        {

            _logger.LogDebug(1, ex.Message);
        }

        return rpnDto;
    }
    #endregion
}
