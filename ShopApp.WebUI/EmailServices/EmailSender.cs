using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace ShopApp.WebUI.EmailServices
{
	public class EmailSender : IEmailSender
	{
		private const string SendGridKey = "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";
		public Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			return Execute(SendGridKey, subject, htmlMessage, email);
		}

		private Task Execute(string sendGridKey, string subject, string message, string email)
		{
			var client = new SendGridClient(sendGridKey);

			var msg = new SendGridMessage()
			{
				From = new EmailAddress("info@shopaap.com", "Shop App"),
				Subject = subject,
				PlainTextContent = message,
				HtmlContent = message
			};
			msg.AddTo(new EmailAddress(email));
			return client.SendEmailAsync(msg);
		}
	}
}
