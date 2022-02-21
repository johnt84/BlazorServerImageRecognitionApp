namespace ImagePrintedTextRecognitionShared
{
    public class ImageRecognitionInput
    {
        public string SubscriptionKey { get; set; } = string.Empty;
        public string AzureEndpointURL { get; set; } = string.Empty;
        public Stream UploadImageFileStream { get; set; }
    }
}
