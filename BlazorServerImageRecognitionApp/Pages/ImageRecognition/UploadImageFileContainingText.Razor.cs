using ImagePrintedTextRecognitionShared;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Headers;

namespace BlazorServerImageRecognitionApp.Pages.ImageRecognition
{
    public partial class UploadImageFileContainingText
    {
        private ImageRecognitionOutput imageRecognitionOutput = new ImageRecognitionOutput();
        private bool anyFileUploaded = false;
        private bool currentFileUploaded = false;

        private async Task OnInputFileChangeAsync(InputFileChangeEventArgs e)
        {
            imageRecognitionOutput = new ImageRecognitionOutput();
            anyFileUploaded = true;
            currentFileUploaded = false;

            int.TryParse(configuration["MaxImageFileSize"], out int maxImageFileSize);

            if (e.File.Size > maxImageFileSize)
            {
                imageRecognitionOutput = new ImageRecognitionOutput()
                {
                    ErrorMessage = $"The file size is {e.File.Size} bytes, this is more than the allowed limit of {maxImageFileSize} bytes.",
                };

                currentFileUploaded = true;

                return;
            }
            else if (e.File == null || !e.File.ContentType.Contains("image"))
            {
                imageRecognitionOutput = new ImageRecognitionOutput()
                {
                    ErrorMessage = "Please upload a valid image file",
                };

                currentFileUploaded = true;

                return;
            }

            try
            {
                var fileContent = new StreamContent(e.File.OpenReadStream(maxImageFileSize));

                fileContent.Headers.ContentType = new MediaTypeHeaderValue(e.File.ContentType);

                var uploadImageFileStream = await fileContent.ReadAsStreamAsync();

                var imageRecognitionInput = new ImageRecognitionInput()
                {
                    SubscriptionKey = configuration["SubscriptionKey"].ToString(),
                    AzureEndpointURL = configuration["AzureEndpointURL"].ToString(),
                    UploadImageFileStream = uploadImageFileStream,
                };

                imageRecognitionOutput = await imagePrintedTextRecognitionService.UploadFileAndConvertToTextAsync(imageRecognitionInput);

                var imageFile = await e.File.RequestImageFileAsync("image/jpeg", 680, 480);
                using var fileStream = imageFile.OpenReadStream(maxImageFileSize);

                using var memoryStream = new MemoryStream();
                await fileStream.CopyToAsync(memoryStream);

                imageRecognitionOutput.ImageUrl = string.Concat("data:image/png;base64,", Convert.ToBase64String(memoryStream.ToArray()));

                currentFileUploaded = true;
            }
            catch (Exception ex)
            {
                imageRecognitionOutput = new ImageRecognitionOutput()
                {
                    ErrorMessage = ex.Message,
                };

                currentFileUploaded = true;
            }
        }
    }
}
