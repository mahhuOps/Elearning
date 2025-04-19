using EmailService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailService.Interfaces
{
    public interface IEmailService
    {
        /// <summary>
        /// Gửi mail
        /// </summary>
        /// <param name="mailConfig">Mail config server</param>
        /// <param name="mail">Model gửi mail</param>
        /// <returns>Gửi thành công hay thất bại</returns>
        Task<bool> SendMail(MailConfig mailConfig, MailModel mail);

        /// <summary>
        /// Kiểm tra thiết lập mail server
        /// </summary>
        /// <param name="mailConfig">Cấu hình mail server</param>
        /// <returns>Hợp lệ Hoặc Không hợp lệ</returns>
        Task<bool> ValidateMailConfig(MailConfig mailConfig);
    }
}
