using common;
using Microsoft.AspNetCore.Mvc;
using myMVC.Models;
using System.Diagnostics;

namespace myMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(string biblioPath)
        {
            string fieldMap = @"ISBN|010$a
题名|200$a
第一作者|200$f
个人主要作者|701$a";

            List<FieldItem> fieldList = new List<FieldItem>();
            try
            {
                fieldList = MarcHelper.ParseFieldMap(fieldMap);
            }
            catch (Exception ex)
            {
                @ViewData["marcField"] = "解析marc字段配置规则出错：" + ex.Message;
                return View();

            }

            string html = "";

            foreach (FieldItem field in fieldList)
            {
                html += "<tr>"
        + "<td class='label'>" + field.Caption + "</td>"
        + "<td>"
            + "<input  class='_field'  id='"+ field.Caption + "|"+field.Field+field.Subfield+"' type='text' value='" + field.Value + "'>"
        + "</td>"
    + "</tr>";
            }

            if (string.IsNullOrEmpty(biblioPath) == true)
            {
                html += "<tr>"
    + "<td class='label'>目标数据库</td>"
    + "<td>"
        + "<div style='border:1px solid #cccccc;'>"
            + "<select id='_selDbName'>"
                + "<option value='中文图书' selected >中文图书</option>"
                + "<option value='测试库'>测试库</option>"
            + "</select>"
        + "</div>"
    + "</td>"
+ "</tr>";
            }

            // 操作按钮
            html += "<tr>"
    + "<td colspan='2'>"
        + "<button class='mui-btn mui-btn-primary' onclick=\"saveField()\">保存</button>&nbsp;&nbsp;"
        + "<button class='mui-btn mui-btn-default' onclick=\"cancelEdit()\">取消</button>"
    + "</td>"
+ "</tr>";

            html = "<table id='_marcEditor'>"
                + html
                + "</table>";


            @ViewData["marcField"] =html;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}