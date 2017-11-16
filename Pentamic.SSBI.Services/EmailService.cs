using FluentEmail.Core;
using FluentEmail.Razor;
using FluentEmail.Smtp;
using System.Threading.Tasks;

namespace Pentamic.SSBI.Services
{
    public class EmailService
    {
        public EmailService()
        {
            Email.DefaultSender = new SmtpSender();
            Email.DefaultRenderer = new RazorRenderer();
        }
        public async Task SendTextAsync(string from, string to, string subject, string body)
        {
            var result = await Email.From(from).To(to).Subject(subject).Body(body).SendAsync();
            if (result.Successful)
            {
                
            }
        }
        public async Task SendTemplateFileAsync(string from, string to, string subject, string templatePath, object model)
        {
            var email = Email.From(from).To(to).Subject(subject)
                .UsingTemplateFromFile(templatePath, model);
            await email.SendAsync();
        }
        public async Task SendTemplateAsync(string from, string to, string subject, string template, object model)
        {
            var email = Email.From(from).To(to).Subject(subject)
                .UsingTemplate(template, model);
            await email.SendAsync();
        }
    }
}