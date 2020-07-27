using FluentEmail.Core;
using FluentEmail.Razor;
using FluentEmail.Smtp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace Pentamic.SSBI.Services
{
    public class EmailService
    {
        public EmailService()
        {
            Email.DefaultSender = new SmtpSender();
            Email.DefaultRenderer = new RazorRenderer();
        }
        public async Task SendAsync(string to, string subject, string templateName, object model)
        {
            var path = HttpContext.Current.Server.MapPath("~/Templates");
            var templatePath = Path.Combine(path, templateName + ".cshtml");
            var email = Email.From("bisystem@pentamic.vn")
                .To(to).Subject(subject)
                .UsingTemplateFromFile(templatePath, model);
            await email.SendAsync();
        }
    }
}