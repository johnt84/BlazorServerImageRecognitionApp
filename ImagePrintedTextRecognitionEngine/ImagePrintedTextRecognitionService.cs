using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using ImagePrintedTextRecognitionShared;

namespace ImageRecognitionEngine
{
    public class ImagePrintedTextRecognitionService : IImagePrintedTextRecognitionService
    {
        public async Task<ImageRecognitionOutput> UploadFileAndConvertToText(ImageRecognitionInput imageRecognitionInput)
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

            return await GetPrintedTextFromUploadStream(imageRecognitionInput);
        }

        private async Task<ImageRecognitionOutput> GetPrintedTextFromUploadStream(ImageRecognitionInput imageRecognitionInput)
        {
            try
            {
                var computerVision = new ComputerVisionClient(new ApiKeyServiceClientCredentials(imageRecognitionInput.SubscriptionKey))
                {
                    Endpoint = imageRecognitionInput.AzureEndpointURL,
                };

                OcrResult printedTextAnalysis = await computerVision.RecognizePrintedTextInStreamAsync(true, imageRecognitionInput.UploadImageFileStream);

                string printedTextInImage = GetPrintedTextOutput(printedTextAnalysis);

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

        private string GetPrintedTextOutput(OcrResult analysis)
        {
            string printedTextInImage = string.Empty;

            foreach (var region in analysis.Regions)
            {
                foreach (var line in region.Lines)
                {
                    printedTextInImage += $"{string.Join(" ", line.Words.Select(x => x.Text).ToList())}\n";
                }

                printedTextInImage += "\n";
            }

            return printedTextInImage;
        }
    }
}