using ImagePrintedTextRecognitionShared;

namespace ImageRecognitionEngine
{
    public interface IImagePrintedTextRecognitionService
    {
        Task<ImageRecognitionOutput> UploadFileAndConvertToTextAsync(ImageRecognitionInput imageRecognitionInput);
    }
}
