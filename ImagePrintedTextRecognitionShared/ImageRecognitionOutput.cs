namespace ImagePrintedTextRecognitionShared
{
    public class ImageRecognitionOutput
    {
        public bool IsSuccesful { get; set; }
        public string PrintedTextInImage { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;

    }
}