namespace art_tattoo_be.Core.GCS;

using Google.Cloud.Storage.V1;
using Google.Apis.Auth.OAuth2;
using System;
using System.IO;
using System.Threading.Tasks;

public interface IGCSService
{
  Task<string?> UploadFileAsync(Stream fileStream, string destinationFileName, string contentType);
}

public class GCSService : IGCSService
{
  private readonly string BucketName;
  private readonly StorageClient StorageClient;

  public GCSService()
  {
    // Initialize the Google Cloud Storage client
    string projectId = "arttattoolover-adf51";
    string credentialsPath = "firebase.json"; // Replace with your Service Account Key path

    var credentials = GoogleCredential.FromFile(credentialsPath);
    StorageClient = StorageClient.Create(credentials);
    BucketName = projectId + ".appspot.com"; // Default Firebase Storage bucket
  }

  public async Task<string?> UploadFileAsync(Stream fileStream, string destinationFileName, string contentType)
  {
    try
    {
      var objectName = destinationFileName;

      // Upload the file to Firebase Storage
      var options = new UploadObjectOptions
      {
        PredefinedAcl = PredefinedObjectAcl.PublicRead // Set the PredefinedAcl to PublicRead
      };
      await StorageClient.UploadObjectAsync(BucketName, objectName, contentType, fileStream, options);

      // Generate a download URL for the uploaded file
      var objectInfo = await StorageClient.GetObjectAsync(BucketName, objectName);
      var downloadUrl = objectInfo.MediaLink;

      return downloadUrl;
    }
    catch (Exception ex)
    {
      // Handle any exceptions that may occur during the upload
      Console.WriteLine($"Error uploading file: {ex.Message}");
      return null;
    }
  }
}
