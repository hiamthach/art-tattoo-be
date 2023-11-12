using art_tattoo_be.Application.DTOs.Studio;

namespace art_tattoo_be.Application.Template;

public class BecomeStudioTemplate
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

}
