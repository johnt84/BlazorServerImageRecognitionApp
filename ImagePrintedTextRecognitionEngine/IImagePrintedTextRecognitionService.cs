using ImagePrintedTextRecognitionShared;

namespace ImageRecognitionEngine
{
    public interface IImagePrintedTextRecognitionService
    {
        Task<ImageRecognitionOutput> UploadFileAndConvertToText(ImageRecognitionInput imageRecognitionInput);
    }
}
