using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using ImagePrintedTextRecognitionShared;

namespace ImageRecognitionEngine
{
    public class ImagePrintedTextRecognitionService : IImagePrintedTextRecognitionService
    {
        public async Task<ImageRecognitionOutput> UploadFileAndConvertToTextAsync(ImageRecognitionInput imageRecognitionInput)
        {
            if(imageRecognitionInput == null || string.IsNullOrWhiteSpace(imageRecognitionInput.SubscriptionKey) 
                || string.IsNullOrWhiteSpace(imageRecognitionInput.AzureEndpointURL) 
                || imageRecognitionInput.UploadImageFileStream == null)
            {
                return new ImageRecognitionOutput()
                {
                    IsSuccesful = false,
                    ErrorMessage = $"Invalid image fileupload",
                };
            }

            return await GetPrintedTextFromUploadStreamAsync(imageRecognitionInput);
        }

        private async Task<ImageRecognitionOutput> GetPrintedTextFromUploadStreamAsync(ImageRecognitionInput imageRecognitionInput)
        {
            try
            {
                var client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(imageRecognitionInput.SubscriptionKey))
                {
                    Endpoint = imageRecognitionInput.AzureEndpointURL,
                };

                var result = await GetReadResultsFromStreamAsync(client, imageRecognitionInput);

                if(result.Status != OperationStatusCodes.Succeeded)
                {
                    return new ImageRecognitionOutput()
                    {
                        IsSuccesful = false,
                        PrintedTextInImage = string.Empty,
                    };
                }

                string printedTextInImage = GetPrintedTextOutput(result.AnalyzeResult.ReadResults);

                return new ImageRecognitionOutput()
                {
                    IsSuccesful = !string.IsNullOrWhiteSpace(printedTextInImage),
                    PrintedTextInImage = printedTextInImage,
                };
            }
            catch(Exception ex)
            {
                return new ImageRecognitionOutput()
                {
                    IsSuccesful = false,
                    ErrorMessage = ex.Message,
                };
            }
        }

        private async Task<ReadOperationResult> GetReadResultsFromStreamAsync(ComputerVisionClient client, ImageRecognitionInput imageRecognitionInput)
        {
            var textHeaders = await client.ReadInStreamAsync(imageRecognitionInput.UploadImageFileStream);

            string operationLocation = textHeaders.OperationLocation;

            const int numberOfCharsInOperationId = 36;
            string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);

            ReadOperationResult result;

            do
            {
                result = await client.GetReadResultAsync(Guid.Parse(operationId));
            }
            while ((result.Status == OperationStatusCodes.Running ||
                result.Status == OperationStatusCodes.NotStarted));

            return result;
        }

        private string GetPrintedTextOutput(IList<ReadResult> textUrlFileResults)
        {
            string printedTextInImage = string.Empty;

            foreach (ReadResult page in textUrlFileResults)
            {
                foreach (Line line in page.Lines)
                {
                    printedTextInImage += $"{line.Text}\n";
                }
            }

            return printedTextInImage;
        }
    }
}