using ImagePrintedTextRecognitionShared;
using ImageRecognitionEngine;
using Microsoft.Extensions.Configuration;

try
{
    var builder = new ConfigurationBuilder()
                               .SetBasePath($"{Directory.GetCurrentDirectory()}/../../..")
                               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

    var config = builder.Build();

    using (var fileStream = new FileStream(config["ImageFilePath"], FileMode.Open, FileAccess.Read))
    {
        var imageRecognitionInput = new ImageRecognitionInput()
        {
            SubscriptionKey = config["AzureSubscriptionKey"],
            AzureEndpointURL = config["AzureEndpointURL"],
            UploadImageFileStream = fileStream,
        };

        var imagePrintedTextRecognitionService = new ImagePrintedTextRecognitionService();
        var imageRecognitionOutput = await imagePrintedTextRecognitionService.UploadFileAndConvertToTextAsync(imageRecognitionInput);

        Console.WriteLine("Printed text in image: \n");

        if (imageRecognitionOutput != null)
        {
            if (!string.IsNullOrWhiteSpace(imageRecognitionOutput.ErrorMessage))
            {
                Console.WriteLine($"Error: {imageRecognitionOutput.ErrorMessage}");
            }
            else if (!string.IsNullOrWhiteSpace(imageRecognitionOutput.PrintedTextInImage))
            {
                Console.WriteLine(imageRecognitionOutput.PrintedTextInImage);
            }
            else
            {
                Console.WriteLine("No printed text found in image file");
            }
        }
        else
        {
            Console.WriteLine("Printed text in image file could not be found");
        }
    }
}
catch (FileNotFoundException ioEx)
{
    Console.WriteLine(ioEx.Message);
}
