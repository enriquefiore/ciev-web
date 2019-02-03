using System;
using System.IO;
using System.Net;
using ciev.Helpers;
using ciev.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Slackbot;

namespace ciev.Controllers
{
    public class MonitorController : Controller
    {
        public IActionResult Index()
        {
            var helper = new GenericHelper();
            var file = string.Format("{0}tmp/log.json", HttpContext.Request.GetEncodedUrl().Split('M')[0]);
            var log = new LogClass { LastDate = Convert.ToString(helper.ConvertTimezone(DateTime.MinValue)) };
            using(var client = new WebClient()) 
            {
                var text = client.DownloadString(file);
                if (!string.IsNullOrEmpty(text))
                    log = JsonConvert.DeserializeObject<LogClass>(text);

                var compare = helper.ConvertTimezone(DateTime.Now).Subtract(helper.ConvertTimezone(Convert.ToDateTime(log.LastDate)));
                if(compare.TotalMinutes > 5) 
                {
                    ViewBag.Message = string.Format("ATENÇÃO! Sensores inoperantes há {0} minutos.", compare.TotalMinutes.ToString().Split('.')[0]);
                    using (var messagebot = new Bot("token", "name"))
                        messagebot.SendMessage("a", "message");
                }

            }
            return View(log);
        }

        [HttpPost]
        public IActionResult Index(string deviceId)
        {
            var helper = new GenericHelper();
            var path = @"wwwroot/tmp/log.json";
            var connect = string.Format("{0}@{1}", helper.ConvertTimezone(DateTime.Now), HttpContext.Connection.RemoteIpAddress);
            var logItem = new LogClass { DeviceId = deviceId, LastConn = connect };
            var message = JsonConvert.SerializeObject(logItem);

            using (StreamWriter writer = new StreamWriter(path, false))
                writer.WriteLine(message);
                
            return View(logItem);
        }
    }
}
