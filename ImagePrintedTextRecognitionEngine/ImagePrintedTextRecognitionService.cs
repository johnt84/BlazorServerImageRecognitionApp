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

                var printedTextInImage = GetPrintedTextOutput(printedTextAnalysis);

                return new ImageRecognitionOutput()
                {
                    IsSuccesful = true,
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

        private PrintedTextInImage GetPrintedTextOutput(OcrResult analysis)
        {
            var printedTextInImage = new PrintedTextInImage();
            
            foreach (var region in analysis.Regions)
            {
                var paragraphInImage = new ParagraphInImage();

                foreach (var line in region.Lines)
                {
                    paragraphInImage.Sentences.Add(string.Join(" ", line.Words.Select(x => x.Text).ToList()));
                }

                printedTextInImage.ParagraphInImages.Add(paragraphInImage);
            }

            return printedTextInImage;
        }
    }
}