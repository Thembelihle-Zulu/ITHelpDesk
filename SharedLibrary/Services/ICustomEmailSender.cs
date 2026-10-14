using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Services
{
    public interface ICustomEmailSender
    {
        Task SendEmailAsync(string to, string subject, string body);
    }

}
