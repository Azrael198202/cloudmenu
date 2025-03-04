using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using cloudmenu_api.DbContextCloudMenu;
using cloudmenu_api.RequestModels;
using cloudmenu_api.DbEntitiesCloudMenu;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;


namespace cloudmenu_api.Controllers
{
    [ApiController]
    [Route("api/lab")]
    public class LabController : ControllerBase
    {

        private readonly AppDbContext _dbContext;
        private readonly ILogger<OrderController> _logger;
        private IConfiguration _configuration;
        public LabController(IConfiguration Configuration, ILogger<OrderController> logger, AppDbContext dbContext)
        {
            _configuration = Configuration;
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpPost("lab1.do")]
        public async Task<List<MCategory>> lab1(ReqMdLab reqDto)
        {
            _logger.LogDebug("LabController-lab1 request:{0}", reqDto);
            // will get error
            String selectSql = $@"
                SELECT
                    *
                FROM
                    m_category
            ";

            var quaryRslt = _dbContext.MCategories.FromSqlRaw(selectSql);
            var rsltLst = await quaryRslt.ToListAsync();

            return rsltLst;
        }

        [HttpPost("lab2.do")]
        public ReqMdLab lab2(ReqMdLab reqDto)
        {
            _logger.LogDebug("LabController-lab2 request:{0}", reqDto);
            return reqDto;
        }

        [HttpGet("createCategoryConst.do")]
        public string createCategoryConst()
        {
            var ctgCQuery = from ctgC in _dbContext.MCategoryClasses
                            select ctgC
                            ;
            var ctgCList = ctgCQuery.ToList();

            // var ctgCList = ctgCQuery.ToList();

            StringBuilder categoryClassStr = new StringBuilder();

            foreach (var ctgc in ctgCList)
            {
                string ctgClssName = Regex.Replace(ctgc.CategoryClassName, @"["" "",、,・,（,）,:]", "");
                categoryClassStr.AppendLine($"public static class C_{ctgc.CategoryClassNumber}_{ctgClssName}");
                categoryClassStr.AppendLine("{");
                categoryClassStr.AppendLine($"public static string value = \"{ctgc.CategoryClassNumber}\";");
                var ctgQuery = from ctg in _dbContext.MCategories
                               where ctg.CategoryClassNumber == ctgc.CategoryClassNumber
                               select ctg;

                var ctgList = ctgQuery.ToList();

                foreach (var ctg in ctgList)
                {
                    string ctgName = Regex.Replace(ctg.CategoryKbnName, @"["" "",、,・,（,）,:]", "");
                    categoryClassStr.AppendLine($"public static string ctg_{ctg.CategoryKbn}_{ctgName} = \"{ctg.CategoryKbn}\";");
                }

                categoryClassStr.AppendLine("}");
            }

            return categoryClassStr.ToString();
        }
        [HttpGet("getDbVersion")]
        public string getDbVersion()
        {
            var connectStr = _configuration.GetConnectionString("DefaultConnection");
            var index = connectStr.IndexOf("password");

            return connectStr.Substring(0, index);
        }

    }
}
