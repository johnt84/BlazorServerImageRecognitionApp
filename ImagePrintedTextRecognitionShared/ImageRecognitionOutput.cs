namespace ImagePrintedTextRecognitionShared
{
    public class ImageRecognitionOutput
    {
        public bool IsSuccesful { get; set; }
        public PrintedTextInImage PrintedTextInImage { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
}