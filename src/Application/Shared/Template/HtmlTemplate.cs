using art_tattoo_be.Application.DTOs.Studio;
using art_tattoo_be.Application.DTOs.User;

namespace art_tattoo_be.Application.Template;

public class HtmlTemplate
{
  public static string HtmlEmailTemplate(BecomeStudioReq studio)
  {
    string htmlContent = $@"
      <!DOCTYPE html>
      <html>
      <head>
          <title>Yêu cầu trở thành studio</title>
      </head>
      <body>
          <p>
              Xin chào, chúng tôi muốn được trở thành studio của hệ thống Art Tattoo.
          </p>
          <p>
              Dưới đây là thông tin của chúng tôi:
          </p>
          <ul>
              <li>Tên: {studio.Name}</li>
              <li>Địa chỉ: {studio.Address}</li>
              <li>Điện thoại: {studio.Phone}</li>
              <li>Email: {studio.Email}</li>
              <li>Website: {studio.Website}</li>
              <li>Facebook: {studio.Facebook}</li>
              <li>Instagram: {studio.Instagram}</li>
          </ul>

          <p>
              Xin vui lòng liên hệ với chúng tôi qua:
          </p>
          <ul>
              <li>Tên: {studio.ContactName}</li>
              <li>Điện thoại: {studio.ContactPhone}</li>
              <li>Email: {studio.ContactEmail}</li>
          </ul>

          <p>
              Xin cảm ơn.
          </p>
          <a href='{studio.RedirectUrl}'>Xem thêm</p>
      </body>
      </html>";

    return htmlContent;
  }


  public static string HtmlEmailReportUserTemplate(UserReport userReport)
  {
    string htmlContent = $@"
      <!DOCTYPE html>
      <html>
      <head>
          <title>Báo cáo người dùng không phù hợp</title>
      </head>

      <body>
          <p>
              Xin chào, chúng tôi muốn báo cáo người dùng không phù hợp.
          </p>
          <p>
              Dưới đây là thông tin của người dùng:
          </p>
          <ul>
              <li>Id: {userReport.Id}</li>
              <li>Tên: {userReport.FullName}</li>
              <li>Email: {userReport.Email}</li>
              <li>Số điện thoại: {userReport.PhoneNumber}</li>
          </ul>

          <p>
              Xin cảm ơn.
          </p>
          <a href='{userReport.RedirectUrl}'>Xem thêm</p>
      </html>";

    return htmlContent;
  }
}
